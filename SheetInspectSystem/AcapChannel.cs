using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avaldata.AcapLibDotNet;
using Avaldata.AcapLibDotNet.Error;
using Avaldata.AcapLibDotNet.Parameters.Id;
using HalconDotNet;

namespace HFVisionInspect
{
    /// <summary>
    /// 取込画像を通知するイベント引数。
    /// ・IsPooled = false : Image の所有権は受け取った側に移る（受け取った側で必ず Dispose する）
    /// ・IsPooled = true  : Image はプールの画像（Dispose 禁止）。数フレーム後に上書きされるので、
    ///                      保持・保存する場合は CopyImage() で複製して使う
    /// </summary>
    public sealed class ImageGrabbedEventArgs : EventArgs
    {
        public ImageGrabbedEventArgs(HImage image, uint dmaCount, uint bufferIndex, bool isPooled)
        {
            Image = image;
            DmaCount = dmaCount;
            BufferIndex = bufferIndex;
            IsPooled = isPooled;
        }

        /// <summary>取込画像</summary>
        public HImage Image { get; }

        /// <summary>true ならプールの画像（Dispose 禁止・いずれ上書きされる）</summary>
        public bool IsPooled { get; }

        /// <summary>DMA 転送完了回数（フレーム番号として利用可能）</summary>
        public uint DmaCount { get; }

        /// <summary>転送されたリングバッファのインデックス</summary>
        public uint BufferIndex { get; }
    }

    /// <summary>AcapLibPlus 関数がエラー（負数）を返した場合の例外</summary>
    public sealed class AcapException : Exception
    {
        public AcapException(string function, int errorCode, string detail)
            : base($"{function} 失敗 (rtn={errorCode}) {detail}")
        {
            Function = function;
            ErrorCode = errorCode;
        }

        /// <summary>エラーが発生した関数名</summary>
        public string Function { get; }

        /// <summary>エラーコード（AVAL_STATUS_xxx）</summary>
        public int ErrorCode { get; }
    }

    /// <summary>
    /// APX-3800 の 1 チャンネル（= IAcapLibDotNet 1 インスタンス）を制御するクラス。
    /// 初期設定 INI ファイルの読込 → Update → FrameEnd イベントで HImage 生成、までを担当します。
    /// 2ch 構成ではこのクラスを 2 つ生成し、それぞれ独立して制御します。
    /// </summary>
    /// <remarks>シリアル通信関連のメソッドは AcapChannel.Serial.cs に分けて記述しています（partial クラス）。</remarks>
    public sealed partial class AcapChannel : IDisposable
    {
		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		
        // ===== AcapLibPlus のエラーコード（マニュアル記載値） =====
		/// <summary>LoadFile: AcapLib2 形式 INI を読込んだが一部キーが変換できなかった（警告扱い）</summary>
		private const int AVAL_STATUS_OUT_OF_RANGE = -20003;

        // ===== AcapLibPlus インスタンス =====
        private readonly IAcapLibDotNet _acap;

        // ===== イベントハンドラのデリゲート =====
        // ネイティブ側から呼ばれるため、GC に回収されないようフィールドで参照を保持しておく
        private readonly IAcapLibDotNet.FrameEndHandler _frameEndHandler;
        private readonly IAcapLibDotNet.LinkDetectHandler _linkDetectHandler;
        private readonly IAcapLibDotNet.LinkLostHandler _linkLostHandler;
        private bool _eventsAttached;

        // ===== Update 後に取得するバッファ情報 =====
        private uint _bufferNum;       // リングバッファ数
        private uint _bufferWidth;     // 横幅[画素]
        private uint _bufferHeight;    // 行数
        private uint _bufferBitCount;  // 1画素のビット幅
        private uint _bufferStride;    // 1行のバイト数

        // ===== 取込統計 =====
        private long _frameCount;            // 受信フレーム数
        private long _lostFrameCount;        // 欠落フレーム数（DmaCount の飛びから算出）
        private long _overwriteSuspectCount; // HImage 化中にリングバッファが一周した疑いのある回数
        private uint _lastDmaCount;
        private bool _hasLastDmaCount;

        // ===== HImage 生成用の作業用バッファ（メモリ増加対策） =====
        // DMA バッファを直接 HALCON に渡さず、一旦ここへコピーしてから HImage を生成する
        private readonly StagingBuffer _staging = new StagingBuffer();

        // ===== 事前確保した HImage のプール（モノクロ時のみ使用、カラー時は null） =====
        // フレームごとの HALCON メモリ確保をなくし、大きい画像でのメモリ増加を防ぐ
        private ImagePool _pool;

        /// <summary>プールのスロット数（表示中の画像が上書きされないよう余裕を持たせる）</summary>
        private const int PoolSlotCount = 4;

        // 作業用バッファ・プールの使用中に解放されないようにするための排他ロック
        private readonly object _convertLock = new object();

        // ===== スナップ（1枚取込）待機用 =====
        private TaskCompletionSource<HImage> _snapTcs;

        // 取込開始/停止の排他用
        private readonly object _acqLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="acap">CDeviceList.GetDeviceChannel() で取得した配列の要素</param>
        /// <param name="name">ログ表示用の名前（"CH1" など）</param>
        public AcapChannel(IAcapLibDotNet acap, string name)
        {
            _acap = acap ?? throw new ArgumentNullException(nameof(acap));
            Name = name;

            // デリゲートを生成して保持（イベント登録/解除で同じインスタンスを使う）
            _frameEndHandler = new IAcapLibDotNet.FrameEndHandler(OnFrameEnd);
            _linkDetectHandler = new IAcapLibDotNet.LinkDetectHandler(OnLinkDetect);
            _linkLostHandler = new IAcapLibDotNet.LinkLostHandler(OnLinkLost);
        }

        // ===================================================================
        // 公開プロパティ・イベント
        // ===================================================================

        /// <summary>チャンネル名（"CH1" など）</summary>
        public string Name { get; }

        /// <summary>Open 済みかどうか</summary>
        public bool IsOpened { get; private set; }

        /// <summary>取込中かどうか</summary>
        public bool IsGrabbing { get; private set; }

        /// <summary>受信フレーム数</summary>
        public long FrameCount => Interlocked.Read(ref _frameCount);

        /// <summary>欠落フレーム数</summary>
        public long LostFrameCount => Interlocked.Read(ref _lostFrameCount);

        /// <summary>リングバッファ上書きの疑いがある回数（0 でなければバッファ数を増やす）</summary>
        public long OverwriteSuspectCount => Interlocked.Read(ref _overwriteSuspectCount);

        /// <summary>バッファ情報の文字列（ログ表示用）</summary>
        public string BufferInfoText =>
            $"W={_bufferWidth} H={_bufferHeight} Bit={_bufferBitCount} Stride={_bufferStride} Buf={_bufferNum}";

        /// <summary>
        /// 連続取込で 1 フレーム受信するごとに発生（AcapLibPlus のイベントスレッドから呼ばれる）。
        /// UI 操作する場合は Invoke / BeginInvoke が必要です。
        /// </summary>
        public event EventHandler<ImageGrabbedEventArgs> ImageGrabbed;

        /// <summary>ログメッセージ通知（任意スレッドから呼ばれる）</summary>
        public event EventHandler<string> LogMessage;

        // ===================================================================
        // 初期化・終了
        // ===================================================================

        /// <summary>
        /// チャンネルをオープンし、INI ファイルを読み込んで設定を反映します。
        /// </summary>
        /// <param name="iniPath">初期設定 INI ファイルのフルパス</param>
        public void Open(string iniPath)
        {
            if (IsOpened) return;
            if (!File.Exists(iniPath)) throw new FileNotFoundException("INI ファイルが見つかりません。", iniPath);

            // --- 1. チャンネルの制御権を取得 ---
            // ※製品登録していない場合は評価版メッセージボックスが表示される
            Check(_acap.Open(), "Open");
            IsOpened = true;

            try
            {
                // --- 2. 初期設定 INI ファイルの読込 ---
                // INI 内の [BufferSettings] に従って InitBuffer も自動実行される
                int rtn = _acap.LoadFile(iniPath);
                if (rtn == AVAL_STATUS_OUT_OF_RANGE)
                {
                    // AcapLib2 形式 INI の一部キーが変換できなかった場合（読込自体は完了）
                    Log($"警告: LoadFile で変換できないキーがありました (rtn={rtn})");
                }
                else
                {
                    Check(rtn, "LoadFile");
                }

                // --- 3. 設定の更新（ボードへ反映＆内部バッファ確保） ---
                Check(_acap.Update(), "Update");

                // --- 4. バッファ情報を取得して保持（HImage 生成時に使用） ---
                Check(_acap.GetBufferNum(out _bufferNum), "GetBufferNum");
                Check(_acap.GetBufferWidth(0, out _bufferWidth), "GetBufferWidth");
                Check(_acap.GetBufferHeight(0, out _bufferHeight), "GetBufferHeight");
                Check(_acap.GetBufferBitCount(0, out _bufferBitCount), "GetBufferBitCount");
                Check(_acap.GetBufferStride(0, out _bufferStride), "GetBufferStride");

                if (_bufferNum < 2)
                {
                    // 1面だと HImage 化中に次フレームで上書きされる可能性が高い
                    Log("警告: リングバッファ数が 1 です。INI の GrabberOut_BufferNum を 3 以上にすることを推奨します。");
                }

                // --- 4-2. 作業用バッファを事前確保（取込中に確保処理が走らないように） ---
                //     サイズ = 横幅 × 行数 × 1画素のバイト数（パディングなし）
                long bytesPerPixel = (_bufferBitCount + 7) / 8;
                lock (_convertLock)
                {
                    if (_bufferBitCount <= 16)
                    {
                        // モノクロ：HImage プールを事前確保（取込中は HALCON のメモリ確保なし）
                        _pool = new ImagePool(PoolSlotCount, (int)_bufferWidth, (int)_bufferHeight, (int)_bufferBitCount);
                    }
                    else
                    {
                        // カラー：作業用バッファ経由で毎フレーム HImage を作成
                        _staging.EnsureSize((long)_bufferWidth * _bufferHeight * bytesPerPixel);
                    }
                }

                // --- 5. イベントハンドラ登録 ---
                AttachEvents();

                Log($"オープン完了 [{AcapUtil.GetDeviceInfoText(_acap)}] {BufferInfoText}");
            }
            catch
            {
                // 途中で失敗した場合はクローズして制御権を開放
                Close();
                throw;
            }
        }

        /// <summary>
        /// 取込を停止し、チャンネルをクローズします。
        /// </summary>
        public void Close()
        {
            if (!IsOpened) return;

            // 取込停止（エラーでもクローズ処理は続行）
            try { Stop(); }
            catch (Exception ex) { Log("Stop エラー: " + ex.Message); }

            // シリアルポートが開いていればクローズ（受信スレッドの削除）
            try { SerialClose(); }
            catch (Exception ex) { Log("SerialClose エラー: " + ex.Message); }

            // イベントハンドラ解除（Close 前に必ず解除する：マニュアル指示）
            DetachEvents();

            // クローズ（内部バッファも解放される）
            int rtn = _acap.Close();
            if (rtn < 0) Log($"Close エラー (rtn={rtn}) {GetLastErrorText()}");

            // 作業用バッファを解放（変換中の場合は終わるまで待つ）
            lock (_convertLock)
            {
                _staging.Dispose();
                _pool?.Dispose();
                _pool = null;
                // ロック内で状態を変えることで、以降の FrameEnd は変換処理に入らない
                IsOpened = false;
            }
            Log("クローズしました。");
        }

        /// <summary>IDisposable 実装（Close を呼ぶ）</summary>
        public void Dispose()
        {
            Close();
        }

        // ===================================================================
        // 取込制御
        // ===================================================================

        /// <summary>連続取込を開始します（画像は ImageGrabbed イベントで通知）。</summary>
        public void StartContinuous()
        {
            lock (_acqLock)
            {
                EnsureOpened();
                if (IsGrabbing) return;

                ResetCounters();

                // 第1引数: 最初に転送するバッファ番号、第2引数: 0 = 連続入力
                Check(_acap.StartAcquisition(0, 0), "StartAcquisition");
                IsGrabbing = true;
            }
            Log("連続取込を開始しました。");
        }

        /// <summary>取込を停止します。</summary>
        public void Stop()
        {
            lock (_acqLock)
            {
                if (!IsGrabbing) return;

                Check(_acap.StopAcquisition(), "StopAcquisition");
                IsGrabbing = false;

                // 待機中のスナップがあればキャンセル扱いにする
                TaskCompletionSource<HImage> tcs = Interlocked.Exchange(ref _snapTcs, null);
                tcs?.TrySetCanceled();
            }
            Log("取込を停止しました。");
        }

        /// <summary>
        /// 1 フレームだけ取り込み、HImage として返します。
        /// </summary>
        /// <param name="timeoutMs">タイムアウト[ms]（外部トリガ待ちの場合は長めに設定）</param>
        /// <returns>取込画像（呼び出し側で Dispose すること）</returns>
        public async Task<HImage> SnapAsync(int timeoutMs)
        {
            // 完了通知用（継続処理はイベントスレッドではなくスレッドプールで実行させる）
            TaskCompletionSource<HImage> tcs =
                new TaskCompletionSource<HImage>(TaskCreationOptions.RunContinuationsAsynchronously);

            lock (_acqLock)
            {
                EnsureOpened();
                if (IsGrabbing) throw new InvalidOperationException($"{Name}: 取込中はスナップできません。");

                ResetCounters();
                Volatile.Write(ref _snapTcs, tcs);

                // 第2引数 1 = 1 フレームだけ入力
                int rtn = _acap.StartAcquisition(0, 1);
                if (rtn < 0)
                {
                    Volatile.Write(ref _snapTcs, null);
                    Check(rtn, "StartAcquisition");
                }
                IsGrabbing = true;
            }

            try
            {
                // FrameEnd かタイムアウトのどちらか早い方を待つ
                Task finished = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs)).ConfigureAwait(false);
                if (finished != tcs.Task)
                {
                    // タイムアウト後に画像が届いた場合、受け取り手がいないのでここで解放する
                    _ = tcs.Task.ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion) t.Result?.Dispose();
                    }, TaskScheduler.Default);
                    throw new TimeoutException($"{Name}: スナップがタイムアウトしました ({timeoutMs}ms)。");
                }
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                // 待機状態を解除
                Interlocked.Exchange(ref _snapTcs, null);
                // フレーム数指定の場合も StopAcquisition は必ず実行する（マニュアル指示）
                Stop();
            }
        }

		// ===================================================================
		// イベントハンドラ（AcapLibPlus のイベントスレッドで実行される）
		// ===================================================================

		/// <summary>
		/// 1 フレームの DMA 転送完了時に呼ばれます。
		/// ここで素早く HImage にコピーしないと、リングバッファが次のフレームで上書きされます。
		/// </summary>
		private void OnFrameEnd(object sender, FrameEndEventArgs e)
        {
            HImage image = null;
            try
            {
                uint dmaCount = e.Buffer_DmaCount;

                // --- 欠落フレームの検出（DmaCount が 2 以上飛んだら欠落） ---
                if (_hasLastDmaCount)
                {
                    uint diff = unchecked(dmaCount - _lastDmaCount);
                    if (diff > 1) Interlocked.Add(ref _lostFrameCount, diff - 1);
                }
                _lastDmaCount = dmaCount;
                _hasLastDmaCount = true;

                string buf;
				// 【切り分け用】最初のフレームだけ、届いた画像情報とプールのサイズを記録
				if (Interlocked.Read(ref _frameCount) == 0)
				{
					Log($"FrameEnd: W={e.Width} H={e.Height} Bit={e.BitCount} Addr={e.Address} " +
						$"/ Buffer: W={_bufferWidth} H={_bufferHeight} Bit={_bufferBitCount} Stride={_bufferStride} " +
						$"/ Pool: {(_pool == null ? "なし" : $"W={_pool.Width} H={_pool.Height} Bpp={_pool.BytesPerPixel}")}");

					buf = $"FrameEnd: W={e.Width} H={e.Height} Bit={e.BitCount} Addr={e.Address} " +
                        $"/ Buffer: W={_bufferWidth} H={_bufferHeight} Bit={_bufferBitCount} Stride={_bufferStride} " +
                        $"/ Pool: {(_pool == null ? "なし" : $"W={_pool.Width} H={_pool.Height} Bpp={_pool.BytesPerPixel}")}";
				}
				
                // --- DMA バッファ → HImage ---
				HImage pooled = null;   // プールの画像（Dispose しない・所有権なし）
                lock (_convertLock)
                {
                    // Close 済み（プール・作業用バッファ解放後）なら何もしない
                    if (!IsOpened) return;

                    if (_pool != null)
                    {
                        // 【モノクロ】事前確保した HImage に画素データをコピー（HALCON のメモリ確保なし）
                        pooled = _pool.Fill(e.Address, (int)_bufferStride);
                    }
                    else
                    {
                        // 【カラー】作業用バッファ経由で HImage を新規作成（所有権あり）
                        image = HImageConverter.FromBufferViaStaging(e.Address, (int)e.Width, (int)e.Height,
                                                                     (int)e.BitCount, (int)_bufferStride, _staging);
                    }
                }

                // --- コピー中にリングバッファが一周していないか確認 ---
                uint nowCount;
                if (_acap.GetParameter(EnumParameterID.Buffer_DmaCount, out nowCount) >= 0)
                {
                    uint advanced = unchecked(nowCount - dmaCount);
                    if (_bufferNum > 0 && advanced >= _bufferNum)
                    {
                        // コピー完了前に同じバッファへ次の DMA が入った可能性がある
                        Interlocked.Increment(ref _overwriteSuspectCount);
                    }
                }

                Interlocked.Increment(ref _frameCount);

                // --- スナップ待機中なら、その呼び出し元に画像を渡す ---
                TaskCompletionSource<HImage> tcs = Interlocked.Exchange(ref _snapTcs, null);
                if (tcs != null)
                {
                    // スナップ画像は呼び出し側が保持・Dispose するので、プール画像なら複製して渡す
                    // （スナップは 1 回だけなので複製によるメモリ確保は問題にならない）
                    if (image == null && pooled != null) image = pooled.CopyImage();
                    if (tcs.TrySetResult(image)) image = null; // 所有権移譲
                    return;
                }

                // --- 連続取込：購読者へ通知 ---
                EventHandler<ImageGrabbedEventArgs> handler = ImageGrabbed;
                if (handler != null)
                {
                    if (pooled != null)
                    {
                        // プール画像：所有権は渡さない（受け取った側は Dispose しない）
                        handler(this, new ImageGrabbedEventArgs(pooled, dmaCount, e.Index, true));
                    }
                    else
                    {
                        HImage passImage = image;
                        image = null; // 所有権移譲（以降このメソッドでは Dispose しない）
                        handler(this, new ImageGrabbedEventArgs(passImage, dmaCount, e.Index, false));
                    }
                }
            }
            catch (Exception ex)
            {
                Log("FrameEnd 処理エラー: " + ex.Message);
            }
            finally
            {
                // 誰にも渡らなかった画像は解放
                image?.Dispose();
            }
        }

        /// <summary>Opt-C:Link リンク接続通知</summary>
        private void OnLinkDetect(object sender)
        {
            Log("Opt-C:Link リンク接続を検出しました。");
        }

        /// <summary>Opt-C:Link リンク切断通知</summary>
        private void OnLinkLost(object sender)
        {
            Log("警告: Opt-C:Link リンクが切断されました。");
        }

        // ===================================================================
        // 内部処理
        // ===================================================================

        /// <summary>イベントハンドラを登録します。</summary>
        private void AttachEvents()
        {
            if (_eventsAttached) return;
            _acap.FrameEndEvent += _frameEndHandler;
            _acap.LinkDetectEvent += _linkDetectHandler;
            _acap.LinkLostEvent += _linkLostHandler;
            _eventsAttached = true;
        }

        /// <summary>イベントハンドラを解除します。</summary>
        private void DetachEvents()
        {
            if (!_eventsAttached) return;
            _acap.FrameEndEvent -= _frameEndHandler;
            _acap.LinkDetectEvent -= _linkDetectHandler;
            _acap.LinkLostEvent -= _linkLostHandler;
            _eventsAttached = false;
        }

        /// <summary>統計カウンタをリセットします。</summary>
        private void ResetCounters()
        {
            Interlocked.Exchange(ref _frameCount, 0);
            Interlocked.Exchange(ref _lostFrameCount, 0);
            Interlocked.Exchange(ref _overwriteSuspectCount, 0);
            _hasLastDmaCount = false;
        }

        /// <summary>Open 済みでなければ例外を投げます。</summary>
        private void EnsureOpened()
        {
            if (!IsOpened)
            {
                logger.Error($"{Name}: オープンされていません。");
				//throw new InvalidOperationException($"{Name}: オープンされていません。");
			}
		}

        /// <summary>
        /// AcapLibPlus 関数の返り値をチェックします。
        /// 負数ならエラー詳細を取得して例外にします（0,1,2 は成功扱い）。
        /// </summary>
        private void Check(int rtn, string function)
        {
            if (rtn < 0)
            {
                throw new AcapException($"{Name}:{function}", rtn, GetLastErrorText());
            }
        }

        /// <summary>GetLastError でエラー詳細文字列を取得します。</summary>
        private string GetLastErrorText()
        {
            try
            {
                AVALLOGMSG msg;
                _acap.GetLastError(out msg);
                return $"[StatusID={msg.StatusID} Name={msg.Name} Msg={msg.LogMsg}]";
            }
            catch (Exception ex)
            {
                return "[GetLastError 失敗: " + ex.Message + "]";
            }
        }

        /// <summary>ログを通知します。</summary>
        private void Log(string message)
        {
            LogMessage?.Invoke(this, $"{Name}: {message}");
        }
    }

    /// <summary>AcapLibPlus 関連の補助関数</summary>
    public static class AcapUtil
    {
        /// <summary>
        /// デバイスチャンネル情報を表示用文字列にします。
        /// （Open 前でも取得可能）
        /// </summary>
        public static string GetDeviceInfoText(IAcapLibDotNet acap)
        {
            DEV_CH_INFO info;
            acap.GetDeviceChannelInfo(out info);
            return $"{ToText(info.FG_Name)} BoardID:{info.BoardID} Ch:{info.Channel}";
        }

        /// <summary>
        /// FG_Name が string / char[] / byte[] のいずれで定義されていても文字列化できるようにする。
        /// </summary>
        private static string ToText(object value)
        {
            if (value == null) return string.Empty;
            if (value is char[] chars) return new string(chars).TrimEnd('\0');
            if (value is byte[] bytes) return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
            return value.ToString().TrimEnd('\0');
        }
    }
}
