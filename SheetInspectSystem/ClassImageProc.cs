using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Diagnostics;
using System.Drawing;

namespace HFVisionInspect
{
    public enum ErrorCode
    {
        None = 0,
		Success = 0,


		ImgError = 10,
        ParamError = 100,
        AreaError = 110,
        BinaryError = 120,

        CheckShapeError = 200,

        OtherError = 1000,
		Unknown = 9999,
	}

    public enum BlobGrayMode
    {
        Dark = 0,
        Light,
        Mean,
    }

    public struct BlobInfo
    {
        public HRegion BlobRegion;
        public double X;
        public double Y;
        public double Left;
        public double Right;
		public double Top;
		public double Botttom;
		public double Area;        // 欠陥面積
        public double Width;
        public double Height;
        public double MinGray;          // 最小輝度値
        public double MaxGray;          // 最大輝度値
        public double MeanGray;         // 平均輝度値

        public int JudgeNo;
        public bool JudgeOK;
    }

    public class ResultInfo
    {
        public BlobInfo BlobData { get; set; }
        public HImage ImgSrc { get; set; }
        public HImage ImgBinary { get; set; }
        public bool JudgeOK { get; set; }

        public ResultInfo()
        {
            BlobData = new BlobInfo();
            ImgSrc = new HImage();
            ImgBinary = new HImage();
            JudgeOK = false;
        }

        public virtual void Dispose()
        {
            ImgSrc.Dispose();
            ImgBinary.Dispose();
        }

        public void CopyFrom(ResultInfo src)
        {
            BlobData = src.BlobData;
            if (ImgSrc != null)
                ImgSrc.Dispose();
            ImgSrc = src.ImgSrc.CopyImage();
            if (ImgBinary != null)
                ImgBinary.Dispose();
            ImgBinary = src.ImgBinary.CopyImage();
            JudgeOK = src.JudgeOK;
        }
    }

    public enum enShowRegion
    {
        None = 0,
        Binary,
        DotResult,
        WaterSheds,
    }

    public class ClassImageProc
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //public InspectParameter InspParam { set; get; }
        private HImage _imgSrc;
        private HImage _imgProc;
        private HImage _imgDiffBase;
        public HImage ImgProc { get { return _imgProc; } }
        public HImage ImgBinary { get; set; }
        private Rectangle _procArea;
        public Rectangle ImageProcArea { get { return _procArea; } }

        public HRegion BinRegion;

        public ClassImageProc()
        {
            _imgSrc = new HImage();
            _imgProc = new HImage();
			_imgDiffBase = new HImage();
			_procArea = new Rectangle();
            ImgBinary = new HImage();

            BinRegion = new HRegion();
        }

        public virtual void Dispose()
        {
            _imgSrc.Dispose();
			_imgProc.Dispose();
			_imgDiffBase.Dispose();
			ImgBinary.Dispose();
			BinRegion.Dispose();
        }

        protected int GaussFilter(HImage srcImg, int size, out HImage dstImg)
        {
            try
            {
                //dstImg = srcImg.GaussFilter(size);
                double alpha = (double)size;
                if (alpha < 0.1)
                    alpha = 0.1;
                else if(alpha > 50)
                    alpha = 50;

				dstImg = srcImg.SmoothImage("gauss", alpha);
			}
			catch (HOperatorException oe)
            {
                dstImg = srcImg.Clone();
                Trace.WriteLine("GaussFilter Error : " + oe.Message);
                logger.Info("GaussFilter Error : " + oe.Message);
                return -1;
            }

            return 0;
        }
        public double GetGrayLevel(HImage srcImg, Rectangle area)
        {
            try
            {
                HRegion regionArea = new HRegion();
                regionArea.GenRectangle1((double)area.Top, (double)area.Left, (double)area.Bottom, (double)area.Right);

                double div;
                double mean = srcImg.Intensity(regionArea, out div);

                return mean;
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine("GetGrayLevel Error : " + oe.Message);
                logger.Info("GetGrayLevel Error : " + oe.Message);

                return 0;
            }
        }

        public ErrorCode SubImageProc()
        {


            return ErrorCode.None;
        }

        public ErrorCode TiledImage(HImage image0, HImage image1, out HImage outimage)
        {
            outimage = new HImage();
            outimage.GenEmptyObj();
            //return ErrorCode.None;

            try
            {
                HImage imgTemp = new HImage();
                HImage imgTile = new HImage();
                imgTemp.GenEmptyObj();
                imgTile.GenEmptyObj();

                imgTemp = imgTemp.ConcatObj(image0);
                imgTemp = imgTemp.ConcatObj(image1);
                imgTile = imgTemp.TileImages(1, "vertical");
                outimage = imgTile.CopyImage();

                imgTemp.Dispose();
                imgTile.Dispose();
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine("ImageConvert Error : " + oe.Message);
                return ErrorCode.ImgError;
            }

            return ErrorCode.None;
        }

        public void DrawRegion(HWindow hw, enShowRegion region)
        {
            hw.DispRegion(BinRegion);
        }

        public int DrawBlobs(HWindow hw, List<BlobInfo> result, int topNo)
        {
			int blobs = result.Count;
			try
			{
				if (blobs > 0)
				{
					int x, y, w, h;
					hw.GetWindowExtents(out y, out x, out w, out h);
					HTuple type, width, height;
					_imgProc.GetImagePointer1(out type, out width, out height);
					double r = (width.D / (double)w);

					HObject regions = new HObject();
					regions.GenEmptyObj();
					double sx, sy, wx, wy;
					HTuple ty, tx;
					for (int i = 0; i < blobs; i++)
					{
						regions = regions.ConcatObj(result[i].BlobRegion);
						sy = result[i].BlobRegion.RegionFeatures("row1");
						sx = result[i].BlobRegion.RegionFeatures("column1");
                        //hw.ConvertCoordinatesWindowToImage(sy, sx, out wy, out wx);
                        //sx = result[i].X;
						hw.ConvertCoordinatesImageToWindow(sy, sx, out wy, out wx);
						ty = (double)wy - 10;
						tx = (double)wx - 10;
						HOperatorSet.DispText(hw, (i + topNo + 1).ToString(), "window", ty, tx, "green", "box", "false");
					}
					hw.DispObj(regions);
					regions.Dispose();
				}
			}
			catch (HOperatorException oe)
			{
				logger.Trace(oe.Message);
			}

            return topNo + blobs;
        }

        public void InspectReset(HImage srcImg, HImage diffImg)
        {
            if (_imgSrc != null)
                _imgSrc.Dispose();
            _imgSrc = srcImg.CopyImage();
            _imgProc = srcImg.CopyImage();

			//if (_imgDiffBase != null)
			//	_imgDiffBase.Dispose();
			//_imgDiffBase = diffImg.CopyImage();
		}

		private ErrorCode GetDifferenceImage(HImage imgSrc, out HImage imgDiff)
        {
            ErrorCode ret = ErrorCode.None;
            imgDiff = new HImage();

            try
            {
                HImage tempSrc0 = new HImage();
                HImage tempSrc1 = new HImage();

                int w, h;
                imgSrc.GetImageSize(out w, out h);
                int left, top, right, bottom;
                left = 0;
                right = w - 1;
                int th = h / 2;
                top = 0;
                bottom = th - 1;
                tempSrc0 = imgSrc.CropRectangle1(top, left, bottom, right);
                top = th;
                bottom = h - 1;
                tempSrc1 = imgSrc.CropRectangle1(top, left, bottom, right);

                double mult = 1.0;
                imgDiff = tempSrc0.AbsDiffImage(tempSrc1, mult);

                tempSrc0.Dispose();
                tempSrc1.Dispose();
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
                ret = ErrorCode.OtherError;
            }

            return ret;
        }

        protected ErrorCode ImageBinary(BinaryProcParam binParam, Rectangle binarea, HImage imgSrc, out HRegion imgBinaryResult)
        {
            ErrorCode ret = ErrorCode.None;

            imgBinaryResult = new HRegion();
            imgBinaryResult.GenEmptyObj();
            HRegion tempBinary = new HRegion();
            tempBinary.GenEmptyObj();

            HRegion tempArea = new HRegion();
            tempArea.GenRectangle1((double)binarea.Top, (double)binarea.Left, (double)binarea.Bottom, (double)binarea.Right);
            HImage tempSrc = new HImage();
            tempSrc = imgSrc.ReduceDomain(tempArea);
            //imgSrcMask = imgSrc.Clone();
            //HOperatorSet.ReduceDomain(tempSrc, regionMask, out imgSrcMask);
            if (binParam.BinaryMode == ImageBinaryMode.BINARY_SAUVOLA)
            {
                if (binParam.BinaryColor == ImageBinaryColor.BINARY_DARK)
                {
                    //マスクした画像をSauvolaによる２値化を行い、暗い部分を検出
                    tempBinary = tempSrc.LocalThreshold("adapted_std_deviation", "dark", 
                        (new HTuple("mask_size")).TupleConcat("scale"), (new HTuple(binParam.SauvolaMaskSize)).TupleConcat(binParam.SauvolaScale));
                }
                else if (binParam.BinaryColor == ImageBinaryColor.BINARY_LIGHT)
                {
                    //マスクした画像をSauvolaによる２値化を行い、明るい部分を検出
                    tempBinary = tempSrc.LocalThreshold("adapted_std_deviation", "light",
                        (new HTuple("mask_size")).TupleConcat("scale"), (new HTuple(binParam.SauvolaMaskSize)).TupleConcat(binParam.SauvolaScale));
                }
                else
                {
                    HRegion tempBinary0 = new HRegion();
                    HRegion tempBinary1 = new HRegion();
                    tempBinary0.GenEmptyObj();
                    tempBinary1.GenEmptyObj();
                    tempBinary0 = tempSrc.LocalThreshold("adapted_std_deviation", "dark",
                        (new HTuple("mask_size")).TupleConcat("scale"), (new HTuple(binParam.SauvolaMaskSize)).TupleConcat(binParam.SauvolaScale));
                    tempBinary1 = tempSrc.LocalThreshold("adapted_std_deviation", "light",
                        (new HTuple("mask_size")).TupleConcat("scale"), (new HTuple(binParam.SauvolaMaskSize)).TupleConcat(binParam.SauvolaScale));
                    tempBinary = tempBinary0.Union2(tempBinary1);
                    tempBinary0.Dispose();
                    tempBinary1.Dispose();
                }
            }
            else if (binParam.BinaryMode == ImageBinaryMode.BINARY_SIMPLE)
            {
                HTuple mean, deviation;
                mean = tempArea.Intensity(tempSrc, out deviation);

                tempBinary = tempSrc.Threshold((double)binParam.ThresholdLo, (double)binParam.ThresholdHi);
                //HOperatorSet.Threshold(tempSrc, out tempBinary, thresLow, thresHigh);
            }
            else if (binParam.BinaryMode == ImageBinaryMode.BINARY_VAR)
            {
                HImage tempReduced = new HImage();
                HImage tempSmooth = new HImage();

                HRegion tempShapeRegion = new HRegion();
                HRegion tempFillRegion = new HRegion();
                HRegion tempErosion = new HRegion();
                HRegion tempVar = new HRegion();
                tempReduced.GenEmptyObj();
                tempShapeRegion.GenEmptyObj();
                tempFillRegion.GenEmptyObj();
                tempErosion.GenEmptyObj();
                tempVar.GenEmptyObj();
                tempSmooth.GenEmptyObj();

                tempReduced = tempSmooth.ReduceDomain(tempErosion);
                tempBinary = tempSrc.VarThreshold(binParam.VarMaskWidth, binParam.VarMaskHeight, binParam.VarDevScale, binParam.VarThreshold, "not_equal");

                tempReduced.Dispose();
                tempSmooth.Dispose();
                tempShapeRegion.Dispose();
                tempFillRegion.Dispose();
                tempErosion.Dispose();
                tempVar.Dispose();
            }
            else
            {
                ret =  ErrorCode.ParamError;
            }

            imgBinaryResult = tempBinary.Clone();

            tempSrc.Dispose();
            tempBinary.Dispose();
            tempArea.Dispose();

            return ret;
        }

        protected ErrorCode ImageMorphology(MorphologyParam MorParam, HRegion imgSrc, out HRegion imgResult)
        {
            ErrorCode ret = ErrorCode.None;

            imgResult = new HRegion();
            imgResult.GenEmptyObj();
            HRegion tempBinary = new HRegion();
            tempBinary.GenEmptyObj();
            tempBinary = imgSrc.Clone();

            HRegion tempMorphology = new HRegion();
            tempMorphology.GenEmptyObj();

            int length;
            double dLength;
            for (int i = 0; i < MorphologyParam.MaxMorphology; i++)
            {
                length = MorParam.Length[i];
                dLength = (double)length + 0.5;
                if (MorParam.Type[i] == enMorphologyType.None)
                {
                    continue;
                }
                else if (MorParam.Type[i] == enMorphologyType.Erosion)
                {
                    //tempMorphology = tempBinary.ErosionRectangle1(length, length);
                    tempMorphology = tempBinary.ErosionCircle(dLength);
                }
                else if (MorParam.Type[i] == enMorphologyType.Dilation)
                {
                    //tempMorphology = tempBinary.DilationRectangle1(length, length);
                    tempMorphology = tempBinary.DilationCircle(dLength);
                }
                else if (MorParam.Type[i] == enMorphologyType.Closing)
                {
                    tempMorphology = tempBinary.ClosingCircle((double)dLength);
                }
                else if (MorParam.Type[i] == enMorphologyType.Opening)
                {
                    tempMorphology = tempBinary.OpeningCircle((double)dLength);
                }
                tempBinary.Dispose();
                tempBinary = tempMorphology.Clone();
                tempMorphology.Dispose();

            }
            imgResult = tempBinary.Clone();

            tempBinary.Dispose();

            return ret;
        }

        public ErrorCode FindBlob(HRegion srcImg, BlobParameter param, BlobGrayMode gray, List<BlobInfo> result)
        {
            ErrorCode ret = ErrorCode.None;

            result.Clear();

            HRegion tempConnect = new HRegion();
            tempConnect.GenEmptyObj();
            HRegion selectArea = new HRegion();
            selectArea.GenEmptyObj();
            HRegion selectGray = new HRegion();
            selectGray.GenEmptyObj();
            HRegion selectWidth = new HRegion();
            selectWidth.GenEmptyObj();
            HRegion selectHeight = new HRegion();
            selectHeight.GenEmptyObj();
            HRegion blobRegion = new HRegion();
            blobRegion.GenEmptyObj();
            HRegion wsRegion = new HRegion();
            wsRegion.GenEmptyObj();
            HRegion selectAll = new HRegion();
            selectAll.GenEmptyObj();

            try
            {
                tempConnect = srcImg.Connection();
                int blobs = tempConnect.CountObj();
                List<int> judgeNum = new List<int>();

                for (int i = 0; i < BlobParameter.BlobJudgeNum; i++)
                {
                    if (param.JudgeParam[i].Enable == false)
                        continue;
                    selectArea.GenEmptyObj();
                    selectArea = tempConnect.SelectShape("area", "and", param.JudgeParam[i].MinArea, param.JudgeParam[i].MaxArea);
                    //selectArea = selectArea.ConcatObj(selectArea);
                    blobs = selectArea.CountObj();

                    selectGray = selectArea.SelectGray(_imgProc, "mean", "and", param.JudgeParam[i].MinGray, param.JudgeParam[i].MaxGray);
                    blobs = selectGray.CountObj();

                    blobs = selectGray.CountObj();
                    for (int j = 0; j < blobs; j++)
                        judgeNum.Add(i);

                    selectAll = selectAll.ConcatObj(selectGray);

                    selectArea.Dispose();
                }
                blobs = selectAll.CountObj();
                BlobInfo tBlob;
                bool bRet;
                for (int i = 0; i < blobs; i++)
                {
                    blobRegion = selectAll.SelectObj(i + 1);
                    double min, max, range;
                    blobRegion.MinMaxGray(_imgProc, 0, out min, out max, out range);
                    int pno = judgeNum[i];
                    {
                        bRet = RegistBlob(_imgProc, blobRegion, out tBlob);
                        tBlob.JudgeNo = pno;
                        if (param.JudgeParam[pno].NGJudgeEnable)        // NGとして判定するか
                            tBlob.JudgeOK = false;                   

                        int cnt = result.Count;
                        for (int j = 0; j < cnt; j++)
                        {
                            double x = result[j].X;
                            double y = result[j].Y;
                            if (x == tBlob.X && y == tBlob.Y)
                                bRet = false;
                        }

                        if (bRet)
                            result.Add(tBlob);
                    }
                    blobRegion.Dispose();
                }
                judgeNum.Clear();
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
                ret = ErrorCode.OtherError;
            }

            wsRegion.Dispose();
            tempConnect.Dispose();
            selectArea.Dispose();
            selectGray.Dispose();
            selectWidth.Dispose();
            selectHeight.Dispose();
            blobRegion.Dispose();
            selectAll.Dispose();

            return ret;
        }

        protected bool RegistBlob(HImage srcImg, HRegion objBlob, out BlobInfo blobData)
        {
            blobData = new BlobInfo();
            blobData.BlobRegion = new HRegion(objBlob);

            double sy, ey, margin = -1;
            sy = objBlob.RegionFeatures("row1");
            if (_procArea.Top + margin > (int)sy)
                return false;
            ey = objBlob.RegionFeatures("row2");
            if (_procArea.Bottom - margin < (int)ey)
                return false;


            blobData.Area = objBlob.RegionFeatures("area");
            blobData.X = objBlob.RegionFeatures("column");
            blobData.Y = objBlob.RegionFeatures("row");
            blobData.Left = objBlob.RegionFeatures("column1");
            blobData.Right = objBlob.RegionFeatures("column2");
            blobData.Top = objBlob.RegionFeatures("row1");
			blobData.Botttom = objBlob.RegionFeatures("row2");
			blobData.Width = objBlob.RegionFeatures("width");
            blobData.Height = objBlob.RegionFeatures("height");
            //blobData.Circularity = objBlob.RegionFeatures("circularity");
            //blobData.Roundness = objBlob.RegionFeatures("roundness");
            //blobData.DistDivCenter = objBlob.RegionFeatures("dist_deviation");
            //blobData.Convexity = objBlob.RegionFeatures("circularity");

            double min, max, range;
            objBlob.MinMaxGray(srcImg, 0, out min, out max, out range);
            blobData.MinGray = min;
            blobData.MaxGray = max;
            blobData.MeanGray = objBlob.Intensity(_imgProc, out range);
            blobData.JudgeOK = true;

            return true;
         }

        public ErrorCode DotInspect(BlobParameter param, Rectangle procArea,  List<BlobInfo> DotInspResult)
        {
            if (!param.ProcEnable)
                return ErrorCode.None;

            ErrorCode ret = ErrorCode.None;
            HImage srcImg = new HImage();
            HRegion binImg = new HRegion();
            try
            {
                srcImg.GenEmptyObj();
                if (_imgProc != null)
                    _imgProc.Dispose();

                if (param.GaussEnable)
                {
                    GaussFilter(_imgSrc, param.GaussAlpha, out srcImg);
				}
                else
                {
					srcImg = _imgSrc.CopyImage();
				}

				if (param.DiffImage)
                {
#if true
					HImage diff = new HImage();
					_imgProc = srcImg.AbsDiffImage(_imgDiffBase, (double)param.DiffMult);
					_procArea = procArea;
#else
                    GetDifferenceImage(srcImg, out _imgProc);
                    int w, h;
                    _imgProc.GetImageSize(out w, out h);
                    _procArea.X = procArea.X;
                    _procArea.Width = procArea.Width;
                    _procArea.Y = 0;
                    _procArea.Height = h;
#endif
				}
				else
                {
                    _imgProc = srcImg.CopyImage();
                    _procArea = procArea;
                }

                binImg.GenEmptyObj();
                //param.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_DARK;
                //if (ImageBinary(param.BinaryParam, procArea, srcImg,  out binImg) == ErrorCode.None)
                if (ImageBinary(param.BinaryParam, _procArea, _imgProc, out binImg) == ErrorCode.None)          // 2値化
                {
					if (ImageMorphology(param.MorphologyParam, binImg, out binImg) == ErrorCode.None)           // モルフォロジー
                    {
						if (BinRegion != null)
							BinRegion.GenEmptyObj();
						BinRegion = binImg.Clone();

						if (ImgBinary != null)
							ImgBinary.Dispose();
						int w, h;
						srcImg.GetImageSize(out w, out h);
						ImgBinary = binImg.RegionToBin(255, 0, w, h);

						FindBlob(BinRegion, param, BlobGrayMode.Dark, DotInspResult);                           // Blob処理
					}
				}


            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
                ret = ErrorCode.OtherError;
            }

            srcImg.Dispose();
            binImg.Dispose();

            return ret;
        }

    }

    public class ClassImageDiff
    {
		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public double GaussAlpha { get; set; } = 3;
        public double DiffImageMult { get; set; } = 5;

		public ErrorCode CreateDiffImage(HImage imgSrc, HImage imgBase, ref HImage imgDiff)
        {
			ErrorCode retCode = ErrorCode.None;

			try
            {
                HImage diff = new HImage();
                diff = imgSrc.AbsDiffImage(imgBase, DiffImageMult);

				HImage meanSrc = new HImage();
				meanSrc = diff.SmoothImage("gauss", GaussAlpha);
				
                imgDiff = meanSrc.CopyImage();

				meanSrc.Dispose();
                diff.Dispose();
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
				retCode = ErrorCode.OtherError;
			}

			return retCode;
        }
	}
}
