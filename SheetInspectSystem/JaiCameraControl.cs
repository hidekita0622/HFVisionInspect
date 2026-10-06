using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HFVisionInspect
{
    /// <summary>
    /// JAI カメラ（SW-8000M-PMCL など）を Short ASCII コマンドで制御するクラス。
    /// 通信は AcapChannel のシリアル機能（画像入力ボード経由）を使用します。
    ///
    /// プロトコル（JAI Command List SW-4000M/8000M-PMCL Ver1.5 「3. Protocol」）:
    ///   通信設定: 9600bps / 8bit / パリティなし / ストップ 1bit / フロー制御なし（起動時は常に 9600bps）
    ///   設定 : "NN=[Param.]<CR><LF>"   → 成功時 "COMPLETE<CR><LF>"
    ///   読出 : "NN?<CR><LF>"           → "NN=値<CR><LF>"
    ///   エラー:                         → "01 Unknown Command!!<CR><LF>"、"02 Bad Parameters!!<CR><LF>"
    /// </summary>
    public sealed class JaiCameraControl
    {
        /// <summary>送信コマンドの終端</summary>
        private const string CommandTerminator = "\r\n";

        /// <summary>応答の終端</summary>
        private const string ResponseTerminator = "\r\n";

        /// <summary>設定成功時の応答</summary>
        private const string CompleteResponse = "COMPLETE";

        /// <summary>エラー応答の形式（例: "01 Unknown command!!"）</summary>
        private static readonly Regex ErrorPattern = new Regex(@"^(\d{2})\s+(.*)$");

        private readonly AcapChannel _channel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">カメラが接続されているチャンネル（Open 済み）</param>
        public JaiCameraControl(AcapChannel channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        /// <summary>応答待ち時間[ms]（設定値の保存など時間のかかるコマンドは長めにする）</summary>
        public uint TimeoutMs { get; set; } = 1000;

        // ===================================================================
        // 接続
        // ===================================================================

        /// <summary>
        /// シリアルをオープンします。
        /// SW-8000M-PMCL は起動時に必ず 9600bps になるため、通常は 9600 のまま使う。
        /// （高速化する場合は CBDRT コマンドによる切替手順が必要：コマンドリスト 3.3）
        /// </summary>
        public void Connect(uint baudRate = 9600)
        {
            _channel.SerialOpen(baudRate);
        }

        /// <summary>シリアルをクローズします。</summary>
        public void Disconnect()
        {
            _channel.SerialClose();
        }

        // ===================================================================
        // 設定（書き込み）
        // ===================================================================

        /// <summary>
        /// 値を設定します（"コマンド=値" を送信し、COMPLETE を確認）。
        /// </summary>
        /// <param name="command">コマンド名（コマンドリスト参照）</param>
        /// <param name="value">設定値</param>
        /// <exception cref="JaiCommandException">カメラがエラーを返した</exception>
        public void SetValue(string command, string value)
        {
            ValidateCommand(command);
            string response = Query($"{command}={value}");
            ParseSetResponse(command, response);
        }

        /// <summary>整数値を設定します。</summary>
        public void SetValue(string command, int value)
        {
            SetValue(command, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>小数値を設定します（小数点は必ず "." で送る）。</summary>
        public void SetValue(string command, double value)
        {
            SetValue(command, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 値なしのコマンドを実行します（"コマンド" のみ送信し、COMPLETE を確認）。
        /// 例: ユーザー設定の保存・読込など、コマンドリストで値なしとされているもの
        /// </summary>
        public void Execute(string command)
        {
            ValidateCommand(command);
            string response = Query(command);
            ParseSetResponse(command, response);
        }

        // ===================================================================
        // 読み出し
        // ===================================================================

        /// <summary>
        /// 現在値を読み出します（"コマンド?" を送信し、"コマンド=値" の値部分を返す）。
        /// </summary>
        public string GetValue(string command)
        {
            ValidateCommand(command);
            string response = Query($"{command}?");
            return ParseGetResponse(command, response);
        }

        /// <summary>現在値を整数で読み出します。</summary>
        public int GetInt(string command)
        {
            string text = GetValue(command);
            int value;
            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                throw new JaiCommandException(command, null, $"整数に変換できない応答です: \"{text}\"");
            return value;
        }

        /// <summary>現在値を小数で読み出します。</summary>
        public double GetDouble(string command)
        {
            string text = GetValue(command);
            double value;
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                throw new JaiCommandException(command, null, $"数値に変換できない応答です: \"{text}\"");
            return value;
        }

        // ===================================================================
        // SW-8000M-PMCL 用コマンド（JAI Command List SW-4000M/8000M-PMCL Ver1.5 準拠）
        // ===================================================================

        // ----- コマンド名（Short ASCII） -----
        private const string CmdDeviceId = "ID";           // シリアル番号（R/O）
        private const string CmdModelName = "MD";          // 機種名（R/O）
        private const string CmdGain = "GA";               // ゲイン（R/W、100～6400）
        private const string CmdExposureMode = "EM";       // 露光モード（0:Off 1:Timed 2:TriggerWidth）
        private const string CmdExposureTime = "PE";       // 露光時間[us]（R/W）
        private const string CmdExposureMin = "PEMIN";     // 露光時間の現在の下限[us]（R/O）
        private const string CmdExposureMax = "PEMAX";     // 露光時間の現在の上限[us]（R/O）
        private const string CmdLineRate = "LR";           // ラインレート（ライン周期、10ns 単位）
        private const string CmdLineRateMin = "ARMIN";     // ライン周期の現在の下限（R/O）
        private const string CmdTriggerOrigin = "TG";      // トリガ 0:Internal 1:External

        // ----- 値の範囲（コマンドリスト記載値） -----
        /// <summary>GA の最小値（= 1.00 倍）</summary>
        public const int GainRawMin = 100;
        /// <summary>GA の最大値（SW-8000M-PMCL: 6400 = 64.00 倍）</summary>
        public const int GainRawMax = 6400;
        /// <summary>LR の最大値（ライン周期 15.15152ms = 約 66Hz）</summary>
        public const int LinePeriodRawMax = 1515152;
        /// <summary>LR の 1 ステップ [ns]</summary>
        private const double LinePeriodStepNs = 10.0;

        // ----- シリアル番号 -----

        /// <summary>カメラのシリアル番号を読み出します（ID?）。</summary>
        public string GetSerialNumber()
        {
            return GetValue(CmdDeviceId);
        }

        /// <summary>機種名を読み出します（MD?）。接続先の確認用。</summary>
        public string GetModelName()
        {
            return GetValue(CmdModelName);
        }

        // ----- ゲイン -----

        /// <summary>
        /// ゲインを倍率で設定します（1.0～64.0 倍）。
        /// GA は 100 = 1.00 倍として扱います（GA の範囲 100～6400 と、仕様のゲイン 1～64 倍の対応から）。
        /// </summary>
        public void SetGain(double magnification)
        {
            SetGainRaw((int)Math.Round(magnification * 100.0));
        }

        /// <summary>ゲインを倍率で読み出します。</summary>
        public double GetGain()
        {
            return GetGainRaw() / 100.0;
        }

        /// <summary>ゲインを GA の設定値で直接設定します（100～6400）。</summary>
        public void SetGainRaw(int raw)
        {
            if (raw < GainRawMin || raw > GainRawMax)
            {
				Trace.WriteLine($"ゲインは {GainRawMin}～{GainRawMax}（1.00～64.00 倍）の範囲で指定してください。");
				return;

				//throw new ArgumentOutOfRangeException(nameof(raw), raw, $"ゲインは {GainRawMin}～{GainRawMax}（1.00～64.00 倍）の範囲で指定してください。");
			}
			SetValue(CmdGain, raw);
        }

        /// <summary>ゲインを GA の設定値で読み出します。</summary>
        public int GetGainRaw()
        {
            return GetInt(CmdGain);
        }

        // ----- 露光時間 -----

        /// <summary>
        /// 露光時間[us]を設定します。
        /// ・露光モードが Timed（EM=1）のときだけ有効です。
        /// ・設定できる範囲（PEMIN～PEMAX）はラインレートによって変わるため、毎回カメラに問い合わせて確認します。
        /// </summary>
        public void SetExposureTime(int microseconds)
        {
            // 露光モードの確認（Timed 以外では PE の設定が効かない）
            int mode = GetInt(CmdExposureMode);
            if (mode != 1)
                throw new InvalidOperationException($"露光モードが Timed ではありません（EM={mode}）。SetExposureModeTimed() を先に実行してください。");

            // 現在のラインレートでの上下限を確認
            int min, max;
            GetExposureRange(out min, out max);
            if (microseconds < min || microseconds > max)
            {
				Trace.WriteLine($"露光時間 {microseconds}us は現在のラインレートで設定できません（範囲 {min}～{max}us）。");
				return;
				//throw new ArgumentOutOfRangeException(nameof(microseconds), microseconds,
				//	$"露光時間は {min}～{max}us の範囲で指定してください（上限はラインレートで変わります）。");

			}

			SetValue(CmdExposureTime, microseconds);
        }

        /// <summary>露光時間[us]を読み出します。</summary>
        public int GetExposureTime()
        {
            return GetInt(CmdExposureTime);
        }

        /// <summary>現在設定できる露光時間の範囲[us]を読み出します（PEMIN? / PEMAX?）。</summary>
        public void GetExposureRange(out int min, out int max)
        {
            min = GetInt(CmdExposureMin);
            max = GetInt(CmdExposureMax);
        }

        /// <summary>露光モードを Timed（露光時間指定）にします（EM=1）。</summary>
        public void SetExposureModeTimed()
        {
            SetValue(CmdExposureMode, 1);
        }

        // ----- ラインレート -----

        /// <summary>
        /// ラインレート[Hz]を設定します。
        /// ・内部トリガ（TG=0）のときだけ有効です。
        /// ・LR はライン周期を 10ns 単位で指定するため、LR = 1e8 / Hz に換算します。
        /// ・最高ラインレート（LR の下限）はタップ構成・CL クロック・ビニングで変わるため、ARMIN? で確認します。
        /// </summary>
        public void SetLineRate(double hz)
        {
            if (hz <= 0) throw new ArgumentOutOfRangeException(nameof(hz), hz, "ラインレートは正の値で指定してください。");
            SetLinePeriodRaw((int)Math.Round(1e9 / LinePeriodStepNs / hz));
        }

        /// <summary>ラインレート[Hz]を読み出します。</summary>
        public double GetLineRate()
        {
            return 1e9 / LinePeriodStepNs / GetLinePeriodRaw();
        }

        /// <summary>ライン周期を LR の設定値（10ns 単位）で直接設定します。</summary>
        public void SetLinePeriodRaw(int raw)
        {
            // 内部トリガの確認（外部トリガ時は LR が効かない）
            int tg = GetInt(CmdTriggerOrigin);
            if (tg != 0)
                throw new InvalidOperationException($"外部トリガ（TG={tg}）のため、ラインレートは設定できません。");

            // 現在の構成での下限（= 最高ラインレート）を確認
            int min = GetInt(CmdLineRateMin);
            if (raw < min || raw > LinePeriodRawMax)
            {
                throw new ArgumentOutOfRangeException(nameof(raw), raw,
                    $"ライン周期は {min}～{LinePeriodRawMax}（{RawToHz(LinePeriodRawMax):F1}～{RawToHz(min):F1}Hz）の範囲で指定してください。");
            }

            SetValue(CmdLineRate, raw);
        }

        /// <summary>ライン周期を LR の設定値（10ns 単位）で読み出します。</summary>
        public int GetLinePeriodRaw()
        {
            return GetInt(CmdLineRate);
        }

        /// <summary>設定できる最高ラインレート[Hz]を読み出します（ARMIN? から換算）。</summary>
        public double GetMaxLineRate()
        {
            return RawToHz(GetInt(CmdLineRateMin));
        }

        /// <summary>LR の値[10ns]を Hz に換算します。</summary>
        private static double RawToHz(int raw)
        {
            return 1e9 / LinePeriodStepNs / raw;
        }

        // ===================================================================
        // 内部処理
        // ===================================================================

        /// <summary>送信して応答を 1 行受信します（前後の空白・改行は除去）。</summary>
        private string Query(string text)
        {
            string response = _channel.SerialQuery(text, CommandTerminator, ResponseTerminator, TimeoutMs);
            return response.Trim();
        }

        /// <summary>設定コマンドの応答を判定します。</summary>
        private static void ParseSetResponse(string command, string response)
        {
            if (string.Equals(response, CompleteResponse, StringComparison.OrdinalIgnoreCase)) return;

            ThrowIfError(command, response);

            // COMPLETE でもエラー形式でもない応答
            throw new JaiCommandException(command, null, $"想定外の応答です: \"{response}\"");
        }

        /// <summary>読み出しコマンドの応答から値を取り出します。</summary>
        private static string ParseGetResponse(string command, string response)
        {
            ThrowIfError(command, response);

            // "コマンド=値" の形式なら値部分を返す
            int eq = response.IndexOf('=');
            if (eq >= 0)
            {
                string name = response.Substring(0, eq).Trim();
                if (!string.Equals(name, command, StringComparison.OrdinalIgnoreCase))
                {
                    // 別コマンドの応答が返ってきた（前の応答の残りなど）
                    throw new JaiCommandException(command, null, $"別コマンドの応答です: \"{response}\"");
                }
                return response.Substring(eq + 1).Trim();
            }

            // "=" が無い場合は応答全体を値とみなす（形式が異なるカメラ向け）
            return response;
        }

        /// <summary>エラー形式の応答なら例外を投げます。</summary>
        private static void ThrowIfError(string command, string response)
        {
            Match m = ErrorPattern.Match(response);
            if (m.Success)
            {
                throw new JaiCommandException(command, m.Groups[1].Value, m.Groups[2].Value);
            }
        }

        /// <summary>コマンド名の簡易チェック（空や改行混入を防ぐ）。</summary>
        private static void ValidateCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("コマンド名が空です。", nameof(command));
            if (command.IndexOfAny(new[] { '\r', '\n', '=', '?' }) >= 0)
                throw new ArgumentException($"コマンド名に使用できない文字が含まれています: \"{command}\"", nameof(command));
        }
    }

    /// <summary>JAI カメラがエラーを返した場合の例外</summary>
    public sealed class JaiCommandException : Exception
    {
        public JaiCommandException(string command, string errorCode, string detail)
            : base($"JAI コマンド \"{command}\" 失敗" + (errorCode != null ? $" (エラー{errorCode})" : "") + $": {detail}")
        {
            Command = command;
            ErrorCode = errorCode;
        }

        /// <summary>送信したコマンド名</summary>
        public string Command { get; }

        /// <summary>カメラが返したエラー番号（"01" など。不明な場合は null）</summary>
        public string ErrorCode { get; }
    }
}
