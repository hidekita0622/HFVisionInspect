using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFVisionInspect
{
	public enum ProcState
	{
		NONE = 0,
		GRAB,
		PROC,
		PROCFIN,
	}

	public partial class UserCameraViewFront : UserControl
	{
		private const int SnapTimeoutMs = 3000;

		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private string InitialDirectory = Application.StartupPath;
		//private HImage _imgGrab0 = new HImage();
		//private HImage _imgGrab1 = new HImage();
		public HImage ImgGrab = new HImage();
		public HImage ImgProc = new HImage();
		public HImage ImgDisp = new HImage();

		private InspectControl _inspCtrl;
		//private ClassAcapGrab _grabCtrl;
		public AcapChannel AcapCtrl;
		private int _cameraCH;

		public ClassSystemParam SystemParam = new ClassSystemParam();
		public InspectParameter InspectParam { get; set; }
		public ClassImageProc _imgProc = new ClassImageProc();
		public Action OnResult;
		private int CameraNo;
		public ProcState CameraStatus { get; set; }
		private bool _mgrMode = false;
		//private int _grabSel = 0;
		public bool IsInspect { get; set; } = false;
		public List<ResultInfo> ResultData { get; set; }

		bool _isProcFin = false;
		Stopwatch _procSW = new Stopwatch();
		private int _countFrame;
		private bool _frameRenew = false;
		//private ClassImageWindow _showImgCtrl;

		private List<double> _grayData = new List<double>();

		public UserCameraViewFront()
		{
			InitializeComponent();
			_countFrame = 0;
			InspectParam = new InspectParameter();
			_inspCtrl = new InspectControl();
		}

		public void InitialControl(CameraViewInfo info)
		{
			InitialDirectory = SystemParam.NGImageSavePath;
			CameraNo = info.CameraNo;
			labelViewName.Text = info.Name;
			_cameraCH = Properties.Settings.Default.FrontCameraCH;

			this.MouseWheel += hSmartWinMouseWheel;
			ImgGrab = new HImage();
			ImgGrab.GenEmptyObj();
			//ImgDiffBase.GenEmptyObj();


			_inspCtrl.ControlID = info.ID;
			_inspCtrl.InspectTaskRun();
			_inspCtrl.OnProcEnd += InspectFin;
			ResultData = new List<ResultInfo>();
		}

		private void menuMouseIReadImage_Click(object sender, EventArgs e)
		{
			LoadImage();
		}

		public void DispRedraw()
		{
			try
			{
				if (!ImgDisp.IsInitialized())
					return;
				//int w, h;
				//_imgDisp.GetImageSize(out w, out h);
				//hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
				//hCamImage.HalconWindow.SetPart((int)0, 0, -2, -2);
				hCamImage.SetFullImagePart();
				DispImage();
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}
		}

		private void DispImage()
		{
			try
			{
				if (!ImgDisp.IsInitialized())
					return;
				hCamImage.HalconWindow.ClearWindow();
				string type;
				int w, h;
				ImgDisp.GetImagePointer1(out type, out w, out h);
				hCamImage.HalconWindow.DispImage(ImgDisp);
				//hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
				//hCamImage.SetFullImagePart();

				hCamImage.HalconWindow.SetDraw("margin");
				hCamImage.HalconWindow.SetColor("maroon");
				hCamImage.HalconWindow.DispRectangle1(0, 0, (double)h - 1, (double)w - 1);

				if (_isProcFin)
				{
					if (checkShowResult.Checked)
					{
						//hCamImage.HalconWindow.SetColor("red");
						//hCamImage.HalconWindow.SetDraw("fill");
						_inspCtrl.DrawBlobs(hCamImage.HalconWindow);
					}
				}
				else
				{
					//hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
					hCamImage.SetFullImagePart();
				}

				if (checkShowInspArea.Checked)
				{
					hCamImage.HalconWindow.SetColor("cyan");
					int left, top, right, bottom;
					left = InspectParam.InspAreaLeft.Left;
					right = InspectParam.InspAreaLeft.Right;
					top = InspectParam.InspAreaLeft.Top;
					bottom = InspectParam.InspAreaLeft.Bottom;
					hCamImage.HalconWindow.SetDraw("margin");
					hCamImage.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);
				}

				hCamImage.Refresh();
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}
		}

		public void LoadImage()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.FileName = "";
			//ofd.InitialDirectory = SystemParam.ImageSavePath;
			ofd.InitialDirectory = InitialDirectory;
			//ofd.Filter = "BMPファイル(*.bmp)|*.bmp|PNGファイル(*.png)|*.png";
			ofd.Filter = "すべてのファイル(*.*) | *.*";

			ofd.FilterIndex = 0;
			ofd.Title = "画像ファイルを選択してください";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				HObject loadImg;
				HOperatorSet.GenEmptyObj(out loadImg);
				HImage imgLoad = new HImage();
				imgLoad.GenEmptyObj();
				InitialDirectory = ofd.FileName;
				imgLoad.ReadImage(ofd.FileName);

				HTuple type = imgLoad.GetImageType();
				string stype = type.S;


				if (ImgGrab != null)
					ImgGrab.Dispose();
				ImgGrab = imgLoad.ConvertImageType("byte");
				//_imgProc = imgLoad.CopyImage();
				if (ImgDisp != null)
					ImgDisp.Dispose();
				ImgDisp = ImgGrab.CopyImage();
				DispImage();
				loadImg.Dispose();
				_frameRenew = true;

			}
		}

		private void SaveImage()
		{
			SaveFileDialog ofd = new SaveFileDialog();
			ofd.FileName = "";
			ofd.InitialDirectory = InitialDirectory;
			ofd.Filter = "BMPファイル(*.bmp)|*.bmp";

			ofd.FilterIndex = 0;
			ofd.Title = "画像ファイルを入力してください";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = false;

			//ダイアログを表示する
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//OKボタンがクリックされたとき、選択されたファイル
				InitialDirectory = ofd.FileName;

				//ImageSaveCtrl.SaveFile(HoDispImg, ofd.FileName);
				SaveImage(ofd.FileName);
			}
		}

		public void SaveImage(string Filename)
		{
			try
			{
				if (ImgDisp != null)
				{
					//画像ファイル書込み
					ImgDisp.WriteImage("bmp", 0, Filename);
				}
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}
		}

		private void OnFrameEnd()
		{
			Invoke((MethodInvoker)delegate ()
			{
				_countFrame++;
				_frameRenew = true;
				if (CameraStatus == ProcState.NONE)
				{
					CameraStatus = ProcState.GRAB;
					//Helpers.WriteLog($"Line Camera : OnFrameEnd ");

					//ClassImageProc proc = new ClassImageProc();
					if (ImgGrab != null)
						ImgGrab.Dispose();


#if false
                    if (_grabSel == 0)
                    {
                        if (_imgGrab0 != null)
                            _imgGrab0.Dispose();
                        _imgGrab0 = _grabCtrl.ImgGrab.CopyImage();

                        proc.TiledImage(_imgGrab1, _imgGrab0, out _imgProc);
                        _grabSel = 1;

                    }
                    else
                    {
                        if (_imgGrab1 != null)
                            _imgGrab1.Dispose();
                        _imgGrab1 = _grabCtrl.ImgGrab.CopyImage();

                        proc.TiledImage(_imgGrab0, _imgGrab1, out _imgProc);
                        _grabSel = 0;
                    }
#else
					//ImgProc = _grabCtrl.ImgGrab.CopyImage();

#endif
					if (IsInspect && !checkInspectOff.Checked)
					{
						CameraStatus = ProcState.PROC;
						InspectRun();
						//Inspect();
					}
					else if (checkRedrawImage.Checked)
					{
						if (ImgDisp != null)
							ImgDisp.Dispose();
						ImgDisp = ImgGrab.CopyImage();
						DispImage();
					}
					CameraStatus = ProcState.NONE;
					//proc.Dispose();
				}
			});
		}

		private void InspectRun()
		{
			_inspCtrl.InspParam.CopyFrom(InspectParam);
			_procSW.Reset();
			_procSW.Start();
			_inspCtrl.InspectRun(ImgGrab, enInspectItem.RULE_INSPECT);

		}

		private void InspectFin()
		{
			Invoke((MethodInvoker)delegate ()
			{
				_isProcFin = true;

				ResultData.Clear();
				int cnt = _inspCtrl.SetResultData(ResultData);
				if (checkDOut.Checked == false)
					OnResult?.Invoke();

				if (cnt == 0)       // 欠陥無し
				{
					double gray = GetImageGray();
					if (_grayData.Count >= Properties.Settings.Default.MeanGrayCount)
					{
						_grayData.RemoveAt(0);
					}
					_grayData.Add(gray);
				}

				if (checkRedrawImage.Checked || cnt > 0)
				{
					//ImgProc = _inspCtrl.ImgProc.CopyImage();
					if (checkDispProc.Checked)
					{
						if (ImgProc != null)
							ImgProc.Dispose();
						ImgProc = new HImage(_inspCtrl.ImgProc);
					}
					else
					{
						if (ImgDisp != null)
							ImgDisp.Dispose();
						ImgDisp = new HImage(ImgGrab);
					}

					InspectParam.InspAreaLeft = _inspCtrl.ImageProcArea;
					DispImage();
				}

				_procSW.Stop();
				long procTime = _procSW.ElapsedMilliseconds;
				if (checkRedrawImage.Checked)
					labelProcTime.Text = procTime.ToString();
			});
		}

		public void SetCamera()
		{
			try
			{
				int ch = 0;
				{
					//// チャンネル制御クラスを生成
					//AcapChannel channel = new AcapChannel(AcapCtrl, $"CH{ch + 1}");

					// ラムダ内で使うためにローカルへコピー（クロージャ対策）
					AcapCtrl.ImageGrabbed += (s, args) => OnImageGrabbed(args);
					//channel.LogMessage += (s, msg) => AddLog(msg);

					// オープン → INI 読込 → Update
					AcapCtrl.Open(SystemParam.CamInfo.IniFilename);
					//_channels[ch] = channel;
				}
			}
			catch (Exception ex)
			{
				logger.Error("オープン失敗: " + ex.Message);
			}
			finally
			{
			}
			//if (_grabCtrl.IsOpen)
			//{
			//	_grabCtrl.SetGrabHeight(_cameraCH,SystemParam.CamInfo.GrabHeight);
			//	_grabCtrl.SetCameraParam(_cameraCH, SystemParam.CamInfo.FrontCamera.Gain, SystemParam.CamInfo.FrontCamera.FrameRate, InspectParam.AutoExposureLeft.ExposureTime);
			//}
		}

		private void OnImageGrabbed(ImageGrabbedEventArgs e)
		{
			try
			{
				// UI スレッドで表示・保持
				BeginInvoke(new Action(() =>
				{
					_countFrame++;
					_frameRenew = true;
					if (CameraStatus == ProcState.NONE)
					{
						CameraStatus = ProcState.GRAB;
						//Helpers.WriteLog($"Line Camera : OnFrameEnd ");

						//ClassImageProc proc = new ClassImageProc();
						if (ImgGrab != null)
							ImgGrab.Dispose();
#if false
                    if (_grabSel == 0)
                    {
                        if (_imgGrab0 != null)
                            _imgGrab0.Dispose();
                        _imgGrab0 = _grabCtrl.ImgGrab.CopyImage();

                        proc.TiledImage(_imgGrab1, _imgGrab0, out _imgProc);
                        _grabSel = 1;

                    }
                    else
                    {
                        if (_imgGrab1 != null)
                            _imgGrab1.Dispose();
                        _imgGrab1 = _grabCtrl.ImgGrab.CopyImage();

                        proc.TiledImage(_imgGrab0, _imgGrab1, out _imgProc);
                        _grabSel = 0;
                    }
#else
						ImgGrab = e.Image.CopyImage();
#endif
						if (IsInspect && !checkInspectOff.Checked)
						{
							CameraStatus = ProcState.PROC;
							InspectRun();
							//Inspect();
						}
						else if (checkRedrawImage.Checked)
						{
							if (ImgDisp != null)
								ImgDisp.Dispose();
							ImgDisp = e.Image.CopyImage();
							DispImage();
						}
						CameraStatus = ProcState.NONE;
						//proc.Dispose();
					}
				}));
			}
			catch (Exception)
			{
				// フォーム終了中（ハンドル破棄後）など BeginInvoke できない場合
				e.Image.Dispose();
			}
		}



		public void CameraInit()
		{
			//_grabCtrl = new ClassAcapGrab();
			//if (SystemParam.CamInfo.Enable)
			//{
			//	_grabCtrl.CameraEnable = SystemParam.CamInfo.FrontCamera.Enable;
			//}
			//_grabCtrl.PortNo = CameraNo;
			//_grabCtrl.IniFilename = SystemParam.CamInfo.IniFilename;

			//if (_grabCtrl.CameraEnable == true)
			//{
			//	if (_grabCtrl.CamOpen(_cameraCH) == ClassAcapGrab.ErrorCode.Error)
			//	{
			//		string msg = "カメラ初期化異常 ( " + _grabCtrl.CameraID.ToString() + " )";
			//		MessageBox.Show(msg, "Initialize Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//		logger.Info(msg);
			//		//Helpers.WriteLog(msg);
			//	}
			//	else
			//	{
			//		_grabCtrl.OnFrame += OnFrameEnd;
			//		//SetCamera();
			//		//_grabCtrl.SetGrabHeight(SystemParam.CamInfo.GrabHeight);
			//		CameraSnap();

			//	}
			//}
			//#if DEBUG
			labelFR.Visible = true;
			timerFR.Start();
			//#endif
		}

		public void ChangeOperatorMode(bool mgrmode)
		{
			_mgrMode = mgrmode;
			contextMouseFront.Enabled = mgrmode;
			checkInspectOff.Enabled = mgrmode;
			checkDOut.Enabled = mgrmode;
			butTryInspect.Enabled = !IsInspect && mgrmode;
		}

		public void InspectStart(bool on)
		{
			IsInspect = on;
			if (IsInspect)
			{
				CameraGrabstop();
				System.Threading.Thread.Sleep(500);
				CameraGrabstart();

				contextMouseFront.Enabled = false;
				butTryInspect.Enabled = false;
			}
			else
			{
				CameraGrabstop();
				contextMouseFront.Enabled = _mgrMode;
				butTryInspect.Enabled = _mgrMode;
			}
			ChangeOperatorMode(_mgrMode);
		}


		//public async Task CameraSnap()
		public async void CameraSnap()
		{
			try
			{
				CameraGrabstop();
				// 2 チャンネルを並行して 1 フレーム取込
				Task<HImage> tasks;
				tasks = AcapCtrl.SnapAsync(SnapTimeoutMs);

				try
				{
					ImgGrab = await tasks;   // UI スレッドに戻って続行
					if (ImgDisp != null)
					{
						ImgDisp.Dispose();
					}
					ImgDisp = ImgGrab.CopyImage();
					DispImage();
				}
				catch (Exception ex)
				{
					logger.Error($"CH1: スナップ失敗: {ex.Message}");
				}
			}
			catch (Exception ex)
			{
				logger.Error($"スナップ失敗: {ex.Message}");
			}
			//if (_grabCtrl.IsOpen)
			//{
			//	_grabCtrl.CamSnapAsync();
			//	//Trace.WriteLine("Camera Snap ( " + InspID.ToString() + " )");
			//	logger.Info($"Line Camera Snap ");
			//}
			//else if (IsInspect)
			//{
			//	//    IsTrial = true;
			//	//    InspectProcStart();
			//}
		}

		public void CameraGrabstart()
		{
			try
			{
				AcapCtrl.StartContinuous();
			}
			catch (Exception ex)
			{
				logger.Error("取込開始失敗: " + ex.Message);
			}
			//if (_grabCtrl.IsOpen)
			//{
			//	//labelLoadFilename.Text = "";
			//	_grabCtrl.CamGrab(_cameraCH);
			//	menuMouseSnap.Enabled = false;
			//	menuMouseGrab.Enabled = false;
			//	menuMouseGrabstop.Enabled = true;
			//}
		}

		public void CameraGrabstop()
		{
			AcapCtrl.Stop();
			//if (_grabCtrl.IsOpen)
			//{
			//	_grabCtrl.CamGrabstop(_cameraCH);
			//	menuMouseSnap.Enabled = true;
			//	menuMouseGrab.Enabled = true;
			//	menuMouseGrabstop.Enabled = false;
			//}
		}

		private void hSmartWinMouseWheel(object sender, MouseEventArgs e)
		{
			Point pt = hCamImage.Location;
			MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
			hCamImage.HSmartWindowControl_MouseWheel(sender, newe);
			DispImage();
		}

		private void butTryInspect_Click(object sender, EventArgs e)
		{
			//if (ImgGrab != null)
			//	ImgGrab.Dispose();
			//ImgGrab = ImgDisp.CopyImage();
			InspectRun();
		}

		private void menuMouseIWriteImage_Click(object sender, EventArgs e)
		{
			SaveImage();
		}

		private void menuMouseSnap_Click(object sender, EventArgs e)
		{
			CameraSnap();
			//			_grabCtrl.CamSnap(_cameraCH);
		}

		private void menuMouseGrab_Click(object sender, EventArgs e)
		{
			CameraGrabstart();
			menuGrabItemEnabled(false);
			//_grabCtrl.CamGrab(_cameraCH);
		}

		private void menuGrabItemEnabled(bool on)
		{
			menuMouseSnap.Enabled = on;
			menuMouseGrab.Enabled = on;
			menuMouseGrabstop.Enabled = !on;
			menuMouseIReadImage.Enabled = on;
			menuMouseIWriteImage.Enabled = on;
			menuMouseProcExecute.Enabled = on;
		}

		private void menuMouseGrabstop_Click(object sender, EventArgs e)
		{
			CameraGrabstop();
			menuGrabItemEnabled(true);
		}

		private void timerFR_Tick(object sender, EventArgs e)
		{
			labelFR.Text = $"{_countFrame} f/s";
			_countFrame = 0;
		}

		private void hCamImage_HMouseMove(object sender, HMouseEventArgs e)
		{
			DispImage();
		}

		private void checkShowInspArea_CheckedChanged(object sender, EventArgs e)
		{
			DispImage();
		}

		private void checkShowResult_CheckedChanged(object sender, EventArgs e)
		{
			DispImage();
		}

		public double GetImageGray()
		{
			double gray = 0;

			if (!ImgDisp.IsInitialized())
				return 0;
			//ClassImageProc proc = new ClassImageProc();
			gray = _imgProc.GetGrayLevel(ImgDisp, InspectParam.InspAreaLeft);

			return gray;
		}

		public double GetMeanGray(bool autoEnable = false)
		{
			if (_grayData.Count == 0) return 0;
			double meanSum = 0;

			for (int i = 0; i < _grayData.Count; i++)
			{
				meanSum += _grayData[i];
			}
			double mean = meanSum / _grayData.Count;


			return mean;
		}


		private void menuMouseProcExecute_Click(object sender, EventArgs e)
		{
			if (ImgGrab != null)
				ImgGrab.Dispose();
			ImgGrab = ImgDisp.CopyImage();
			InspectRun();

		}

		private void checkDispProc_CheckedChanged(object sender, EventArgs e)
		{
			if (checkDispProc.Checked)
			{
				if (ImgProc != null)
				{
					ImgDisp.Dispose();
					ImgDisp = ImgProc.CopyImage();
					DispImage();
				}
			}
			else
			{
				ImgDisp.Dispose();
				ImgDisp = ImgGrab.CopyImage();
				DispImage();
			}
		}
	}
}
