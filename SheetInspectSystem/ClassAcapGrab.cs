#define HALCON_MEMORY_DIRECT_DMA      //Halconで確保したバッファに直接DMA

using Avaldata;
using Avaldata.AcapLibDotNet;
using Avaldata.AcapLibDotNet.Parameters;
using Avaldata.AcapLibDotNet.Parameters.Id;
using HalconDotNet;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HFVisionInspect
{
    // コールバック関数
    public delegate void AcapCallBackProc(int nCH, uint ulEvent, int nFrame, int nIndex);

    public class GValue
    {
        public int ghDevice = (-1);                 //デバイスハンドル
        public int gnWidth;                         //Xサイズ
        public int gnHeight;                        //Yサイズ
        public int gnBoardBit;                      //ボードビット幅
        public int gnCameraBit;                     //カメラビット幅

        public int gnBitCount;						//表示用ビットカウント
        public int gnPixShift;						//ビットシフト
        public int gnHighClip;                      //ハイクリップ
        public int gnIndex;							//インデックス
        public int gnMemNum;                        //内部バッファ面数

        public int nCH;

		public bool gbGrabFlag;                     //Grabフラグ
        public IntPtr gpBuffPointer;                //バッファポインタ

        public GCHandle gcHandle;					//GCHandle

        public IntPtr FuncPtr;                      //関数ポインタ
        public AcapCallBackProc Proc;               //プロシージャ
        public String gsIniFileName;                //Iniファイルネーム  

        public uint serialBPS = 9600;
        public uint serialBuffer = 32768;
    }


    class ClassAcapGrab
    {
        const int SERIAL_BAUDRATE = 115200;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int PortNo { get; set; }

        public int CameraImageWidth { get; } = 1936;
        public int CameraImageHeight { get; } = 1216;
        ///public HImage HoGrabImg;
        public HImage ImgGrab { get; set; } = new HImage();
        public HImage[] hi_Image;
        public HTuple[] ht_Pointer;

		public CDeviceList DeviceList;
		public IAcapLibDotNet[] IAcap;
		public enum CaptureStatus
		{
			Idle = 0,
			Snap = 1,
			Streaming = 2,
		}
		CaptureStatus _CaptureStatus;
		// オープン処理がおこなわれたかどうか
		public bool OpenFlag = false;

		public string CameraID { set; get; } = "";
        public string DeviceName { set; get; } = "Linea";
        public string IniFilename { set; get; }
        public bool CameraEnable { set; get; } = false;
        public bool IsOpen { set; get; } = false;
        private GrabMode IsGrab = GrabMode.Stop;
        public int ImageWidth { get; set; } = 8192;
        public int ImageHeight { get; set; } = 2000;

        public GValue gv = new GValue();

        public enum ErrorCode
        {
            Success = 0,
            Error,
            TimeOut,
        }

        enum GrabMode
        {
            Stop = 0,
            Grab,
            Snap,
        }

        public Action OnFrame;
        //private Task<ErrorCode> grabTask;

        public ClassAcapGrab()
        {
			// デバイスリストの取得
			DeviceList = new CDeviceList();
			// デバイスチャンネルの配列を取得
			IAcap = DeviceList.GetDeviceChannel();
			//IniFilename = "C:/AVALDATA/SDK-AcapLib2/Sample/AIPTool/Board[APX-3324]No[0]Ch[1].ini";
		}

		public ErrorCode CamOpen(int ch = 0, string iniFile = "")
        {
            try
            {
                int ich = IAcap.Length;

				// 1チャンネルも見つからなければエラー終了
				uint ChannelNum = DeviceList.GetDeviceChannelNum();
				if (ChannelNum == 0 || ch >= ChannelNum)
				{
					MessageBox.Show("No device channel found.");
					this.CamClose(ch);
					return ErrorCode.Error;
				}

				// 最初に見つかったデバイスチャンネルを使用(オープン)する
				int rtn = IAcap[ch].Open();
				if (rtn != Avaldata.AcapLibDotNet.Error.StatusID.AVAL_STATUS_SUCCESS)
				{
					this.CamClose(ch);
					return ErrorCode.Error;
				}

                //イニシャルファイル選択
                rtn = IAcap[ch].LoadFile(iniFile);
				if (rtn != Avaldata.AcapLibDotNet.Error.StatusID.AVAL_STATUS_SUCCESS)
				{
					return ErrorCode.Error;
                }

                rtn = IAcap[ch].Update();

				DEV_CH_INFO DevChInfo;
				IAcap[ch].GetDeviceChannelInfo(out DevChInfo);

				// コールバック登録
				IAcap[ch].FrameEndEvent += new IAcapLibDotNet.FrameEndHandler(this.FrameEndEventHandler);

                //サイズ情報取得
                uint w = 0, h = 0, bit, memNum;
                if (IAcap[ch].GetParameter(EnumParameterID.GrabberIn_Width, out w) != 0)
                {
                    return ErrorCode.Error;
                }
				if (IAcap[ch].GetParameter(EnumParameterID.GrabberIn_Height, out h) != 0)
				{
						return ErrorCode.Error;
                }
				gv.gnWidth = (int)w;
				gv.gnHeight = (int)h;
                if (IAcap[ch].GetParameter(EnumParameterID.CameraOut_BitCount, out bit) != 0)
                {
                    return ErrorCode.Error;
                }
				gv.gnBoardBit = (int)bit;
				gv.gnCameraBit = (int)bit;
                //if (AcapLib2.AcapGetInfo(gv.ghDevice, gv.nCH, AcapLib2.COMMON.ACL_CAM_BIT, 0, ref gv.gnCameraBit) == 0)
                //{
                //    return ErrorCode.Error;
                //}
                //if (AcapLib2.AcapGetInfo(gv.ghDevice, gv.nCH, AcapLib2.COMMON.ACL_PIX_SHIFT, 0, ref gv.gnPixShift) == 0)
                //{
                //    return ErrorCode.Error;
                //}
                //if (AcapLib2.AcapGetInfo(gv.ghDevice, gv.nCH, AcapLib2.COMMON.ACL_HIGH_CLIP, 0, ref gv.gnHighClip) == 0)
                //{
                //    //ハイクリップ未対応機種
                //    gv.gnHighClip = 0;
                //}
                if (IAcap[ch].GetBufferNum(out memNum) != 0)
                {
                    return ErrorCode.Error;
                }
				gv.gnMemNum = (int)memNum;

				//アドレス取得
				gv.gpBuffPointer = IAcap[ch].GetBufferAddress(0);

                //// Local control variables 
                HTuple hv_Type = null;
                HTuple hv_w = null;
                HTuple hv_h = null;

                // Local iconic variables 
                HOperatorSet.SetSystem("width", gv.gnWidth);
                HOperatorSet.SetSystem("height", gv.gnHeight);

                // Initialize local and output iconic variables 
                hi_Image = new HImage[gv.gnMemNum];
                ht_Pointer = new HTuple[gv.gnMemNum];


                //Halconのバッファは１つだけ確保
                if (gv.gnBoardBit == 8)
                {
                    hi_Image[ch] = new HImage("byte", (int)gv.gnWidth, gv.gnHeight);
                }
                else if (gv.gnBoardBit == 16)
                {
                    hi_Image[ch] = new HImage("int2", (int)gv.gnWidth, gv.gnHeight);
                }
                else if ((gv.gnBoardBit == 24) || (gv.gnBoardBit == 32))
                {
                    hi_Image[ch] = new HImage("byte", (int)gv.gnWidth * gv.gnBoardBit / 8, gv.gnHeight);
                }

                //先頭アドレスの取得
                ht_Pointer[ch] = hi_Image[0].GetImagePointer1(out hv_Type, out hv_w, out hv_h);

                gv.gbGrabFlag = false;

                ImageWidth = (int)gv.gnWidth;
                ImageHeight = gv.gnHeight;
                string strtemp = string.Format("CameraSize ( {0}, {1} )", ImageWidth.ToString(), ImageHeight.ToString());
                logger.Trace(strtemp);

                IsOpen = true;
                //grabTask = Task<ErrorCode>.Factory.StartNew(GrabTaskAsync);

                if (ImgGrab != null)
                    ImgGrab.Dispose();
                ImgGrab.GenEmptyObj();

				// 画像入力状態の初期化
				_CaptureStatus = CaptureStatus.Idle;

			}
			catch (HOperatorException oe)
            {
                logger.Trace(oe.Message);
                return ErrorCode.Error;
            }

            return ErrorCode.Success;
        }

        private ErrorCode CamBufferSet(int ch, int height)
        {
            if (height > 0)
            {
                if (IAcap[ch].SetParameter(EnumParameterID.GrabberIn_Height, (uint)height) != 0)
                {
                    return ErrorCode.Error;
                }
            }

			if (IAcap[ch].GetParameter(EnumParameterID.GrabberIn_Height, out uint h) != 0)
			{
				return ErrorCode.Error;
            }

            if (gv.gnBoardBit == 8)
            {
                //8 bit
                for (int i = 0; i < gv.gnMemNum; i++)
                {
                    if (hi_Image[i] != null)
                        hi_Image[i].Dispose();
                    hi_Image[i] = new HImage("byte", gv.gnWidth, gv.gnHeight);
                }
            }
            else if (gv.gnBoardBit == 16)
            {
                //10,12,14,16 bit
                for (int i = 0; i < gv.gnMemNum; i++)
                {
                    if (hi_Image[i] != null)
                        hi_Image[i].Dispose();
                    hi_Image[i] = new HImage("int2", gv.gnWidth, gv.gnHeight);
                }
            }
            else if ((gv.gnBoardBit == 24) || (gv.gnBoardBit == 32))
            {
                for (int i = 0; i < gv.gnMemNum; i++)
                {
                    if (hi_Image[i] != null)
                        hi_Image[i].Dispose();
                    hi_Image[i] = new HImage("byte", gv.gnWidth * gv.gnBoardBit / 8, gv.gnHeight);
                }
            }

            // Local control variables 
            HTuple hv_Type = null;
            HTuple hv_w = null;
            HTuple hv_h = null;
            for (uint i = 0; i < gv.gnMemNum; i++)
            {
                //Halconで確保したバッファの先頭アドレスの取得
                ht_Pointer[i] = hi_Image[i].GetImagePointer1(out hv_Type, out hv_w, out hv_h);

                // 転送先の仮設定
                if (IAcap[ch].SetBufferAddress(i, (IntPtr)ht_Pointer[i].L, (uint)hv_w, (uint)hv_h, (uint)gv.gnBitCount) != 0)
                {
                    return ErrorCode.Error;
                }
            }

            // 再初期化
            //if (AcapLib2.AcapReflectParam(gv.ghDevice, gv.nCH) == 0)
            //{
            //    return ErrorCode.Error;
            //}

            return ErrorCode.Success;
        }

        public void CamClose(int ch)
        {
            try
            {
				// 画像入力完了(FrameEnd)イベントを解除
				if (OpenFlag == true)
				{
					IAcap[ch].FrameEndEvent -= new IAcapLibDotNet.FrameEndHandler(this.FrameEndEventHandler);

					// デバイスチャンネルをクローズする
					int rtn = IAcap[ch].Close();
					OpenFlag = false;
				}
			}
            catch (HOperatorException oe)
            {
                logger.Trace(oe.Message);
            }
        }

		//////////////////////////////////////////////////////////////////////////////////////////
		//コールバック関数
		private void FrameEndEventHandler(object sender, Avaldata.AcapLibDotNet.FrameEndEventArgs e)
		{
			// Snapだったら画像入力を停止する
			if (_CaptureStatus == CaptureStatus.Snap)
			{
				IAcap[0].StopAcquisition();
				_CaptureStatus = CaptureStatus.Idle;
			}
			// Streamingだったら描画処理の遅延を発生させる(負荷対策)
			if (_CaptureStatus == CaptureStatus.Streaming)
			{
				System.Threading.Thread.Sleep(10);
				System.Windows.Forms.Application.DoEvents();
			}
		}

        public void CamReset()
        {
            //CamClose();
            //System.Threading.Thread.Sleep(1000);
            //CamOpen();
        }

        public void SetGrabHeight(int ch, int height)
        {
            try
            {
                if (IsOpen)
                {
                    //Trace.WriteLine("Set Image Height ( " + height.ToString() + " )");
                    logger.Trace("Set Image Height ( " + height.ToString() + " )");
                    //Grabber.SetFramegrabberParam("Height", height);
                    CamBufferSet(ch, height);
                }
            }
            catch (HOperatorException oe)
            {
                logger.Trace(oe.Message);
            }
        }

        /// <summary>
        /// 露光時間の設定[μsec]
        /// </summary>
        /// <param name="time"></param>
        public void SetExposureTime(int ch, int time)
        {
            try
            {
                if (IsOpen)
                {
                    string cmd = $"exposure {time}";
                    //Trace.WriteLine($"Set ExposureTime ( {cmd} )");
                    logger.Trace($"Set ExposureTime ( {cmd} )");
                    //Grabber.SetFramegrabberParam("gain", gain);
                    SerialOpen(ch);
                    SerialCommandSend(ch, cmd);
					Thread.Sleep(100);
					SerialClose(ch);

                }
            }
            catch (HOperatorException oe)
            {
                //Trace.WriteLine(oe.Message);
                logger.Trace(oe.Message);
            }
        }

        public void SetGain(int ch, int gain)
        {
            try
            {
                if (IsOpen)
                {
                    string cmd = $"gain {gain}";
                    //Trace.WriteLine($"Set Gain ( {cmd} )");
                    logger.Trace("Set Gain ( " + gain.ToString() + " )");
                    //Grabber.SetFramegrabberParam("gain", gain);
                    SerialOpen(ch);
                    SerialCommandSend(ch, cmd);
					Thread.Sleep(100);
					SerialClose(ch);
                }
            }
            catch (HOperatorException oe)
            {
                //Trace.WriteLine(oe.Message);
                logger.Trace(oe.Message);
            }
        }

        public void SetLineRate(int ch, int frameRate)
        {
            //Trace.WriteLine("Set LineRate ( " + frameRate.ToString() + " )");
            try
            {
                if (IsOpen)
                {
                    string cmd = $"linerate {frameRate}";
                    //Trace.WriteLine($"Set LineRate ( {cmd} )");
                    logger.Trace($"Set LineRate ( {cmd} )");
                    //Grabber.SetFramegrabberParam("AcquisitionLineRate", frameRate);
                    SerialOpen(ch);
                    SerialCommandSend(ch, cmd);
					Thread.Sleep(100);
					SerialClose(ch);
                }
            }
            catch (HOperatorException oe)
            {
                //Trace.WriteLine(oe.Message);
                logger.Trace(oe.Message);
            }
        }

		public void SetLineReverse(int ch, bool on)
		{
			//Trace.WriteLine($"Set ScanReverse ( {on} )");
			try
			{
				if (IsOpen)
				{
					string cmd = "flip 0";
                    if (on)
                    {
						cmd = "flip 1";
					}
					//Trace.WriteLine($"Set ScanReverse ( {on} )");
					logger.Trace($"Set ScanReverse ( {on} )");
					SerialOpen(ch);
					SerialCommandSend(ch, cmd);
					Thread.Sleep(100);
					SerialClose(ch);
				}
			}
			catch (HOperatorException oe)
			{
				logger.Trace(oe.Message);
			}
		}

		public void SetTestPattern(int ch, int mode)
		{
			try
			{
				if (IsOpen)
				{
					string cmd = $"tpmode {mode}";
					logger.Trace($"Set TestPattern ( {mode} )");
					SerialOpen(ch);
					SerialCommandSend(ch, cmd);
					Thread.Sleep(100);
					SerialClose(ch);
				}
			}
			catch (HOperatorException oe)
			{
				logger.Trace(oe.Message);
			}
		}

		public void SetCameraParam(int ch, double gain, int frameRate, int ExpTime)
        {
            try
            {
                if (IsOpen)
                {
                    SerialOpen(ch);
                    string cmd = $"gain {gain.ToString("0.00")}";
                    SerialCommandSend(ch, cmd);
                    Thread.Sleep(100);

                    cmd = $"linerate {frameRate}";
                    SerialCommandSend(ch, cmd);
					Thread.Sleep(100);

					cmd = $"exposure {ExpTime}";
                    SerialCommandSend(ch, cmd);
					Thread.Sleep(100);

					SerialClose(ch);

                    string str = $"Set Gain ( {gain} )  linerate ({frameRate})   ExposureTime ( {ExpTime} )";
                    //Trace.WriteLine(str);
                    logger.Info(str);
                }
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
                logger.Info(oe.Message);
            }

        }

        public void CamSnap(int ch)
        {
            try
            {
                if (gv.gbGrabFlag == false)
                {
                    // 入力開始
                    if (IAcap[ch].StartAcquisition(0, 1) == 0)
                    {
                        Trace.WriteLine("AcapGrabStart");
                    }

                    // 入力停止
                    if (IAcap[ch].StopAcquisition() == 0)
                    {
                        Trace.WriteLine("AcapGrabStop");
                    }
                }
                else
                {
                    // 入力停止
                    if (IAcap[ch].StopAcquisition() == 0)
                    {
                        Trace.WriteLine("AcapGrabStop");
                    }
                    gv.gbGrabFlag = false;
                }
            }
            catch (HOperatorException oe)
            {
                logger.Trace(oe.Message);
            }

        }

        public void CamSnapAsync()
        {
            try
            {
                if (IsGrab != GrabMode.Stop)
                    return;
                IsGrab = GrabMode.Snap;
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
            }
        }

        private ErrorCode GrabTaskAsync()
        {
            logger.Info("LineCamera Grab Task Start ( " + CameraID + " ) ...");

            //Grabber.GrabImageStart(-1);
            while (IsOpen)
            {
                if (IsGrab == GrabMode.Stop)
                {
                    System.Threading.Thread.Sleep(100);        // 10 -> 2 20191206変更
                    continue;
                }
                try
                {
                    while (IsGrab != GrabMode.Stop)
                    {
                        if (ImgGrab != null)
                            ImgGrab.Dispose();
                        //ImgGrab = Grabber.GrabImageAsync(-1);
                        //Trace.WriteLine("Grab ...");
                        //OnFrame?.Invoke();
                        if (IsGrab == GrabMode.Snap)
                        {
                            IsGrab = GrabMode.Stop;
                            //							break;
                        }
                    }
                }
                catch (HOperatorException oe)
                {
                    Trace.WriteLine($"GrabTaskAsync Exception : {oe.Message}");
                    logger.Info(oe.Message);
                    //return ErrorCode.Error;
                }
                //Grabber.SetFramegrabberParam("do_abort_grab", "true");
            }
            logger.Info("LineCamera Grab Task End (" + CameraID + ")");

            return ErrorCode.Success;
        }
        private HTuple GrabFrameEnd()
        {
            return 0;
        }

        public void CamGrab(int ch)
        {
            if (gv.gbGrabFlag == false)
            {
                if (IAcap[ch].StartAcquisition(0, 0) == 0)
                {
                    Trace.WriteLine("AcapGrabStart");
                }
                gv.gbGrabFlag = true;
            }
        }

        public void CamGrabstop(int ch)
        {
            if (IAcap[ch].StopAcquisition() == 0)
            {
                Trace.WriteLine("AcapGrabStop");
            }
            gv.gbGrabFlag = false;
        }

        private void SerialOpen(int ch)
        {
            uint bps = 9600;
            if (IAcap[ch].SerialOpen(gv.serialBPS, gv.serialBuffer) != 0)
            {
                logger.Trace("AcapSerialOpen Error");
                return;
            }
        }

        private void SerialCommandSend(int ch, string cmd)
        {
            try
            {
				byte[] sendData = null;
				uint sendSize;

				sendData = Encoding.UTF8.GetBytes(cmd);
				sendSize = (uint)sendData.Length;

				logger.Trace($"SerialCommand (CH{gv.nCH}) : {cmd}");
                //Trace.WriteLine(cmd);
                if (IAcap[ch].SerialSend(sendData, ref sendSize, 1000) != 0)
                {
                    Trace.WriteLine("AcapSerialWrite Error");
                    return;
                }

				byte[] receiveData = new byte[256];
				uint receiveSize = 256;

				for (int i = 0; i < 256; i++)
					receiveData[i] = 0;

				if (IAcap[ch].SerialReceive(receiveData, ref receiveSize, 100) != 0)
				{
                    Trace.WriteLine("AcapSerialWrite Error");
                    return;
                }
                else
                {
//                    Trace.WriteLine(recv.ToString());
                }
            }
            catch (SystemException oe)
            {
                //logger.Trace(oe.Message);
                logger.Trace(oe.Message);
            }
        }

		private void SerialClose(int ch)
        {
            if (IAcap[ch].SerialClose() != 0)
            {
                logger.Trace("AcapSerialClose Error");
            }
        }
    }
}
