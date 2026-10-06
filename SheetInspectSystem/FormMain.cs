using HFVisionInspect.Properties;
using NLog;
using NLog.Config;
//using NLog.Fluent;
using NLog.Targets;
using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
//using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Avaldata;
using Avaldata.AcapLibDotNet;
using HalconDotNet;

namespace HFVisionInspect
{
	public enum ViewID
	{
		ViewFront = 0,
		ViewRear = 1,
	}

	public partial class FormMain : Form
    {
		/// <summary>使用するチャンネル数</summary>
		private const int ChannelCount = 2;

		/// <summary>スナップのタイムアウト[ms]（外部トリガ待ちの場合は延ばす）</summary>
		private const int SnapTimeoutMs = 3000;

		// ----------------------------------------------------------------------
		// AcapLibPlus のデバイスリストとチャンネル配列
		// ガベージコレクションで回収されないよう static フィールドで保持（マニュアル指示）
		// ----------------------------------------------------------------------
		private static CDeviceList _deviceList;
		private static IAcapLibDotNet[] _acapArray;

		//// ----- チャンネルごとの管理データ -----
		//private readonly AcapChannel[] _channels = new AcapChannel[ChannelCount];  // 制御クラス
		//private readonly HImage[] _latestImages = new HImage[ChannelCount];        // 最新取込画像（UI スレッドで管理）

		//// ----- チャンネルごとの UI コントロール（配列でまとめて扱う） -----
		//private HSmartWindowControl[] _hWindows;
		//private Label[] _lblStatus;
		
        public enum InspectState
        {
            Ready = 0,
            Inspect = 1,
            Reset = 2,
            Detection = 3,
            //WaitCutter = 3,
            //PosWait = 4,
            //PosInspect = 5,
            Startup = 10,
        }

        private enum CutterState
        {
            None = 0,
            Delay,
            Active,
        }

        private enum TabSelect
        {
            Defect = 0,
            FrontCamera,
            RearCamera,
        }

        const int MAX_MONITOR = 3;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private string InitialDirectory = Application.StartupPath;

        string SystemFile = "";
        string LightLifeFile = "";
        private LightLifeControl LifeCtrl;
        //private ClassDIO dioCtrl;
        public ClassSystemParam SystemParam = new ClassSystemParam();
        private InspectParameter _inspectParam = new InspectParameter();
        private Point[] MonitorPos;
        private HImage _imgDefect = new HImage();

        private FormInspectHistory _formHistory = new FormInspectHistory();
        private InspectResultData _inspectResult = new InspectResultData();
        private InspectResultData _loadResult = new InspectResultData();
        private bool _loadDataDisp = false;

        //private ClassImageSave _saveCtrl = new ClassImageSave();

        private bool IsManagerMode;
        private CutterState _cutterState;
        private InspectState _inspState = InspectState.Startup;

        private bool _isLightLifeAlarm;
		private bool _isLightGrayAlarm;


		DateTime lastDate = DateTime.Now.Date;

        private TabSelect ActiveTabNo = 0;



		public FormMain()
        {
            //Trace.WriteLine($"HFVisionInspect Start ....");
            InitializeComponent();
            InitializeLogger();


            logger.Info($"HFVisionInspect Start ............................................");
			if (!CheckHalconLicense())
			{
				MessageBox.Show("アプリケーションは正常に起動出来ません！！！", "HALCON Library error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			SystemFile = Application.StartupPath + Define.SystemFilename;      // フルパス
            Helpers.Import(SystemFile, ref SystemParam);
            _inspectResult.CameraResolution = SystemParam.CamInfo.Resolution;
            _inspectResult.RecordWidth = SystemParam.DefectImageWidth;
            _inspectResult.RecoedHeight = SystemParam.DefectImageHeight;

            AvalBoardIniialize();

            userCamViewFront.SystemParam.CopyFrom(SystemParam);
            userCamViewFront.InitialControl(SystemParam.FrontCamera);
            //btInspectSetLeft.Text = $"{SystemParam.LeftCamera.Name} 設定";
			labelBrightLeft.Text = SystemParam.FrontCamera.Name;
			labelExposureSetLeft.Text = SystemParam.FrontCamera.Name;

			userCamViewRear.SystemParam.CopyFrom(SystemParam);
            userCamViewRear.InitialControl(SystemParam.RearCamera);
			btInspectSetRight.Text = $"{SystemParam.RearCamera.Name} 設定";
			labelBrightRight.Text = SystemParam.RearCamera.Name;
			labelExposureSetRight.Text = SystemParam.RearCamera.Name;

			String paramFile = Application.StartupPath + Define.InspectParamFilename;      // フルパス
            Helpers.Import(paramFile, ref _inspectParam);
			userCamViewFront.InspectParam.CopyFrom(_inspectParam);
            userCamViewRear.InspectParam.CopyFrom(_inspectParam);

			LightLifeFile = Application.StartupPath + Define.LightLifedata;
            LifeCtrl = new LightLifeControl();
            Helpers.Import(LightLifeFile, ref LifeCtrl);
            timerLightLife.Start();
            //Helpers.Export(LightLifeFile, LifeCtrl);

            MonitorPos = new Point[MAX_MONITOR];
            int maxMonitor = GetAllDisplayInformation();
            if (MonitorPos[0].X == 0)
            {
                this.Location = new Point(MonitorPos[0].X, MonitorPos[0].Y);
                _formHistory.ShowPos = MonitorPos[1];
            }
            else
            {
                this.Location = new Point(MonitorPos[1].X, MonitorPos[1].Y);
                _formHistory.ShowPos = MonitorPos[0];
            }


            //_saveCtrl.CompRatio = SystemParam.ImageCompRatio;
            //_saveCtrl.DefaultPath = SystemParam.ImageSavePath;

#if DEBUG
            IsManagerMode = true;
#else
			IsManagerMode = true;
#endif
            _cutterState = CutterState.None;
            SetManagerMode();
        }

        private bool CheckHalconLicense()
        {
            bool rtn = false;
            try
            {
                HTuple hv_version = new HTuple();
                HOperatorSet.GetSystem("version", out hv_version);
                rtn = true;
                logger.Info($"Halcon Version {hv_version.ToString()}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return rtn;
        }

		/// <summary>
		/// ロガーの初期化
		/// </summary>
		private static void InitializeLogger()
		{
			// LoggingConfigurationを生成 
			var config = new LoggingConfiguration();
			// FileTargetを生成し LoggingConfigurationに設定 
			var fileTarget = new FileTarget("file");
			var console = new ConsoleTarget("console");
			config.AddTarget(fileTarget);
			config.AddTarget(console);

			// fileTargetのプロパティを設定
			fileTarget.Name = "f";
			fileTarget.FileName = "${basedir}/Log/${shortdate}.log";
			fileTarget.Layout = "${longdate} [${uppercase:${level}}] ${message}";
			//fileTarget.KeepFileOpen = true; // ファイルを開いた状態にしておく

			// LoggingRuleを定義
			var rule1 = new LoggingRule("*", NLog.LogLevel.Info, fileTarget);       // ファイル出力
			config.LoggingRules.Add(rule1);
			var rule2 = new LoggingRule("*", NLog.LogLevel.Trace, fileTarget);
			config.LoggingRules.Add(rule2);
			var rule3 = new LoggingRule("*", NLog.LogLevel.Debug, console);         // コンソール出力
			config.LoggingRules.Add(rule3);

			// 設定を有効化
			LogManager.Configuration = config;
		}

		private void FormMain_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly asmInfo = System.Reflection.Assembly.GetExecutingAssembly();
            string title = string.Format("中空糸外観検査システム  ( Ver {0} )", asmInfo.GetName().Version);
            //this.Text = title;
            logger.Info(title);
            //Helpers.WriteLog(title);

            userCamViewFront.OnResult += ImageProcFinFront;
			userCamViewFront.SystemParam.CopyFrom(SystemParam);
			if (SystemParam.CamInfo.FrontCamera.Enable)
            {
				userCamViewFront.CameraInit();
				userCamViewFront.SetCamera();
			}

			userCamViewRear.OnResult += ImageProcFinRear;
            userCamViewRear.SystemParam.CopyFrom(SystemParam);
            if (SystemParam.CamInfo.RearCamera.Enable)
            {
				userCamViewRear.CameraInit();
				userCamViewRear.SetCamera();
			}

			//userCamViewFront.RegistImageEnable(_inspectParam.DarkBlob.DiffImage);
			//userCamViewRear.RegistImageEnable(_inspectParam.DarkBlob.DiffImage);

			_imgDefect.GenEmptyObj();
            textInspectID.Text = SystemParam.InspectID;

            GetLightLife();
        }

        private void AvalBoardIniialize()
        {
            if (SystemParam.CamInfo.Enable == true)
            {
				// デバイスリストを生成（PC が認識している画像入力ボードを検索）
				_deviceList = new CDeviceList();
				//// 認識しているチャンネル総数（APX-3800 Base2ch × 1 枚なら 2）
				uint num = _deviceList.GetDeviceChannelNum();
				if (num == 0)
				{
                    Debug.WriteLine("画像入力ボードが見つかりません");
					return;
				}
				// チャンネルごとの IAcapLibDotNet 配列を取得
				_acapArray = _deviceList.GetDeviceChannel();
				// チャンネル制御クラスを生成
				userCamViewFront.AcapCtrl = new AcapChannel(_acapArray[0], $"CH1");
				userCamViewRear.AcapCtrl = new AcapChannel(_acapArray[1], $"CH2");
			}
		}

        private void AvalBoardClose()
        {
            if (_acapArray != null)
            {
                for (int i = 0; i < _acapArray.Length; i++)
                {
                    if (_acapArray[i] != null)
                    {
                        _acapArray[i].Close();
                        _acapArray[i] = null;
                    }
                }
            }
  		}

        private void SetCameraInfo()
        {
            if (SystemParam.CamInfo.Enable == false)
                return;

			var jaiFront = new JaiCameraControl(userCamViewFront.AcapCtrl);
			var jaiRear = new JaiCameraControl(userCamViewRear.AcapCtrl);
			jaiFront.Connect();                               // 起動時は常に 9600bps
			jaiRear.Connect();                               // 起動時は常に 9600bps


			int exTime = jaiFront.GetExposureTime();
            if (exTime != SystemParam.CamInfo.FrontCamera.ExposureTime)
            {
                jaiFront.SetExposureTime(SystemParam.CamInfo.FrontCamera.ExposureTime);
				logger.Info($"Front Camera Exposure Time Set {SystemParam.CamInfo.FrontCamera.ExposureTime}");
            }
			exTime = jaiRear.GetExposureTime();
			if (exTime != SystemParam.CamInfo.RearCamera.ExposureTime)
			{
				jaiRear.SetExposureTime(SystemParam.CamInfo.RearCamera.ExposureTime);
				logger.Info($"Rear Camera Exposure Time Set {SystemParam.CamInfo.RearCamera.ExposureTime}");
			}
            double gain = jaiFront.GetGain();
            if (gain != SystemParam.CamInfo.FrontCamera.Gain)
            {
                jaiFront.SetGain(SystemParam.CamInfo.FrontCamera.Gain);
                logger.Info($"Front Camera Gain Set {SystemParam.CamInfo.FrontCamera.Gain}");
            }
			gain = jaiRear.GetGain();
			if (gain != SystemParam.CamInfo.RearCamera.Gain)
			{
				jaiRear.SetGain(SystemParam.CamInfo.RearCamera.Gain);
				logger.Info($"Rear Camera Gain Set {SystemParam.CamInfo.RearCamera.Gain}");
			}

            int lineRate = (int)jaiFront.GetLineRate();
            if (lineRate != SystemParam.CamInfo.FrontCamera.LineRate)
            {
                jaiFront.SetLineRate(SystemParam.CamInfo.FrontCamera.LineRate);
                logger.Info($"Front Camera Line Rate Set {SystemParam.CamInfo.FrontCamera.LineRate}");
            }
			lineRate = (int)jaiRear.GetLineRate();
			if (lineRate != SystemParam.CamInfo.RearCamera.LineRate)
			{
				jaiRear.SetLineRate(SystemParam.CamInfo.RearCamera.LineRate);
				logger.Info($"Rear Camera Line Rate Set {SystemParam.CamInfo.RearCamera.LineRate}");
			}

			jaiFront.Disconnect();
            jaiRear.Disconnect();
		}


		private int GetAllDisplayInformation()
        {
            int mno = 0;
            try
            {
                string strDispno;
                Debug.WriteLine("全てのディスプレイの情報取得");
                foreach (System.Windows.Forms.Screen screen_data in System.Windows.Forms.Screen.AllScreens)
                {
                    Debug.WriteLine("デバイス名 : " + screen_data.DeviceName);
                    Debug.WriteLine("ディスプレイの位置 : X=" + screen_data.Bounds.X + ", Y=" + screen_data.Bounds.Y);
                    Debug.WriteLine("ディスプレイのサイズ : 幅=" + screen_data.Bounds.Width + ", 高さ=" + screen_data.Bounds.Height);
                    //Debug.WriteLine("ディスプレイの作業領域の位置 : X=" + screen_data.WorkingArea.X + ",Y=" + screen_data.WorkingArea.Y);
                    //Debug.WriteLine("ディスプレイの作業領域のサイズ : 幅=" + screen_data.WorkingArea.Width + ", 高さ=" + screen_data.WorkingArea.Height);
                    Debug.WriteLine("-----");

                    strDispno = screen_data.DeviceName.Substring(screen_data.DeviceName.Length - 1);
                    mno = int.Parse(strDispno) - 1;
                    if (mno >= 0 && mno < MAX_MONITOR)
                    {
                        MonitorPos[mno].X = screen_data.Bounds.X;
                        MonitorPos[mno].Y = screen_data.Bounds.Y;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return mno;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (e.CloseReason == CloseReason.UserClosing &&
            DialogResult.No == MessageBox.Show(this.Owner, "システムを終了します。\nよろしいですか？", "終了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                e.Cancel = true;
            }
            else
            {
                if (_inspectResult.GetCount() > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show(this.Owner, "検査データを保存しますか？", "保存確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    {
                        SaveResultData();
                    }
                }
                LifeCtrl.UpdateLife();
                Helpers.Export(LightLifeFile, LifeCtrl);

                AvalBoardClose();

				SystemParam.InspectID = textInspectID.Text;
                SystemParam.CameraViewHeight = splitContainer3.SplitterDistance;

                Helpers.Export(SystemFile, SystemParam);

                String paramFile = Application.StartupPath + Define.InspectParamFilename;      // フルパス

                _inspectParam.AutoExposureLeft.CopyFrom(userCamViewFront.InspectParam.AutoExposureLeft);
				_inspectParam.AutoExposureRight.CopyFrom(userCamViewRear.InspectParam.AutoExposureRight);
				Helpers.Export(paramFile, _inspectParam);

				//Trace.WriteLine($"HFVisionInspect End");
				logger.Info($"HFVisionInspect End\n");


            }
        }

        private void SetManagerMode()
        {
            if (IsManagerMode)
            {
                menuSystemChangeMode.Text = "オペレータモード(&M)";
            }
            else
            {
                menuSystemChangeMode.Text = "管理者モード(&M)";
            }
            //menuChangeMode.Checked = IsManagerMode;
            //MenuFile.Enabled = IsManagerMode;
            menuSystemProperty.Enabled = IsManagerMode;
            menuInspect.Enabled = IsManagerMode;
            btInspectSetRight.Enabled = IsManagerMode;
			btInspectSetLeft.Enabled = IsManagerMode;

			userCamViewFront.ChangeOperatorMode(IsManagerMode);
            userCamViewRear.ChangeOperatorMode(IsManagerMode);
            logger.Info($"Chanege Manager Mode ({IsManagerMode})");
        }

        private void manuSystemChangeMode_Click(object sender, EventArgs e)
        {
            if (IsManagerMode)
            {
                IsManagerMode = false;
            }
            else
            {
                if (SystemParam.Password != "")
                {
                    FormInputBox cInput = new FormInputBox("パスワードを入力して下さい。", "パスワード確認", "");
                    if (cInput.ShowDialog() == DialogResult.OK)
                    {
                        if (cInput.sInputText != SystemParam.Password)
                        {
                            MessageBox.Show("パスワードが間違っています！！！", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                IsManagerMode = true;
            }
            SetManagerMode();
        }

        private void ImageProcFinRear()
        {
            int count = userCamViewRear.ResultData.Count;
            if (count > 0 && _cutterState != CutterState.Active)
            {
                ResultData data = new ResultData();
                bool bNG = false;
                //Trace.WriteLine($"Camera 0 Result Data Count = { count }");
                int defectCnt = 0;
                for (int i = 0; i < count; i++)
                {
					if (_inspectResult.CheckResultData(userCamViewRear.ResultData[i], SystemParam.CamInfo.RearCamera.BasePosX) == false)
						continue;
					int no = _inspectResult.SetData(userCamViewRear.ResultData[i], SystemParam.CamInfo.RearCamera.BasePosX);
                    data = _inspectResult.GetData(no);
                    if (no >= 0)
                        DispResultData(data);

                    if (data.JudgeOK == false)
                        bNG = true;

                    defectCnt++;

				}
                _loadDataDisp = false;

				int w, h;
				if (_imgDefect != null)
					_imgDefect.Dispose();
                if(defectCnt == 0)
                {
                    return;
				}

				hWinInspectLeft.HalconWindow.ClearWindow();
				hWinInspectLeft.HalconWindow.DispImage(userCamViewFront.ImgGrab.CopyImage());
				hWinInspectRight.HalconWindow.ClearWindow();
				_imgDefect = userCamViewRear.ImgGrab.CopyImage();
				_imgDefect.GetImageSize(out w, out h);
				hWinInspectRight.HalconWindow.DispImage(_imgDefect);
				hWinInspectRight.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
				hWinInspectRight.SetFullImagePart();
				hWinInspectLeft.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
				hWinInspectLeft.SetFullImagePart();

				double sx, sy, wx, wy;
				HTuple ty, tx;
				sx = w - 100;
				sy = 1;
				hWinInspectRight.HalconWindow.ConvertCoordinatesImageToWindow(sy, sx, out wy, out wx);
				ty = (double)wy;
				tx = (double)wx;
				HOperatorSet.DispText(hWinInspectRight.HalconWindow, SystemParam.RearCamera.Name, "window", ty, tx, "green", "box", "false");
				sx = 10;
				hWinInspectRight.HalconWindow.ConvertCoordinatesImageToWindow(sy, sx, out wy, out wx);
				ty = (double)wy;
				tx = (double)wx;
				HOperatorSet.DispText(hWinInspectLeft.HalconWindow, SystemParam.FrontCamera.Name, "window", ty, tx, "green", "box", "false");

				hWinInspectRight.HalconWindow.SetColor("red");
				int left, top, right, bottom;
				left = data.RegionRect.Left;
				right = data.RegionRect.Right;
				top = data.RegionRect.Top;
				bottom = data.RegionRect.Bottom;
				hWinInspectRight.HalconWindow.SetDraw("margin");
				hWinInspectRight.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);

    //               string font;
    //               font = hWinInspectImage0.HalconWindow.GetFont();
    //               font += "-40";
				//hWinInspectImage0.HalconWindow.SetFont(font);
				left = data.BlobRectangle.Left;
				right = data.BlobRectangle.Right;
				top = data.BlobRectangle.Top;
				bottom = data.BlobRectangle.Bottom;
				hWinInspectRight.HalconWindow.SetColor("yellow");
				hWinInspectRight.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);

				_formHistory.InspectResult = _inspectResult;
				_formHistory.UpdateInspectResult();

				if (bNG == true)
				{
					//SetInspectState(InspectState.Detection);
					//DefectAlarm(true);
					if (SystemParam.NGImageSave)
					{
						DateTime NowDate = DateTime.Now;
						string datestr = NowDate.ToString("yyyyMMdd");
						string timestr = NowDate.ToString("HHmmssfff");
						string sFilename = timestr;
						string sPath = string.Format("{0}\\{1}", SystemParam.NGImageSavePath, datestr);
						//_saveCtrl.SavePathname(_imgDefect, sPath, sFilename);
					}
				}
		    }
            else
            {
                if (_cutterState == CutterState.Active && count > 0)
                {
                    logger.Trace($"ImageProcFinCamera0 : Defect invalid({count})  Cutter Signal ");
                }
                return;
            }
        }

        private void ImageProcFinFront()
        {
            int count = userCamViewFront.ResultData.Count;
            bool bNG = false;
            if (count > 0 && _cutterState != CutterState.Active)
            {
                ResultData data = new ResultData();
				//Trace.WriteLine($"Camera 1 Result Data Count = { count }");
				int defectCnt = 0;
				for (int i = 0; i < count; i++)
                {
                    if (_inspectResult.CheckResultData(userCamViewFront.ResultData[i], SystemParam.CamInfo.FrontCamera.BasePosX) == false)
                        continue;
                    int no = _inspectResult.SetData(userCamViewFront.ResultData[i], SystemParam.CamInfo.FrontCamera.BasePosX);
                    data = _inspectResult.GetData(no);
                    if (no >= 0)
                        DispResultData(data);
 
                    if (data.JudgeOK == false)
                        bNG = true;
                    defectCnt++;

				}
                _loadDataDisp = false;

                int w, h;
                if (_imgDefect != null)
                    _imgDefect.Dispose();
				if (defectCnt == 0)
				{
					return;
				}
                hWinInspectRight.HalconWindow.ClearWindow();
				hWinInspectRight.HalconWindow.DispImage(userCamViewRear.ImgGrab.CopyImage());
				hWinInspectLeft.HalconWindow.ClearWindow();
				_imgDefect = userCamViewFront.ImgGrab.CopyImage();
				_imgDefect.GetImageSize(out w, out h);
                hWinInspectLeft.HalconWindow.DispImage(_imgDefect);
                hWinInspectLeft.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
                hWinInspectLeft.SetFullImagePart();
				hWinInspectRight.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
				hWinInspectRight.SetFullImagePart();

				double sx, sy, wx, wy;
				HTuple ty, tx;
				sx = 10;
				sy = 1;
				hWinInspectLeft.HalconWindow.ConvertCoordinatesImageToWindow(sy, sx, out wy, out wx);
				ty = (double)wy;
				tx = (double)wx;
				HOperatorSet.DispText(hWinInspectLeft.HalconWindow, SystemParam.FrontCamera.Name, "window", ty, tx, "green", "box", "false");
				sx = w - 100;
				sy = 10;
				hWinInspectLeft.HalconWindow.ConvertCoordinatesImageToWindow(sy, sx, out wy, out wx);
				ty = (double)wy;
				tx = (double)wx;
				HOperatorSet.DispText(hWinInspectRight.HalconWindow, SystemParam.RearCamera.Name, "window", ty, tx, "green", "box", "false");

				if (defectCnt > 0)
                {
                    hWinInspectLeft.HalconWindow.SetColor("red");
                    int left, top, right, bottom;
                    left = data.RegionRect.Left;
                    right = data.RegionRect.Right;
                    top = data.RegionRect.Top;
                    bottom = data.RegionRect.Bottom;
                    hWinInspectLeft.HalconWindow.SetDraw("margin");
                    hWinInspectLeft.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);

					left = data.BlobRectangle.Left;
					right = data.BlobRectangle.Right;
					top = data.BlobRectangle.Top;
					bottom = data.BlobRectangle.Bottom;
					hWinInspectLeft.HalconWindow.SetColor("yellow");
					hWinInspectLeft.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);

					_formHistory.InspectResult = _inspectResult;
                    _formHistory.UpdateInspectResult();

                    if (bNG == true)
                    {
                        //SetInspectState(InspectState.Detection);
                        //DefectAlarm(true);
                        if (SystemParam.NGImageSave)
                        {
                            DateTime NowDate = DateTime.Now;
                            string datestr = NowDate.ToString("yyyyMMdd");
                            string timestr = NowDate.ToString("HHmmssfff");
                            string sFilename = timestr;
                            string sPath = string.Format("{0}\\{1}", SystemParam.NGImageSavePath, datestr);
                            //_saveCtrl.SavePathname(_imgDefect, sPath, sFilename);
                        }
                    }
                }
            }
            else
            {
                if (_cutterState == CutterState.Active && count > 0)
                {
                    logger.Trace($"ImageProcFinCamera1 : Defect invalid({count})  Cutter Signal ");
                }
                return;
            }
        }

        private void  DispResultData(ResultData resultData) 
        {
            string logText = "検出履歴 : ";

            string[] sItem = new string[20];
            if (resultData.No >= 0)
            {
                DispResultImage(resultData);

                sItem[0] = (resultData.No).ToString();
                sItem[1] = resultData.Date.ToLongDateString();
                sItem[2] = resultData.Date.ToLongTimeString();
                sItem[3] = resultData.BlobArea.ToString("0");
                sItem[4] = resultData.BlobWidht.ToString("0");
                sItem[5] = resultData.BlobLength.ToString("0");
                sItem[6] = resultData.BlobPosX.ToString("0");
                sItem[7] = resultData.JudgeOK ? "OK" : "NG";
                sItem[8] = resultData.JudgeLevel.ToString();

                logText += $"{sItem[0]}, {sItem[1]}, {sItem[2]}, {sItem[3]}, {sItem[4]}, {sItem[5]}, {sItem[6]}, {sItem[7]}, {sItem[8]}";
            }
            else
            {
                if (resultData.No == (int)ResultType.InspectStart)
                    sItem[0] = "検査開始";
                else if (resultData.No == (int)ResultType.InspectStop)
                    sItem[0] = "検査停止";
                else if (resultData.No == (int)ResultType.CutterOn)
                    sItem[0] = "検査中断(自動)";                    // 20250523文字列変更
				else if (resultData.No == (int)ResultType.CutterOff)
                    sItem[0] = "検査開始(自動)";
				else if (resultData.No == (int)ResultType.PosWait)
					sItem[0] = "カメラ待機位置移動";
				else if (resultData.No == (int)ResultType.PosInspect)
					sItem[0] = "カメラ検査位置移動";
				else
					return;
                sItem[1] = resultData.Date.ToLongDateString();
                sItem[2] = resultData.Date.ToLongTimeString();
                sItem[3] = "";
                sItem[4] = "";
                sItem[5] = "";
                sItem[6] = "";
                sItem[7] = "";
                sItem[8] = "";
				logText += $"{sItem[0]}, {sItem[1]}, {sItem[2]}";
			}
			ListViewItem additem = new ListViewItem(new string[] { sItem[0], sItem[1], sItem[2], sItem[3], sItem[4], sItem[5], sItem[6], sItem[7], sItem[8] });
            listInspectResult.Items.Add(additem);

            logger.Info(logText);

            int count = listInspectResult.Items.Count;
             if (count > 0)
            {
                //listInspectResult.Items[count - 1].Selected = true;
                //listInspectResult.Select();
                listInspectResult.EnsureVisible(count - 1);
            }

        }

        private void DispResultImage(ResultData resultData)
        {
            if (resultData.No < 0)
                return;
            int w, h;
            hWinDefectImage.HalconWindow.ClearWindow();
            resultData.BlobImage.GetImageSize(out w, out h);
            hWinDefectImage.HalconWindow.DispImage(resultData.BlobImage);
            hWinDefectImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
            hWinDefectImage.SetFullImagePart();

            hWinDefectProcImage.HalconWindow.ClearWindow();
            resultData.BlobProcImage.GetImageSize(out w, out h);
            hWinDefectProcImage.HalconWindow.DispImage(resultData.BlobProcImage);
            hWinDefectProcImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
            hWinDefectProcImage.SetFullImagePart();
        }

        private void DispResultData(InspectResultData resultData)
        {
            listInspectResult.Items.Clear();
            int count = resultData.GetCount();
            for (int i = 0; i < count; i++)
            {
                ResultData data = resultData.GetData(i);
                DispResultData(data);
            }
        }

        private bool SetInspectState(InspectState state)
        {
            bool ret = true;
   //         if ( _inspState != InspectState.PosWait)
   //         {
   //             logger.Trace("Position Wait (Ready)");
			//	_inspState = InspectState.PosWait;
			//	butInspectStart.BackColor = Color.DarkGreen;
			//	butInspectStart.Enabled = false;
			//	butInspectStop.BackColor = Color.DarkRed;
			//	butInspectStop.Enabled = false;
			//	chkLightPower.Enabled = true;

			//	btInspectSetRight.Enabled = true;
			//	btInspectSetLeft.Enabled = true;
			//	butResultSave.Enabled = true;
			//	btLoadInspectResult.Enabled = true;
			//	userCamViewFront.InspectStart(false);
			//	userCamViewRear.InspectStart(false);
			//	MenuFile.Enabled = IsManagerMode;
			//	menuInspect.Enabled = IsManagerMode;

			//	labelInspectState.Text = "待機中";
			//	labelInspectState.BackColor = Color.Gray;
			//	return false;
			//}
			//else if (state == InspectState.PosInspect && (_inspState == InspectState.PosWait || _inspState == InspectState.Ready))
   //         {
			//	//Trace.WriteLine("Position Inspect (Ready)");
			//	logger.Trace("Position Inspect (Ready)");
			//	_inspState = InspectState.Ready;
			//	butInspectStart.BackColor = Color.LightGreen;
			//	butInspectStart.Enabled = true;

			//	return false;
			//}

			if (state == InspectState.Ready && _inspState != InspectState.Ready)            // 待機中
            {
				logger.Trace("Inspect Stop(Ready)");
				_inspState = InspectState.Ready;

                butInspectStart.BackColor = Color.LightGreen;
                butInspectStart.Enabled = true;
                butInspectStop.BackColor = Color.DarkRed;
                butInspectStop.Enabled = false;
                chkLightPower.Enabled = true;

                btInspectSetRight.Enabled = true;
				btInspectSetLeft.Enabled = true;
				butResultSave.Enabled = true;
                btLoadInspectResult.Enabled = true;
                userCamViewFront.InspectStart(false);
                userCamViewRear.InspectStart(false);
                MenuFile.Enabled = IsManagerMode;
                menuInspect.Enabled = IsManagerMode;

                labelInspectState.Text = "待機中";
                labelInspectState.BackColor = Color.Gray;
            }
            else if (state == InspectState.Inspect && (_inspState == InspectState.Ready || _inspState == InspectState.Detection))    // 検査中
            {
                //Trace.WriteLine("Inspect Start");
				logger.Trace("Inspect Start");

				if (_loadDataDisp)
                {
                    DispResultData(_inspectResult);
                    _loadDataDisp = false;
                }
                _inspState = InspectState.Inspect;

                butInspectStart.BackColor = Color.DarkGreen;
                butInspectStart.Enabled = false;
                butInspectStop.BackColor = Color.Red;
                butInspectStop.Enabled = true;
                labelDisplayJudge.BackColor = Color.Gray;
                labelDisplayJudge.Text = "- -";
                chkLightPower.Enabled = false;

                btInspectSetRight.Enabled = false;
				btInspectSetLeft.Enabled = false;
				butResultSave.Enabled = false;
                btLoadInspectResult.Enabled = false;
                userCamViewFront.InspectStart(true);
                userCamViewRear.InspectStart(true);
                MenuFile.Enabled = false;
                menuInspect.Enabled = false;

                labelInspectState.Text = "検査中";
                labelInspectState.BackColor = Color.LightGreen;
            }
			else if (state == InspectState.Reset)    // 結果リストクリア
			{
				//Trace.WriteLine("Inspect Result Reset");
				logger.Trace("Inspect Result Reset");
				SaveResultData();
				_inspectResult.Clear();
				DispResultData(_inspectResult);
				butResultReset.Enabled = false;

			}
			else if (state == InspectState.Detection && _inspState == InspectState.Inspect)       // 欠陥アラーム出力
            {
                //Trace.WriteLine("Inspect Detection");
				logger.Trace("Inspect Detection");
				_inspState = InspectState.Detection;

                labelDisplayJudge.BackColor = Color.Red;
                labelDisplayJudge.Text = "N G";

                //userCamView0.InspectStart(false);
                //userCamView1.InspectStart(false);

                labelInspectState.Text = "欠陥検出";
                labelInspectState.BackColor = Color.Red;
            }
            else if (state == InspectState.Startup)     // 起動時
            {
				//Trace.WriteLine("Startup");
				logger.Trace("Startup");
				_inspState = InspectState.Startup;

				butInspectStart.BackColor = Color.LightGreen;
				//butInspectStart.BackColor = Color.DarkGreen;
				butInspectStart.Enabled = true;
				butInspectStop.BackColor = Color.DarkRed;
				butInspectStop.Enabled = false;
			}
            else
                ret = false;

            return ret;

        }

        private void butInspectStart_Click(object sender, EventArgs e)
        {
            DioInspectStart();
        }

        private void butInspectStop_Click(object sender, EventArgs e)
        {
            DioInspectStop();
        }

        private void DioInspectStart()
        {
            Invoke((MethodInvoker)delegate ()
            {
                if (chkLightPower.Checked == false)
                {
                    MessageBox.Show("照明が点灯していません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;            
                }

                if (SetInspectState(InspectState.Inspect) == false)
                    return;
                //if (chkLightPower.Checked == false)
                //{
                //    LightPower(true);
                //    System.Threading.Thread.Sleep(2000);
                //}
                ResultData data = new ResultData();
                data.CopyFrom(_inspectResult.SetStart());
                DispResultData(data);

            });
        }

        private void DioInspectStop()
        {

            Invoke((MethodInvoker)delegate ()
            {
                if (SetInspectState(InspectState.Ready) == false)
                    return;

                ResultData data = new ResultData();
                data.CopyFrom(_inspectResult.SetStop());
                DispResultData(data);

           });
        }
        private void DioInspectReset()
        {
            Trace.WriteLine("DioInspectReset");
            Invoke((MethodInvoker)delegate ()
            {
                SetInspectState(InspectState.Reset);
                //DefectAlarmReset();
            });
        }

        private void DioInspectRelease()
        {
            Trace.WriteLine("DioInspectRelease");
            Invoke((MethodInvoker)delegate ()
            {
                SetInspectState(InspectState.Inspect);
                //dioCtrl.SetSignalYellow(false);
                //DefectAlarm(false);
            });
        }

        private void listInspectResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listInspectResult.SelectedItems.Count == 0)
                return;
            int iIndex = listInspectResult.SelectedItems[0].Index;

            if (_loadDataDisp)
            {
                int dataCount = _loadResult.GetCount();
                if (iIndex >= 0 && iIndex < dataCount)
                {
                    ResultData data = _loadResult.GetData(iIndex);
                    DispResultImage(data);
                }
            }
            else
            {
                int dataCount = _inspectResult.GetCount();
                if (iIndex >= 0 && iIndex < dataCount)
                {
                    ResultData data = _inspectResult.GetData(iIndex);
                    DispResultImage(data);
                }
            }
        }

        private void manuSystemProperty_Click(object sender, EventArgs e)
        {
            logger.Info($"Open Property Form...");
            FormSystemParam dlg = new FormSystemParam();
            dlg.operatorMode = false;
            dlg.SystemParam.CopyFrom(SystemParam);

			if (dlg.ShowDialog() == DialogResult.OK)
            {
                SystemParam.CopyFrom(dlg.SystemParam);
                Helpers.Export(SystemFile, SystemParam);
                _inspectResult.CameraResolution = SystemParam.CamInfo.Resolution;
                _inspectResult.RecordWidth = SystemParam.DefectImageWidth;
                _inspectResult.RecoedHeight = SystemParam.DefectImageHeight;
				_inspectResult.RegistPosMargin = SystemParam.RegistPosRange;
				_inspectResult.RegistTimeMargin = SystemParam.RegistTimeRange;

				userCamViewFront.SystemParam.CopyFrom(SystemParam);
                userCamViewRear.SystemParam.CopyFrom(SystemParam);
                //userCamViewFront.SetCamera();
                //userCamViewRear.SetCamera();
                //CameraViewLine.SetParam();
                SetCameraInfo();

				//dioCtrl.Close();
				//_resultCtrl.Close();
			}
            logger.Info($"... Close Property Form");
        }

        private void timerLightLife_Tick(object sender, EventArgs e)
        {
            GetLightLife();

        }

        private void imageGrayCheck()
        {
            double mean0, mean1;

			if (checkAutoExposure.Checked)
            {
				mean0 = userCamViewFront.GetMeanGray(checkAutoExposure.Checked);
				lblGrayMeanLeft.Text = mean0.ToString("0.0");
				mean1 = userCamViewRear.GetMeanGray(checkAutoExposure.Checked);
				lblGrayMeanRight.Text = mean1.ToString("0.0");


			}
            else
            {
                mean0 = userCamViewFront.GetImageGray();
				lblGrayMeanLeft.Text = mean0.ToString("0.0");
				mean1 = userCamViewRear.GetImageGray();
				lblGrayMeanRight.Text = mean1.ToString("0.0");

			}
			if (_isLightGrayAlarm == false && (mean0 < SystemParam.GrayMeanLimit || mean1 < SystemParam.GrayMeanLimit))
			{
				_isLightGrayAlarm = true;
				labelMessage.Text = "平均輝度値が基準設定より低くなっています";

				//new Thread(new ThreadStart(delegate
				//{

				//	using (Form frm = new Form())
				//	{
				//		frm.TopMost = true;
				//		if (MessageBox.Show("平均輝度値が低くなっています。照明ランプの交換をお勧めします。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
				//		{
				//			_isLightAlarm = false;
				//		}
				//		frm.TopMost = false;
				//	}
				//})).Start();
			}
			else if (_isLightGrayAlarm == true && (mean0 >= SystemParam.GrayMeanLimit && mean1 >= SystemParam.GrayMeanLimit))
			{
				_isLightGrayAlarm = false;
			}
			labelMessage.Visible = _isLightGrayAlarm || _isLightLifeAlarm; 
		}

		private void GetLightLife()
        {
            double dH = LifeCtrl.GetLifeHour();
            labelLifeHour.Text = dH.ToString("0");
            if (_isLightLifeAlarm == false && dH >= SystemParam.LightLifeHourLimit)
            {
				_isLightLifeAlarm = true;
				labelMessage.Text = "照明点灯時間が寿命時間を超えています";
				//new Thread(new ThreadStart(delegate
				//            {
				//                if (MessageBox.Show("照明点灯時間が設定を超えています。照明ランプの交換をお勧めします。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
				//                {
				//		_isLightAlarm = false;
				//	}
				//})).Start();
			}
			else if (_isLightLifeAlarm == true && dH < SystemParam.LightLifeHourLimit)
            {
				_isLightLifeAlarm = false;
			}
			labelMessage.Visible = _isLightGrayAlarm || _isLightLifeAlarm;
		}


		private void menuLightLifeReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("照明点灯時間をリセットします\nよろしいでしょうか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                LifeCtrl.ResetLife();
                Helpers.Export(LightLifeFile, LifeCtrl);
                GetLightLife();
            }
        }

        private void chkLightPower_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLightPower.Checked)
            {
                LightPower(true);
				SetInspectState(InspectState.Ready);
                //timerLightLife.Start();
                //LifeCtrl.UpdateLife();
			}
			else
            {
                LightPower(false);
				SetInspectState(InspectState.Startup);
				//timerLightLife.Stop();
				//LifeCtrl.UpdateLife();
			}
		}

        private void LightPower(bool on)
        {
            if (on)
            {
                chkLightPower.BackColor = Color.Yellow;
                chkLightPower.Text = "照明 点灯中";
            }
            else
            {
                chkLightPower.BackColor = Color.DarkGreen;
                chkLightPower.Text = "照明 消灯中";
            }
            chkLightPower.Checked = on;
        }


        //private void DefectAlarm(bool on)
        //{
        //    if (on)
        //    {
        //        labelDisplayJudge.BackColor = Color.Red;
        //        labelDisplayJudge.Text = "N G";
        //        dioCtrl.SetSignalRed(true);
        //        dioCtrl.SetBuzzer(0, true);
        //    }
        //    else
        //    {
        //        labelDisplayJudge.BackColor = Color.LightGreen;
        //        labelDisplayJudge.Text = "O k";
        //    }
        //}

        //private void DefectAlarmReset()
        //{
        //    labelDisplayJudge.BackColor = Color.Gray;
        //    labelDisplayJudge.Text = "- -";
        //    dioCtrl.SetSignalRed(false);
        //    dioCtrl.SetBuzzer(0, false);
        //}

         private void btInspectSetRight_Click(object sender, EventArgs e)
        {
            //InsepectSet(SystemParam.RightCamera.No);
			FormInspectSetting frmSetting = new FormInspectSetting();
			frmSetting.InspParam.CopyFrom(_inspectParam);
			frmSetting.InspParam.AutoExposureRight.CopyFrom(userCamViewRear.InspectParam.AutoExposureRight);
			frmSetting.CameraResolution = SystemParam.CamInfo.Resolution;
			frmSetting.SetControlID = ViewID.ViewRear;
			frmSetting.ImgSrc = userCamViewRear.ImgGrab.CopyImage();
            frmSetting.StartPosition = FormStartPosition.CenterScreen;

			if (frmSetting.ShowDialog() == DialogResult.OK)
			{
				_inspectParam.CopyFrom(frmSetting.InspParam);
				userCamViewFront.InspectParam.CopyFrom(_inspectParam);
				userCamViewRear.InspectParam.CopyFrom(_inspectParam);
				String paramFile = Application.StartupPath + Define.InspectParamFilename;      // フルパス
				Helpers.Export(paramFile, _inspectParam);

			}

		}

		private void btInspectSetLeft_Click(object sender, EventArgs e)
		{
			//InsepectSet(SystemParam.LeftCamera.No);
			FormInspectSetting frmSetting = new FormInspectSetting();
			frmSetting.InspParam.CopyFrom(_inspectParam);
			frmSetting.InspParam.AutoExposureLeft.CopyFrom(userCamViewFront.InspectParam.AutoExposureLeft);
			frmSetting.CameraResolution = SystemParam.CamInfo.Resolution;
            if (ActiveTabNo == TabSelect.FrontCamera)
            {
				frmSetting.SetControlID = ViewID.ViewFront;
				frmSetting.ImgSrc = userCamViewFront.ImgGrab.CopyImage();
			}
			else if (ActiveTabNo == TabSelect.RearCamera)
            {
				frmSetting.SetControlID = ViewID.ViewRear;
				frmSetting.ImgSrc = userCamViewRear.ImgGrab.CopyImage();
			}
			else
            {
                return;
            }
			frmSetting.StartPosition = FormStartPosition.CenterScreen;

			if (frmSetting.ShowDialog() == DialogResult.OK)
			{
				_inspectParam.CopyFrom(frmSetting.InspParam);
				userCamViewFront.InspectParam.CopyFrom(_inspectParam);
				userCamViewRear.InspectParam.CopyFrom(_inspectParam);
				String paramFile = Application.StartupPath + Define.InspectParamFilename;      // フルパス
				Helpers.Export(paramFile, _inspectParam);

			}
		}

		private void InsepectSet(int camNo)
        {
			FormInspectSetting frmSetting = new FormInspectSetting();
			userCamViewFront.InspectParam.CopyFrom(_inspectParam);
			frmSetting.InspParam.CopyFrom(_inspectParam);
			frmSetting.CameraResolution = SystemParam.CamInfo.Resolution;
			if (camNo == 0)
			{
				frmSetting.SetControlID = ViewID.ViewRear;
				frmSetting.ImgSrc = userCamViewRear.ImgGrab.CopyImage();
			}
			else
			{
				frmSetting.SetControlID = ViewID.ViewFront;
				frmSetting.ImgSrc = userCamViewFront.ImgGrab.CopyImage();
			}

			if (frmSetting.ShowDialog() == DialogResult.OK)
			{
				_inspectParam.CopyFrom(frmSetting.InspParam);
				userCamViewFront.InspectParam.CopyFrom(_inspectParam);
				userCamViewRear.InspectParam.CopyFrom(_inspectParam);
				String paramFile = Application.StartupPath + Define.InspectParamFilename;      // フルパス
				Helpers.Export(paramFile, _inspectParam);

			}
		}

		private void butResultSave_Click(object sender, EventArgs e)
        {
            SaveResultData();
        }

        private void SaveResultData()
        {
            Cursor preCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            string id = textInspectID.Text;
            string path = SystemParam.ImageSavePath;
            if (id != "")
                path += $"\\{id}";
            _inspectResult.SaveData(path);
            Cursor.Current = preCursor;

            long HDDSpace = Helpers.HDDFreeSpace(SystemParam.NGImageSavePath);     // HDD容量確認
            long HDDAlarmLevel = SystemParam.HDDCapacityLimit;
            logger.Debug($"HDD空き容量 : {HDDSpace} GB");
            if (HDDSpace < HDDAlarmLevel)
            {
                MessageBox.Show(this, "ディスクの容量が少なくなっています。\n古いデータを削除してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btLoadInspectResult_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            //ofd.InitialDirectory = InitialDirectory;
            ofd.InitialDirectory = SystemParam.ImageSavePath;
            ofd.Filter = "検査結果ファイル(*.csv) | *.csv";

            ofd.FilterIndex = 0;
            ofd.Title = "検査結果ファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                InitialDirectory = ofd.FileName;
                string pathname = Path.GetDirectoryName(InitialDirectory);
                string filename = Path.GetFileName(InitialDirectory);

                if ( _loadResult.LoadData(pathname, filename) == true)
                {
                    DispResultData(_loadResult);
                    _loadDataDisp = true;
                }
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            //if (MessageBox.Show(this.Owner, "照明を点灯しますか？", "照明確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LightPower(true);
                chkLightPower.Checked = true;
                SetInspectState(InspectState.Ready);
            }
            //else 
            //{
            //    SetInspectState(InspectState.Startup);
            //}
            //_formHistory.Show(this);
            WindowState = FormWindowState.Maximized;
            splitContainer3.SplitterDistance = SystemParam.CameraViewHeight;

			if (File.Exists(Define.LastImageFilenameLeft))
            {
			}

		}

		private void menuInspectRecipeLoad_Click(object sender, EventArgs e)
        {
            ReadRecipe();
        }

        private void ReadRecipe()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.InitialDirectory = SystemParam.RecipePath;
            ofd.Filter = "設定ファイル (*.rcp)|*.rcp|すべてのファイル (*.*)|*.*";

            ofd.FilterIndex = 0;
            ofd.Title = "設定ファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                Helpers.Import(filename, ref _inspectParam);
                SystemParam.ActiveRecipefile = System.IO.Path.GetFileName(filename);
                //labelRecipeName.Text = InspectParam.RecipeName;
                logger.Info($"Load Recipe File ({_inspectParam.RecipeName})");

				userCamViewFront.InspectParam.CopyFrom(_inspectParam);
                userCamViewRear.InspectParam.CopyFrom(_inspectParam);
            }
        }


        private void menuInspectRecipeSave_Click(object sender, EventArgs e)
        {
            WriteRecipe();
        }

        private void WriteRecipe()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = _inspectParam.RecipeName;
            ofd.InitialDirectory = SystemParam.RecipePath;
            ofd.Filter = "rcpファイル(*.rcp)|*.rcp";

            ofd.FilterIndex = 0;
            ofd.Title = "設定ファイル名を入力してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                SystemParam.ActiveRecipefile = filename;
                Helpers.Export(filename, _inspectParam);
                //labelRecipeName.Text = InspectParam.RecipeName;
                logger.Info($"Write Recipe File ({_inspectParam.RecipeName})");
            }

        }

        private void timerCutter_Tick(object sender, EventArgs e)
        {
            timerCutter.Stop();
            if (_cutterState == CutterState.Delay)
            {
                _cutterState = CutterState.Active;
                timerCutter.Interval = SystemParam.CutterTimer;
                timerCutter.Start();
                Trace.WriteLine($"DioInCutter TimerStart : {timerCutter.Interval} ms");
                labelInspectState.Text = "検査中断";
                labelInspectState.BackColor = Color.Yellow;
                ResultData data = new ResultData();
                data.CopyFrom(_inspectResult.SetCutter(true));
                DispResultData(data);
            }
            else
            {
                _cutterState = CutterState.None; 
                labelInspectState.Text = "検査中";
                labelInspectState.BackColor = Color.LightGreen;
                logger.Info($"CutterTimer Stop ");
                Trace.WriteLine($"CutterTimer Stop ");
                ResultData data = new ResultData();
                data.CopyFrom(_inspectResult.SetCutter(false));
                DispResultData(data);
            }
        }

        private void menuInspectResultClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this.Owner, "結果データをリセットします。\nよろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _inspectResult.Clear();
                DispResultData(_inspectResult);
            }

        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                //_formHistory.FormMaximized();

            }
        }

        private void timerCheckBright_Tick(object sender, EventArgs e)
        {
            DateTime dt = System.DateTime.Now;
            labelDatetime.Text = dt.ToString("yyyy/MM/dd HH:mm:ss");

            imageGrayCheck();
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            userCamViewFront.DispRedraw();
            userCamViewRear.DispRedraw();
        }

        private void tabImageView_Selected(object sender, TabControlEventArgs e)
        {
            userCamViewFront.DispRedraw();
            userCamViewRear.DispRedraw();

        }

		private void menuDioCheck_Click(object sender, EventArgs e)
		{
		}

		private void checkAutoExposure_CheckedChanged(object sender, EventArgs e)
		{
			if (checkAutoExposure.Checked)
			{
				checkAutoExposure.BackColor = Color.LightGreen;
				checkAutoExposure.Text = "自動露光 ON";
			}
			else
			{
				checkAutoExposure.BackColor = Color.DarkGreen;
				checkAutoExposure.Text = "自動露光 OFF";
			}
		}

		private void timerDate_Tick(object sender, EventArgs e)
		{
			if (DateTime.Now.Date != lastDate)
			{
				logger.Info($"日付変更 結果リスト書き込み");
				lastDate = DateTime.Now.Date;

				Cursor preCursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				string id = textInspectID.Text;
				string path = SystemParam.ImageSavePath + Properties.Settings.Default.ResultLogBack;
				_inspectResult.SaveData(path);
				Cursor.Current = preCursor;

				long HDDSpace = Helpers.HDDFreeSpace(SystemParam.ImageSavePath);     // HDD容量確認
				long HDDAlarmLevel = SystemParam.HDDCapacityLimit;
				logger.Debug($"HDD空き容量 : {HDDSpace} GB");
				if (HDDSpace < HDDAlarmLevel)
				{
					MessageBox.Show(this, "ディスクの容量が少なくなっています。\n古いデータを削除してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

			}
		}

		private void tabImageView_SelectedIndexChanged(object sender, EventArgs e)
		{
            ActiveTabNo = (TabSelect)tabImageView.SelectedIndex;
		}

		private void butInspRelease_Click(object sender, EventArgs e)
		{

		}

		private void butBuzzerReset_Click(object sender, EventArgs e)
		{

		}

		private void butResultReset_Click(object sender, EventArgs e)
		{
			DioInspectReset();
		}

		private void butNGRelease_Click(object sender, EventArgs e)
		{
			DioInspectRelease();
		}
	}
}
