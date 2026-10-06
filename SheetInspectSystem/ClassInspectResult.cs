using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using NLog;


namespace HFVisionInspect
{
    public enum ResultType
    {
        Data = 0,
        None = -1,
        InspectStart = -2,
        InspectStop = -3,
        CutterOn = -4,
        CutterOff = -5,
        PosWait = -6,
        PosInspect = -7,
    }
    public class ResultData
    {
        //public ResultType Type;
        public int No;
        public DateTime Date;
        public double BlobArea;
        public double BlobWidht;
        public double BlobLength;
        public double BlobPosX;
        public Rectangle BlobRectangle;
        public bool JudgeOK;
        public int JudgeLevel;
        public Rectangle RegionRect;
        public HImage BlobImage;
        public HImage BlobProcImage;

        public ResultData()
        {
            //Type = ResultType.None;
            No = -1;
            BlobArea = 0;
            BlobWidht = 0;
            BlobLength = 0;
            BlobPosX = 0;
			BlobRectangle = new Rectangle(0, 0, 10, 10);
			JudgeOK = false;
            JudgeLevel = 0;
            RegionRect = new Rectangle(0, 0, 10, 10);
            BlobImage = new HImage();
            BlobImage.GenEmptyObj();
            BlobProcImage = new HImage();
            BlobProcImage.GenEmptyObj();
        }

        public void CopyFrom(ResultData src)
        {
            //Type = src.Type;
            No = src.No;
            Date = src.Date;
            BlobArea = src.BlobArea;
            BlobWidht = src.BlobWidht;
            BlobLength = src.BlobLength;
            BlobPosX = src.BlobPosX;
			BlobRectangle = src.BlobRectangle;
			JudgeOK = src.JudgeOK;
            JudgeLevel = src.JudgeLevel;
            RegionRect = src.RegionRect;
            if (BlobImage != null)
                BlobImage.Dispose();
            BlobImage = src.BlobImage.CopyImage();
            if (BlobProcImage != null)
                BlobProcImage.Dispose();
            BlobProcImage = src.BlobProcImage.CopyImage();
        }

        public virtual void Dispose()
        {
            BlobImage.Dispose();
            BlobProcImage.Dispose();
        }
    }

    public class InspectResultData
    {
		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		
        const string ImageFolder = "\\Img";
        public List<ResultData> InspResultData;
        public double CameraResolution;
        public int RecordWidth;
        public int RecoedHeight;
        public int RegistPosMargin;
        public int RegistTimeMargin;

        private int _dataNo;

        public InspectResultData()
        {
            InspResultData = new List<ResultData>();
            RecordWidth = 512;
            RecoedHeight = 512;
            RegistPosMargin = 50;
            RegistTimeMargin = 2;

			_dataNo = 0;
        }

        public void Clear()
        {
            InspResultData.Clear();
            _dataNo = 0;
        }

        public int GetCount()
        {
            return InspResultData.Count;
        }

        public ResultData SetStart()
        {
            ResultData regData = new ResultData();
            regData.No = (int)ResultType.InspectStart;
            regData.Date = DateTime.Now;
            InspResultData.Add(regData);

            return regData;
        }

        public ResultData SetStop()
        {
            ResultData regData = new ResultData();
            regData.No = (int)ResultType.InspectStop;
            regData.Date = DateTime.Now;
            InspResultData.Add(regData);

            return regData;
        }

		public ResultData SetPosWait()
		{
			ResultData regData = new ResultData();
			regData.No = (int)ResultType.PosWait;
			regData.Date = DateTime.Now;
			InspResultData.Add(regData);

			return regData;
		}

		public ResultData SetPosInspect()
		{
			ResultData regData = new ResultData();
			regData.No = (int)ResultType.PosInspect;
			regData.Date = DateTime.Now;
			InspResultData.Add(regData);

			return regData;
		}


		public ResultData SetCutter(bool on)
        {
            ResultData regData = new ResultData();
            if (on)
                regData.No = (int)ResultType.CutterOn;
            else
                regData.No = (int)ResultType.CutterOff;
            regData.Date = DateTime.Now;
            InspResultData.Add(regData);

            return regData;
        }

        public int SetData(ResultInfo resutlData, int BasePosX)
        {
            int ret = -1;
            try
            {
                BlobInfo blob = resutlData.BlobData;
                double res = CameraResolution; 
                ResultData regData = new ResultData();
                regData.No = _dataNo;
                //regData.Type = ResultType.Data;
                regData.Date = DateTime.Now;
                regData.BlobArea = blob.Area * res * res;
                regData.BlobWidht = blob.Width * res;
                regData.BlobLength = blob.Height * res;
                regData.BlobPosX = blob.X * res + BasePosX;

				regData.JudgeLevel = blob.JudgeNo + 1;
                regData.JudgeOK = resutlData.JudgeOK;

                int left, right, top, bottom;

				left = (int)blob.Left;
                right = (int)blob.Right;
                top = (int)blob.Top;
                bottom = (int)blob.Botttom;
				regData.BlobRectangle = new Rectangle(left, top, right - left+1, bottom-top+1);

				int w, h;
				resutlData.ImgSrc.GetImageSize(out w, out h);
				left = (int)blob.X - RecordWidth / 2;
                right = (int)blob.X + RecordWidth / 2 - 1;
                if (left < 0)
                {
                    left = 0;
                    right = RecordWidth - 1;
                }
                if (right >= w)
                {
                    right = w - 1;
                    left = w - RecordWidth;
                }
                top = (int)blob.Y - (RecoedHeight / 2);
                bottom = (int)blob.Y + (RecoedHeight / 2);
                if (top < 0)
                {
                    top = 0;
                    bottom = RecoedHeight - 1;
                }
                if (bottom >= h)
                {
                    bottom = h - 1;
                    top = h - RecoedHeight;
                }
                regData.RegionRect = new Rectangle(left, top, RecordWidth, RecoedHeight);
                HImage imgCrip = new HImage();
                imgCrip = resutlData.ImgSrc.CropRectangle1(top, left, bottom, right);
                regData.BlobImage = imgCrip.CopyImage();
                imgCrip.Dispose();
                imgCrip = resutlData.ImgBinary.CropRectangle1(top, left, bottom, right);
                regData.BlobProcImage = imgCrip.CopyImage();

                imgCrip.Dispose();
                InspResultData.Add(regData);

                ret = InspResultData.Count - 1;
                _dataNo++;
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine("GaussFilter Error : " + oe.Message);
            }

            return ret;
        }

        public ResultData GetData(int no)
        {
            ResultData data = new ResultData();
            if (no >= 0 && no < InspResultData.Count)
            {
                data.CopyFrom(InspResultData[no]);
                return data;
            }
            else
            {
                data.No = -1;
                return data;
            }
        }

		public bool CheckResultData(ResultInfo resutlData, int BasePosX)
		{
			//int checkCnt = 10;
			int posMargin = RegistPosMargin;
            int timeMargin = RegistTimeMargin;

			BlobInfo blob = resutlData.BlobData;
			double res = CameraResolution;
			int cntAll = GetCount();
			ResultData data;
			for (int st = cntAll - 10; st < cntAll; st++)
			{
                if (st < 0) continue;
				data = GetData(st);
				if (Math.Abs(data.Date.Second - DateTime.Now.Second) < timeMargin)
                {
					double blobPosX = blob.X * res + BasePosX;
					if (Math.Abs(blobPosX- data.BlobPosX) <= posMargin )
                    {
                        return false;
                    }
                }
			}

			return true;
		}


		public bool SaveData(string path)
        {
            bool ret = false;
            try
            {
                DateTime NowDate = DateTime.Now;
                string writeFolder = path + "\\" + NowDate.ToString("yyyyMMdd-HHmmss");
                string imgFolder = writeFolder + ImageFolder;
                if (!System.IO.File.Exists(writeFolder))
                {
                    System.IO.Directory.CreateDirectory(writeFolder);
                    System.IO.Directory.CreateDirectory(imgFolder);
                }
                else
                {
                    return false;
                }
                string csvFilename = writeFolder + "\\" + NowDate.ToString("検出結果_HHmmss") + ".csv";
                bool Append;
                string str;
                if (System.IO.File.Exists(csvFilename))
                {
                    Append = true;     //　追記(true)
                }
                else
                {
                    Append = false;     // 新規(false) ファイル
                                        //					DeleteFileDate(LogPath, 30);        // 30日を超えた分のみ削除
                }
                using (var sw = new StreamWriter(csvFilename, Append, System.Text.Encoding.GetEncoding("sjis")))
                {
                    if (Append == false)
                    {
                        str = "No,日付,時刻,面積,幅,長さ,位置,判定,レベル";
                        sw.WriteLine(str);
                    }
                    string dataText = "";
                    ResultData data = new ResultData();
                    string imgName = "";
                    for (int i = 0;  i < InspResultData.Count; i++)
                    {
                        data = InspResultData[i];
                        dataText = $"{data.No}, { data.Date.ToLongDateString()}, {data.Date.ToLongTimeString()},";
                        dataText += $"{data.BlobArea}, { data.BlobWidht}, {data.BlobLength}, {data.BlobPosX},{data.JudgeOK},{data.JudgeLevel}";
                        sw.WriteLine(dataText);

                        if (data.No >= 0)
                        {
                            imgName = imgFolder + $"\\Defect_{data.No}.bmp";
                            data.BlobImage.WriteImage("bmp", 0, imgName);
                            imgName = imgFolder + $"\\Binary_{data.No}.bmp";
                            data.BlobProcImage.WriteImage("bmp", 0, imgName);
                        }
                    }
                }

				if (!System.IO.File.Exists(csvFilename))
				{
					logger.Info($"履歴データ保存 失敗:{csvFilename}");
				}
                else
                {
					logger.Info($"履歴データ保存:{csvFilename}");
                    ret = true;
				}

			}
			catch (HOperatorException oe)
            {
                Trace.WriteLine("SaveData Halcon Error : " + oe.Message);
				logger.Info($"履歴データ保存 失敗:");
			}
			catch (System.Exception e)
            {
                Trace.WriteLine("SaveData Error : " + e.Message);
				logger.Info($"履歴データ保存 失敗:");
			}

			return ret;
        }

        public bool LoadData(string path , string csvName)
        {

            try
            {
                string csvFilename = path + "\\" + csvName;
                string str;
                using (var sw = new StreamReader(csvFilename, System.Text.Encoding.GetEncoding("sjis")))
                {
                    Clear();
                    string imgName = "";
                    string line = sw.ReadLine();        // ヘッダ部を取込
                    string[] hedder = line.Split(',');

                    string imgFolder = path + ImageFolder;
                    while (!sw.EndOfStream)
                    {
                        line = sw.ReadLine();
                        string[] values = line.Split(',');

                        ResultData regData = new ResultData();
                        //regData.Type = ResultType.Data;
                        regData.No = int.Parse(values[0]);
                        str = values[1] + values[2];
                        regData.Date = DateTime.Parse(str);
                        regData.BlobArea = double.Parse(values[3]);
                        regData.BlobWidht = double.Parse(values[4]);
                        regData.BlobLength = double.Parse(values[5]);
                        regData.BlobPosX = double.Parse(values[6]);
                        regData.JudgeOK = bool.Parse(values[7]);
                        regData.JudgeLevel = int.Parse(values[8]);

                        if (regData.No >= 0)
                        {
                            imgName = imgFolder + $"\\Defect_{regData.No}.bmp";
                            regData.BlobImage.ReadImage(imgName);
                            imgName = imgFolder + $"\\Binary_{regData.No}.bmp";
                            regData.BlobProcImage.ReadImage(imgName);
                        }

                        InspResultData.Add(regData);
                        _dataNo++;
                    }
                }
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine("SaveData Halcon Error : " + oe.Message);
                return false;
            }
            catch (System.Exception e)
            {
                Trace.WriteLine("SaveData Error : " + e.Message);
                return false;
            }
            return true;
        }
    }

}
