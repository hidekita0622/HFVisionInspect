using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HalconDotNet;
using static HFVisionInspect.ClassEdgeSearch;
using System.ComponentModel;

namespace HFVisionInspect
{
	public class PointDatas
	{
		public List<PointF> pnts;
		public double meanX;
		public double meanY;

		public PointDatas()
		{
			pnts = new List<PointF>();
			meanX = 0;
		}

		public void Clear()
		{
			pnts.Clear();
		}

		public void Add(PointF pnt)
		{
			pnts.Add(pnt);
		}

		public void Add(PointDatas src)
		{
			pnts.AddRange(src.pnts);
		}

		public void Copy(PointDatas r)
		{
			this.pnts = new List<PointF>(r.pnts);
			this.meanX = r.meanX;
		}

		public void Draw(HWindow hw)
		{
			for (int i = 0; i < pnts.Count; i++)
			{
				double x = pnts[i].X;
				double y = pnts[i].Y;
				hw.DispCross(y, x, 5, 0);
			}
		}

		public void CalcMean()
		{
			double sumX = 0, sumY = 0;
			for (int i = 0; i < pnts.Count; i++)
			{
				double x = pnts[i].X;
				sumX += x;
				double y = pnts[i].Y;
				sumY += y;
			}
			meanX = sumX / pnts.Count;
			meanY = sumY / pnts.Count;
		}
	}

	public class EdgeSearchParameter
	{
		const string EdgeCategory = "\t検出パラメータ";

		public ClassEdgeSearch.EdgeParameter EdgeSearchParam;

		[Category(EdgeCategory), DisplayName("\t有効")]
		public bool IsEnabled { get; set; }
		[Category(EdgeCategory), DisplayName("閾値")]
		public double Threshold { get { return EdgeSearchParam.Threshold; } set { EdgeSearchParam.Threshold = value; } }
		[Category(EdgeCategory), DisplayName("平滑化シグマ")]
		public double Sigma { get { return EdgeSearchParam.Sigma; } set { EdgeSearchParam.Sigma = value; } }
		[Category(EdgeCategory), DisplayName("エッジ極性")]
		public EdgeTransition Trans { get { return EdgeSearchParam.Transition; } set { EdgeSearchParam.Transition = value; } }
		[Category(EdgeCategory), DisplayName("抽出点")]
		public EdgeSelect Select { get { return EdgeSearchParam.Select; } set { EdgeSearchParam.Select = value; } }
		[Category(EdgeCategory), DisplayName("エッジ方向")]
		public EdgeDirection Direction { get { return EdgeSearchParam.Direction; } set { EdgeSearchParam.Direction = value; } }
		[Category(EdgeCategory), DisplayName("範囲")]
		public Rectangle BoxArea { get { return EdgeSearchParam.BoxArea; } set { EdgeSearchParam.BoxArea = value; } }
		[Category(EdgeCategory), DisplayName("範囲オフセット")]
		public int AreaOffset { get; set; }

		public EdgeSearchParameter()
		{
			EdgeSearchParam = new ClassEdgeSearch.EdgeParameter();
			IsEnabled = false;
			AreaOffset = 50;
		}

		public void CopyFrom(EdgeSearchParameter src)
		{
			EdgeSearchParam.CopyFrom(src.EdgeSearchParam);
			IsEnabled = src.IsEnabled;
			AreaOffset = src.AreaOffset;
		}
	}
	/// <summary>
	/// エッジ検出クラス
	/// </summary>
	public static class ClassEdgeSearch
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public enum EdgeTransition { all = 0, negative, positive, }
		public enum EdgeSelect { all = 0, first, last, }
		public enum EdgeDirection { LeftToRight = 0, RightToLeft, TopToBottom, BottomToTop,}

		public enum EdgeArcDirection { CW = 0, CCW,}

        /// <summary>
        /// 指定エリア内のエッジ検出パラメータ
        /// </summary>
        public class EdgeParameter
		{
			public double Threshold { get; set; }
			public double Sigma { get; set; }
			public EdgeTransition Transition { get; set; }
			public EdgeSelect Select { get; set; }

			public PointF StartPos { get; set; }
			public PointF EndPos { get; set; }

			public int Length { get; set; }
			public Rectangle BoxArea { get; set; }
			public EdgeDirection Direction { get; set; }

			public EdgeParameter()
			{
				Threshold = 50.0;
				Sigma = 1.0;
				Transition = EdgeTransition.all;
				Select = EdgeSelect.first;
				StartPos = new PointF(0, 0);
				EndPos = new PointF(10, 10);
				Length = 3;
				BoxArea = new Rectangle(0, 0, 100, 100);
				Direction = EdgeDirection.LeftToRight;
			}

			public void CopyFrom(EdgeParameter src)
			{
				Threshold = src.Threshold;
				Sigma = src.Sigma;
				Transition = src.Transition;
				Select = src.Select;
				StartPos = src.StartPos;
				EndPos = src.EndPos;
				Length = src.Length;
				BoxArea = src.BoxArea;
				Direction = src.Direction;

            }
		}
	
		/// <summary>
		/// 指定エリア内のエッジ検出
		/// </summary>
		/// <param name="image">処理画像</param>
		/// <param name="param">パラメータ</param>
		/// <param name="result">検出結果データ</param>
		/// <returns>ErrorCode</returns>
		public static ErrorCode MeasureBoxEdge(HImage image, EdgeParameter param, out PointDatas result)
		{
			result = new PointDatas();
			try
			{
				HTuple threshold = param.Threshold;
				HTuple sigma = param.Sigma;
				float dx = param.EndPos.X - param.StartPos.X;
				float dy = param.EndPos.Y - param.StartPos.Y;
				HTuple length1 = Math.Sqrt(dx * dx + dy * dy) / 2.0;
				HTuple length2 = 3;
				HTuple transition, select;
				if (param.Transition == EdgeTransition.all)
					transition = "all";
				else if (param.Transition == EdgeTransition.negative)
					transition = "negative";
				else
					transition = "positive";
				if (param.Select == EdgeSelect.all)
					select = "all";
				else if (param.Select == EdgeSelect.first)
					select = "first";
				else
					select = "last";
				HTuple phi = Math.Atan2(dy, dx);
				HTuple width, height;
				int stX, stY, edX, edY, step;
				step = param.Length;
				HOperatorSet.GetImageSize(image, out width, out height);
				PointF pnt = new PointF();

				// 横方向エッジ検出
				if (param.Direction == EdgeDirection.LeftToRight || param.Direction == EdgeDirection.RightToLeft)
				{
					stX = param.BoxArea.Left;
					edX = param.BoxArea.Right;
					stY = param.BoxArea.Top;
					edY = param.BoxArea.Bottom;
					if (param.Direction == EdgeDirection.LeftToRight)
					{
						phi = 0;
					}
					else
					{
						phi = Math.PI;
					}
					HTuple CenterCol = (stX + edX) / 2;
					HTuple CenterRow;
					length1 = edX - stX;
					length2 = step;
					for (int y = stY ; y < edY; y += step)
					{
                        HTuple tMeasureHandle = new HTuple();
                        CenterRow = y + step / 2;
						HOperatorSet.GenMeasureRectangle2(CenterRow, CenterCol, phi, length1,
							length2, width, height, "bilinear", out tMeasureHandle);
						HTuple edgeCol, edgeRow, amplitude, distance;
						HOperatorSet.MeasurePos(image, tMeasureHandle, sigma, threshold, transition, select,
							out edgeRow, out edgeCol, out amplitude, out distance);
						if (edgeRow.Length > 0 && edgeRow >= stY && edgeRow <= edY && edgeCol >= stX && edgeCol <= edX)
						{
							pnt.X = edgeCol[0].F;
							pnt.Y = edgeRow[0].F;
							result.pnts.Add(pnt);
						}
                        HOperatorSet.CloseMeasure(tMeasureHandle);
                    }
                }
				// 縦方向エッジ検出
				else
				{
					stX = param.BoxArea.Left;
					edX = param.BoxArea.Right;
					stY = param.BoxArea.Top;
					edY = param.BoxArea.Bottom;
					if (param.Direction == EdgeDirection.TopToBottom)
					{
						phi = Math.PI / 2 * -1;
					}
					else
					{
						phi = Math.PI / 2;
					}

					HTuple CenterCol;
					HTuple CenterRow = (stY + edY) / 2;
					length1 = (edY - stY) / 2;
					length2 = step / 2;
					for (int x = stX; x < edX; x += step)
					{
                        HTuple tMeasureHandle = new HTuple();
                        CenterCol = x + step / 2;
						HOperatorSet.GenMeasureRectangle2(CenterRow, CenterCol, phi, length1,
							length2, width, height, "bilinear", out tMeasureHandle);
						HTuple edgeCol, edgeRow, amplitude, distance;
						HOperatorSet.MeasurePos(image, tMeasureHandle, sigma, threshold, transition, select,
							out edgeRow, out edgeCol, out amplitude, out distance);
						if(edgeRow.Length > 0)
						{
							pnt.X = edgeCol[0].F;
							pnt.Y = edgeRow[0].F;
							result.pnts.Add(pnt);
						}
                        HOperatorSet.CloseMeasure(tMeasureHandle);
                    }
                }
			}
			catch (HalconException ex)
			{
				logger.Error($"HalconException: {ex.Message}");
				return ErrorCode.OtherError;
			}

			return ErrorCode.Success;
		}

		/// <summary>
		/// 円弧上のエッジ検出パラメータ
		/// </summary>
        public class ArcEdgeParameter
        {
            public double Threshold { get; set; }
            public double Sigma { get; set; }
            public EdgeTransition Transition { get; set; }
            public EdgeSelect Select { get; set; }

            public PointF CenterPos { get; set; }		// 中心座標
			public double MinRadius { get; set; }           // 最小半径
            public double MaxRadius { get; set; }			// 最大半径
            public double StartAngle { get; set; }		// 始点角度(Deg)
            public double ExtentAngle { get; set; }		// 探索角度(Deg)
			public EdgeArcDirection ArkDirection { get; set; }	// 探索方向

            public int Length { get; set; }			// 探索間隔

            public ArcEdgeParameter()
            {
                Threshold = 20.0;
                Sigma = 5.0;
                Transition = EdgeTransition.all;
                Select = EdgeSelect.first;
                CenterPos = new PointF(10, 10);
				MinRadius = 100;
                MaxRadius = 300;
                StartAngle = 0;
				ExtentAngle = 60;
				ArkDirection = EdgeArcDirection.CCW;
                Length = 1;
            }

            public void CopyFrom(ArcEdgeParameter src)
            {
                Threshold = src.Threshold;
                Sigma = src.Sigma;
                Transition = src.Transition;
                Select = src.Select;
                CenterPos = src.CenterPos;
				MinRadius = src.MinRadius;
                MaxRadius = src.MaxRadius;
                StartAngle = src.StartAngle;
				ExtentAngle = src.ExtentAngle;
				ArkDirection = src.ArkDirection;
                Length = src.Length;
            }
        }

        /// <summary>
        /// 円弧上のエッジ検出
        /// </summary>
        /// <param name="image">処理画像</param>
        /// <param name="param">パラメータ</param>
        /// <param name="result">検出結果データ</param>
        /// <returns>ErrorCode</returns>
        public static ErrorCode MeasureArcEdge(HImage image, ArcEdgeParameter param, out PointDatas result)
        {
            result = new PointDatas();
            try
            {
                HTuple threshold = param.Threshold;
                HTuple sigma = param.Sigma;
                HTuple length1 = 3;
                HTuple transition, select;
                if (param.Transition == EdgeTransition.all)
                    transition = "all";
                else if (param.Transition == EdgeTransition.negative)
                    transition = "negative";
                else
                    transition = "positive";
                if (param.Select == EdgeSelect.all)
                    select = "all";
                else if (param.Select == EdgeSelect.first)
                    select = "first";
                else
                    select = "last";
                HTuple width, height;
                HTuple minR = param.MinRadius;
                HTuple maxR = param.MaxRadius;
                //HTuple angleSt = ImageProc.ToRadians( param.StartAngle);
                //HTuple angleEx = ImageProc.ToRadians(param.ExtentAngle);
				HTuple angleSt = param.StartAngle;
				HTuple angleEx = param.ExtentAngle;
				if (param.ArkDirection == EdgeArcDirection.CW)	// 時計回り
				{
									angleEx *= -1;
				}
				int step = param.Length;
                HOperatorSet.GetImageSize(image, out width, out height);
                PointF pnt = new PointF();

                {
                    HTuple CenterCol = param.CenterPos.X;
                    HTuple CenterRow = param.CenterPos.Y;
                    for (int r = (int)param.MinRadius; r < (int)param.MaxRadius; r += step)
                    {
						HTuple rs = r;
                        HTuple tMeasureHandle = new HTuple();
                        HOperatorSet.GenMeasureArc(CenterRow, CenterCol, rs, angleSt, angleEx, 3, width, height, "nearest_neighbor", out tMeasureHandle);
                        HTuple edgeCol, edgeRow, amplitude, distance;
                        HOperatorSet.MeasurePos(image, tMeasureHandle, sigma, threshold, transition, select,
                            out edgeRow, out edgeCol, out amplitude, out distance);
                        if (edgeRow.Length > 0)
                        {
                            pnt.X = edgeCol[0].F;
                            pnt.Y = edgeRow[0].F;
                            result.pnts.Add(pnt);
                        }
                        HOperatorSet.CloseMeasure(tMeasureHandle);
                    }
                }
            }
            catch (HalconException ex)
            {
                logger.Error($"HalconException: {ex.Message}");
                return ErrorCode.Unknown;
            }

            return ErrorCode.Success;
        }


    }
}
