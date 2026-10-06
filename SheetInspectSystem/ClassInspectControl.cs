using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace HFVisionInspect
{
    public enum enInspectItem
    {
//        INSPECT_ALL = 0,
        RULE_INSPECT = 0,
        DEEP_LARNING,

        BLACK_BLOB,
//        WHITE_BLOB,
//        SHAPE_BLOB,
//        PARTICLE_BLOB,
    }

    public class InspectControl
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private ClassImageProc _imageProc;
        private enInspectItem _itemMode;

        public ViewID ControlID { set; get; }

        public InspectParameter InspParam { set; get; }
        public List<BlobInfo> DotInspResult { get; set; }

        public int TotalBlobCount { get; set; }

        public Action OnProcEnd;
        private Task<ErrorCode> procTask;
        private bool isTaskRun = false;
        private bool inspectRun = false;

        public HImage ImgSrc { get; set; }
        public HImage ImgProc { get { return _imgProc; } }
        private HImage _imgProc;
		public  HImage ImgDiffBase { get; set; }
        public Rectangle ImageProcArea { get { return _imageProc.ImageProcArea; } }
        enInspectItem _Inspectitem;

		public  RuleRecipe _ruleRecipe;
        private HollowFiberRuleInspector _ruleInspector;

		public InspectControl()
        {
            ControlID = ViewID.ViewFront;
            InspParam = new InspectParameter();
            _imageProc = new ClassImageProc();
            DotInspResult = new List<BlobInfo>();

            ImgSrc = new HImage();
           _imgProc = new HImage();
			ImgDiffBase = new HImage();

			_ruleRecipe = new RuleRecipe();
            _ruleInspector = new HollowFiberRuleInspector(_ruleRecipe);    
		}

        public void InspectTaskRun()
        {
            isTaskRun = true;
            procTask = Task<ErrorCode>.Factory.StartNew(procTaskAsync);
        }
        private ErrorCode procTaskAsync()
        {
            logger.Info($"Inspect control Task Start : {ControlID.ToString()}...");
            Trace.WriteLine($"Inspect control Task Start : {ControlID.ToString()}...");

            while (isTaskRun) 
            {
                if (inspectRun == false)
                {
                    System.Threading.Thread.Sleep(2);
                    continue;
                }
                try
                {
                    // 検査処理
					BlobInspection(ImgSrc, _Inspectitem);
					OnProcEnd?.Invoke();
					inspectRun = false;
					//Trace.WriteLine($"InspectProc Run Fin : {ControlID}...");
				}
				catch (HOperatorException oe)
                {
                    Trace.WriteLine($"InspectRun Exception : {oe.Message}");
                    logger.Info(oe.Message);
                    //return ErrorCode.Error;
                }
            }
            logger.Info("Inspect control Task End (" + ControlID.ToString() + ")");
            Trace.WriteLine($"Inspect control Task End : {ControlID.ToString()}...");

            return 0;
        }

        public virtual void Dispose()
        {
            ImgSrc.Dispose();
            ImgProc.Dispose();
			_imgProc.Dispose();
			ImgDiffBase.Dispose();
			isTaskRun = false;
            BlobDataClear();
        }

        private void BlobDataClear()
        {
            DotInspResult.Clear();
            TotalBlobCount = 0;
        }

        public ErrorCode InspectRun(HImage imgSrc, enInspectItem item)
        {
            //if (inspectRun == true)
            //    return ErrorCode.OtherError;
            if (ImgSrc != null)
                ImgSrc.Dispose();
            if (ImgProc != null)
                _imgProc.Dispose();
            ImgSrc = imgSrc.CopyImage();
            _Inspectitem = item;
            inspectRun = true;
            //ace.WriteLine($"InspectProc Run : {ControlID}...");

            return ErrorCode.None;
        }

        public ErrorCode BlobInspection(HImage imgSrc, enInspectItem item)
        {
            ErrorCode ret = ErrorCode.None;

            _imageProc.InspectReset(imgSrc, ImgDiffBase);
            //_imageProc.ImageConvert();
            _itemMode = item;
            BlobDataClear();

            if (item == enInspectItem.BLACK_BLOB)
            {
                Rectangle area = new Rectangle();
				PointDatas edgedata;
				if (ControlID == ViewID.ViewRear)
                {
					area = InspParam.InspAreaRight;
					if (InspParam.EdgeParamRight.IsEnabled)
                    {
                        ret = ClassEdgeSearch.MeasureBoxEdge(imgSrc, InspParam.EdgeParamRight.EdgeSearchParam, out edgedata);
                        if (ret == ErrorCode.Success && edgedata.pnts.Count > 0)
                        {
							edgedata.CalcMean();
							//int right = area.Right;
							//area.X = (int)edgedata.meanX + InspParam.EdgeParamCam0.AreaOffset;
							//area.Width = right - area.X;
                            //                            InspParam.InspAreaCam0 = area;
                            int right = (int)edgedata.meanX - InspParam.EdgeParamRight.AreaOffset;
                            area.Width = (int)right - area.X;
                        }
                        else
							logger.Info($"処理範囲自動抽出エラー (Cam{ControlID.ToString()} : {ret})");
					}
				}
				else
                {
					area = InspParam.InspAreaLeft;
					if (InspParam.EdgeParamLeft.IsEnabled)
					{
						ret = ClassEdgeSearch.MeasureBoxEdge(imgSrc, InspParam.EdgeParamLeft.EdgeSearchParam, out edgedata);
						if (ret == ErrorCode.Success && edgedata.pnts.Count > 0)
						{
							edgedata.CalcMean();
							//int right = (int)edgedata.meanX - InspParam.EdgeParamCam1.AreaOffset;
							//area.Width = (int)right - area.X;
							//							InspParam.InspAreaCam1 = area;
							int right = area.Right;
							area.X = (int)edgedata.meanX + InspParam.EdgeParamLeft.AreaOffset;
							area.Width = right - area.X;
						}
						else
							logger.Info($"処理範囲自動抽出エラー (Cam{ControlID.ToString()} : {ret})");
					}
				}

				if ((ret = _imageProc.DotInspect(InspParam.DarkBlob, area, DotInspResult)) != ErrorCode.None)
                {
                    logger.Info($"検査処理エラー ({ret})");
                    return ret;
                }
				if (_imgProc != null)
					_imgProc.Dispose();
				_imgProc = _imageProc.ImgProc.CopyImage();
                TotalBlobCount += DotInspResult.Count;
            }
 
            if (item == enInspectItem.RULE_INSPECT)        // ルール検査
			{
                RuleInspectionResult result = _ruleInspector.Inspect(imgSrc);
                if (_imgProc != null) 
                    _imgProc.Dispose();
				//_imgProc = _ruleInspector.ImageProc.CopyImage();
				_imgProc = new HImage(_ruleInspector.ImageProc);

				if (_imageProc.ImgBinary != null)
					_imageProc.ImgBinary.Dispose();
				//_imageProc.ImgBinary = _ruleInspector.ImageProc.CopyImage();
				_imageProc.ImgBinary = new HImage(_imgProc);

				ConvertRuleInspectResult(result, DotInspResult);
				TotalBlobCount += DotInspResult.Count;
			}

			GC.Collect();
			GC.WaitForPendingFinalizers(); 
            
            return ret;
        }

        void ConvertRuleInspectResult(RuleInspectionResult ruLeResult, List<BlobInfo> DotInspResult)
        {
            DotInspResult.Clear();

			for (int vNo = 0; vNo < ruLeResult.Yarns.Count; vNo++)
            {
                for (int blobNo = 0; blobNo < ruLeResult.Yarns[vNo].DefectCount; blobNo++)
                {
                    BlobInfo tBlob = new BlobInfo();
                    tBlob.X = ruLeResult.Yarns[vNo].DefectColumns[blobNo];
                    tBlob.Y = ruLeResult.Yarns[vNo].DefectRows[blobNo];
                    tBlob.Area = ruLeResult.Yarns[vNo].DefectAreas[blobNo];
                    tBlob.BlobRegion = new HRegion(ruLeResult.Yarns[vNo].DefectRegion[blobNo]);
					DotInspResult.Add(tBlob);
				}
			}
		}

        public void DrawRegion(HWindow hw, enShowRegion region)
        {
            _imageProc.DrawRegion(hw, region);
        }

        public void DrawBlobs(HWindow hw)
        {
            int TotalBlobs = 0;
            if (_itemMode == enInspectItem.RULE_INSPECT)
            {
                hw.SetColor("red");
                hw.SetDraw("margin");
                TotalBlobs = _imageProc.DrawBlobs(hw, DotInspResult, TotalBlobs);
            }
        }

        public int DispBlobList(ListView listView)
        {
            listView.Items.Clear();

            int TotalBlobs = 0;
            if ( _itemMode == enInspectItem.RULE_INSPECT)
                TotalBlobs = DispBlackBlobList(listView, 0);

            return TotalBlobs;
        }

        public int GetBlobCount()
        {
            return DotInspResult.Count;
        }
  
        private int DispBlackBlobList(ListView listView, int listTopNo)
        {
            string[] sItem = new string[10];
            int iDataCnt = DotInspResult.Count;       // 欠陥数
            for (int iNo = 0; iNo < iDataCnt; iNo++)
            {
                BlobInfo tBlob = DotInspResult[iNo];
                sItem[0] = (listTopNo + iNo + 1).ToString();
                sItem[1] = string.Format("({0}, {1})", tBlob.X.ToString("0"), tBlob.Y.ToString("0"));
                sItem[2] = tBlob.Width.ToString("0");
                sItem[3] = tBlob.Height.ToString("0");
                sItem[4] = tBlob.Area.ToString("0");
                sItem[5] = tBlob.MeanGray.ToString("0");
                sItem[6] = (tBlob.JudgeNo + 1).ToString();
                ListViewItem additem = new ListViewItem(new string[] { sItem[0], sItem[1], sItem[2], sItem[3], sItem[4], sItem[5], sItem[6] });
                listView.Items.Add(additem);
            }

            return listTopNo + iDataCnt;
        }

        public int SetResultData(List<ResultInfo> resultData)
        {
            int DefectCnt = 0;
            for (int i = 0; i < DotInspResult.Count; i++)
            {
                ResultInfo result = new ResultInfo();
                result.BlobData = DotInspResult[i];
                result.ImgSrc = new HImage(ImgSrc);
				result.ImgBinary = new HImage(_imageProc.ImgBinary);
				//result.ImgBinary = ImgSrc.CopyImage();
				result.JudgeOK = DotInspResult[i].JudgeOK;
                resultData.Add(result);
                DefectCnt++;
            }

            return DefectCnt;
        }

        public void RecordDiffBase(HImage imgSrc)
        {
            if (ImgDiffBase != null)
				ImgDiffBase.Dispose();

			ImgDiffBase = imgSrc.CopyImage();
        }
    }
}
