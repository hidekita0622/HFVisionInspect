using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Avaldata.AcapLibDotNet.Parameters.Id;

namespace HFVisionInspect
{
    /// <summary>
    /// AcapChannel のシリアル通信機能（画像入力ボード経由でカメラと RS-232C 通信する）。
    ///
    /// マニュアル 5-9「シリアル通信」の手順に従う:
    ///   シリアルオープン → ボーレート設定 → 送受信バッファクリア → データ送信 → データ受信 → シリアルクローズ
    ///
    /// 注意:
    ///   ・ライブラリはネゴシエーションをしないため、「コマンド送信 → 応答待ち」の同期型通信のみ対応
    ///   ・受信割り込みが無いため、受信は「バイト数」「終端文字」「タイムアウト」で完了を判断する
    ///   ・チャンネルの Open 後に使用すること（チャンネル Close 時に自動でシリアルもクローズする）
    /// </summary>
    public sealed partial class AcapChannel
    {
        // ===== シリアル関連のエラーコード（マニュアル記載値） =====
        /// <summary>送受信タイムアウト</summary>
        private const int AVAL_STATUS_TIMEOUT = -1011;
        /// <summary>未対応の ParameterID（OptCLink_BaudRate 非対応機種など）</summary>
        private const int AVAL_STATUS_NOT_IMPLEMENTED = -1003;

        /// <summary>Status_AcapLibPlus の Bit16：シリアル受信バッファのオーバーフロー</summary>
        private const uint SerialReceiveOverFlowBit = 1u << 16;

        /// <summary>終端文字待ち受信で 1 回の SerialReceive に与える待ち時間[ms]</summary>
        private const uint SerialPollTimeoutMs = 10;

        // シリアル送受信の排他用（コマンドと応答が他スレッドの通信と混ざらないようにする）
        private readonly object _serialLock = new object();

        /// <summary>シリアルポートがオープンされているか</summary>
        public bool IsSerialOpened { get; private set; }

        /// <summary>現在のボーレート[bps]</summary>
        public uint SerialBaudRate { get; private set; }

        /// <summary>文字列コマンドの文字コード（カメラのコマンドは通常 ASCII）</summary>
        public Encoding SerialEncoding { get; set; } = Encoding.ASCII;

        // ===================================================================
        // オープン・クローズ
        // ===================================================================

        /// <summary>
        /// シリアルポートをオープンします。
        /// </summary>
        /// <param name="baudRate">カメラのボーレート[bps]（9600 以上、カメラ側の設定と合わせる）</param>
        /// <param name="receiveBufferSize">ライブラリ内の受信中間バッファサイズ[byte]</param>
        /// <param name="setOptCLinkBaudRate">
        /// true: Opt-C:Link デバイス（AOC-162 等）とカメラ間のボーレートも設定する。
        /// AOC-162 は電源を入れ直すと初期値に戻るため、毎回設定する（マニュアル 7-103）。
        /// </param>
        public void SerialOpen(uint baudRate = 9600, uint receiveBufferSize = 16384, bool setOptCLinkBaudRate = true)
        {
            lock (_serialLock)
            {
                EnsureOpened();
                if (IsSerialOpened) return;

                // --- 1. シリアルオープン（受信スレッドが生成される） ---
                Check(_acap.SerialOpen(baudRate, receiveBufferSize), "SerialOpen");
                IsSerialOpened = true;
                SerialBaudRate = baudRate;

                try
                {
                    // --- 2. Opt-C:Link デバイス（AOC-162 等）のボーレート設定 ---
                    if (setOptCLinkBaudRate)
                    {
                        int rtn = _acap.SetParameter(EnumParameterID.OptCLink_BaudRate, baudRate);
                        if (rtn == AVAL_STATUS_NOT_IMPLEMENTED)
                        {
                            // 非対応の構成（旧型 Opt-C:Link 製品など）：ボード側の設定だけで通信する
                            Log("OptCLink_BaudRate は非対応のため設定をスキップしました。");
                        }
                        else
                        {
                            Check(rtn, "SetParameter(OptCLink_BaudRate)");
                        }
                    }

                    // --- 3. 送受信バッファクリア ---
                    Check(_acap.SerialPurge(), "SerialPurge");
                }
                catch
                {
                    // 途中で失敗したらクローズして元に戻す
                    SerialCloseCore();
                    throw;
                }
            }
            Log($"シリアルオープン ({baudRate}bps)");
        }

        /// <summary>シリアルポートをクローズします（受信スレッドが削除される）。</summary>
        public void SerialClose()
        {
            lock (_serialLock)
            {
                if (!IsSerialOpened) return;
                SerialCloseCore();
            }
            Log("シリアルクローズ");
        }

        /// <summary>クローズ処理本体（_serialLock 内で呼ぶ）</summary>
        private void SerialCloseCore()
        {
            int rtn = _acap.SerialClose();
            IsSerialOpened = false;
            if (rtn < 0) Log($"SerialClose エラー (rtn={rtn}) {GetLastErrorText()}");
        }

        /// <summary>受信バッファに溜まっている不要データを破棄します。</summary>
        public void SerialPurge()
        {
            lock (_serialLock)
            {
                EnsureSerialOpened();
                Check(_acap.SerialPurge(), "SerialPurge");
            }
        }

        // ===================================================================
        // 送信
        // ===================================================================

        /// <summary>
        /// バイト列を送信します。
        /// </summary>
        /// <param name="data">送信データ</param>
        /// <param name="timeoutMs">送信完了までの待ち時間[ms]</param>
        public void SerialSend(byte[] data, uint timeoutMs = 1000)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("送信データが空です。", nameof(data));

            lock (_serialLock)
            {
                EnsureSerialOpened();
                SendCore(data, timeoutMs);
            }
        }

        /// <summary>
        /// 文字列を送信します（SerialEncoding で変換。改行コードは呼び出し側で付けること）。
        /// </summary>
        public void SerialSend(string text, uint timeoutMs = 1000)
        {
            SerialSend(SerialEncoding.GetBytes(text ?? string.Empty), timeoutMs);
        }

        /// <summary>送信処理本体（_serialLock 内で呼ぶ）</summary>
        private void SendCore(byte[] data, uint timeoutMs)
        {
            uint size = (uint)data.Length;   // 実行後は送信できたバイト数が入る
            int rtn = _acap.SerialSend(data, ref size, timeoutMs);
            if (rtn == AVAL_STATUS_TIMEOUT)
            {
                throw new TimeoutException($"{Name}: シリアル送信タイムアウト ({size}/{data.Length} byte 送信済)");
            }
            Check(rtn, "SerialSend");
        }

        // ===================================================================
        // 受信
        // ===================================================================

        /// <summary>
        /// 指定バイト数を受信します（応答のバイト数が決まっているコマンド向け）。
        /// </summary>
        /// <param name="count">受信するバイト数</param>
        /// <param name="timeoutMs">全体の待ち時間[ms]</param>
        /// <returns>受信データ（count バイト）</returns>
        /// <exception cref="TimeoutException">時間内に count バイト揃わなかった</exception>
        public byte[] SerialReceive(int count, uint timeoutMs = 1000)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            lock (_serialLock)
            {
                EnsureSerialOpened();
                return ReceiveExactCore(count, timeoutMs);
            }
        }

        /// <summary>
        /// 終端文字列を受信するまで受信します（応答の終わりが決まっているコマンド向け）。
        /// </summary>
        /// <param name="terminator">終端文字列（例: "\r\n"、カメラのプロンプト "&gt;" など）</param>
        /// <param name="timeoutMs">全体の待ち時間[ms]</param>
        /// <param name="maxLength">最大受信バイト数（これを超えたらエラー）</param>
        /// <returns>受信データ（終端文字列を含む）</returns>
        public byte[] SerialReceiveUntil(byte[] terminator, uint timeoutMs = 1000, int maxLength = 4096)
        {
            if (terminator == null || terminator.Length == 0) throw new ArgumentException("終端文字が空です。", nameof(terminator));

            lock (_serialLock)
            {
                EnsureSerialOpened();
                return ReceiveUntilCore(terminator, timeoutMs, maxLength);
            }
        }

        /// <summary>
        /// 一定時間に届いたデータをすべて受信します（応答の長さも終端も不明な場合）。
        /// 必ず timeoutMs だけ待つので、応答時間の最悪値より長めに指定すること。
        /// </summary>
        /// <param name="timeoutMs">待ち時間[ms]</param>
        /// <param name="maxLength">最大受信バイト数</param>
        public byte[] SerialReceiveAll(uint timeoutMs = 200, int maxLength = 4096)
        {
            lock (_serialLock)
            {
                EnsureSerialOpened();

                // 多めに受信要求 → 必ずタイムアウトするが、受信できた分は返ってくる（マニュアル 5-9-1）
                byte[] buffer = new byte[maxLength];
                uint size = (uint)maxLength;
                int rtn = _acap.SerialReceive(buffer, ref size, timeoutMs);
                if (rtn < 0 && rtn != AVAL_STATUS_TIMEOUT) Check(rtn, "SerialReceive");

                Array.Resize(ref buffer, (int)size);
                return buffer;
            }
        }

        /// <summary>指定バイト数の受信本体（_serialLock 内で呼ぶ）</summary>
        private byte[] ReceiveExactCore(int count, uint timeoutMs)
        {
            byte[] result = new byte[count];
            int received = 0;
            Stopwatch sw = Stopwatch.StartNew();

            // タイムアウト時は途中まで受信できているので、残りを受信し直す（マニュアル 5-9-1）
            while (received < count)
            {
                long remainMs = timeoutMs - sw.ElapsedMilliseconds;
                if (remainMs <= 0)
                {
                    throw new TimeoutException($"{Name}: シリアル受信タイムアウト ({received}/{count} byte 受信)");
                }

                byte[] chunk = new byte[count - received];
                uint size = (uint)chunk.Length;   // 実行後は受信できたバイト数が入る
                int rtn = _acap.SerialReceive(chunk, ref size, (uint)remainMs);
                if (rtn < 0 && rtn != AVAL_STATUS_TIMEOUT) Check(rtn, "SerialReceive");

                // 受信できた分を連結
                Buffer.BlockCopy(chunk, 0, result, received, (int)size);
                received += (int)size;
            }
            return result;
        }

        /// <summary>終端文字列待ち受信の本体（_serialLock 内で呼ぶ）</summary>
        private byte[] ReceiveUntilCore(byte[] terminator, uint timeoutMs, int maxLength)
        {
            List<byte> received = new List<byte>(256);
            byte[] one = new byte[1];
            Stopwatch sw = Stopwatch.StartNew();

            // 1 バイトずつ受信して、末尾が終端文字列になるまで繰り返す（マニュアル 5-9-1）
            while (true)
            {
                if (sw.ElapsedMilliseconds >= timeoutMs)
                {
                    throw new TimeoutException(
                        $"{Name}: シリアル受信タイムアウト（終端文字を受信できず、{received.Count} byte 受信: {ToDisplayText(received.ToArray())}）");
                }

                uint size = 1;
                int rtn = _acap.SerialReceive(one, ref size, SerialPollTimeoutMs);
                if (rtn < 0 && rtn != AVAL_STATUS_TIMEOUT) Check(rtn, "SerialReceive");
                if (size == 0) continue;   // まだ届いていない → 待ち続ける

                received.Add(one[0]);

                // 末尾が終端文字列と一致したら完了
                if (EndsWith(received, terminator)) return received.ToArray();

                if (received.Count >= maxLength)
                {
                    throw new InvalidOperationException($"{Name}: 受信データが最大長 {maxLength} byte を超えました。");
                }
            }
        }

        // ===================================================================
        // コマンド送受信（送信 → 応答受信をまとめて行う）
        // ===================================================================

        /// <summary>
        /// コマンド文字列を送信し、応答を終端文字列まで受信して文字列で返します。
        /// </summary>
        /// <param name="command">コマンド（例: "gcp"）</param>
        /// <param name="sendTerminator">コマンドの後ろに付ける改行（例: "\r"）。カメラの仕様に合わせる</param>
        /// <param name="responseTerminator">応答の終端文字列（例: "\r\n" やプロンプト "&gt;"）</param>
        /// <param name="timeoutMs">応答待ち時間[ms]</param>
        /// <returns>応答文字列（終端文字列を含む）</returns>
        public string SerialQuery(string command, string sendTerminator = "\r",
                                  string responseTerminator = "\r\n", uint timeoutMs = 1000)
        {
            if (string.IsNullOrEmpty(responseTerminator))
                throw new ArgumentException("応答の終端文字列を指定してください。", nameof(responseTerminator));

            byte[] sendData = SerialEncoding.GetBytes((command ?? string.Empty) + (sendTerminator ?? string.Empty));
            byte[] terminator = SerialEncoding.GetBytes(responseTerminator);

            lock (_serialLock)
            {
                EnsureSerialOpened();

                // 送信前に、前回までの不要な受信データを捨てる（マニュアル推奨）
                Check(_acap.SerialPurge(), "SerialPurge");

                SendCore(sendData, 1000);
                byte[] response = ReceiveUntilCore(terminator, timeoutMs, 4096);
                string text = SerialEncoding.GetString(response);

                Log($"シリアル: 送信[{ToDisplayText(sendData)}] 応答[{ToDisplayText(response)}]");
                return text;
            }
        }

        /// <summary>
        /// コマンドバイト列を送信し、指定バイト数の応答を受信します（バイナリプロトコルのカメラ向け）。
        /// </summary>
        public byte[] SerialQuery(byte[] command, int responseLength, uint timeoutMs = 1000)
        {
            if (command == null || command.Length == 0) throw new ArgumentException("送信データが空です。", nameof(command));

            lock (_serialLock)
            {
                EnsureSerialOpened();
                Check(_acap.SerialPurge(), "SerialPurge");
                SendCore(command, 1000);
                byte[] response = ReceiveExactCore(responseLength, timeoutMs);

                Log($"シリアル: 送信[{ToDisplayText(command)}] 応答[{ToDisplayText(response)}]");
                return response;
            }
        }

        /// <summary>
        /// シリアル受信バッファがオーバーフローしているか確認します（Status_AcapLibPlus の Bit16）。
        /// オーバーフローしていたら SerialPurge で復帰させてください。
        /// </summary>
        public bool IsSerialReceiveOverflow()
        {
            EnsureOpened();
            uint status;
            Check(_acap.GetParameter(EnumParameterID.Status_AcapLibPlus, out status), "GetParameter(Status_AcapLibPlus)");
            return (status & SerialReceiveOverFlowBit) != 0;
        }

        // ===================================================================
        // 内部処理
        // ===================================================================

        /// <summary>シリアルがオープンされていなければ例外を投げます。</summary>
        private void EnsureSerialOpened()
        {
            EnsureOpened();
            if (!IsSerialOpened) throw new InvalidOperationException($"{Name}: シリアルポートがオープンされていません。");
        }

        /// <summary>リストの末尾が指定バイト列と一致するか</summary>
        private static bool EndsWith(List<byte> data, byte[] tail)
        {
            if (data.Count < tail.Length) return false;
            int offset = data.Count - tail.Length;
            for (int i = 0; i < tail.Length; i++)
            {
                if (data[offset + i] != tail[i]) return false;
            }
            return true;
        }

        /// <summary>ログ表示用に変換（表示可能な ASCII はそのまま、制御文字は &lt;0D&gt; のように表示）</summary>
        private static string ToDisplayText(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                if (b >= 0x20 && b < 0x7F) sb.Append((char)b);
                else sb.Append('<').Append(b.ToString("X2")).Append('>');
            }
            return sb.ToString();
        }
    }
}
