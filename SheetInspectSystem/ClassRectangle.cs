using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HFVisionInspect
{
	class ClassRectangle
	{
		public Rectangle Rect { get; set; }

		/// <summary>
		/// 規定値
		/// </summary>
		/// <remarks></remarks>
		public enum NUM {
			_RECT_CORNERS = 4,		//矩形コーナー数
			_SGMT_HIT = 30,			//矩形伸縮用線分当たり判定距離囲
			_CENTER_HIT = 50,		//矩形移動用センター当たり判定距離囲
			_CORNER_HIT = 50,		//矩形伸縮用コーナー当たり判定距離囲
			_MIN_RECT_W = 50,		//矩形最小幅
			_MIN_RECT_H = 50,		//矩形最大幅
		}

		/// <summary>
		/// ドラッグ当たり判定用
		/// </summary>
		/// <remarks></remarks>
		public enum HitID{
			NONE = -1,
			CORNER_LU,
			CORNER_RU,
			CORNER_RD,
			CORNER_LD,
			SIDE_U,
			SIDE_R,
			SIDE_D,
			SIDE_L,
			CENTER_MOVE,
		}

		public ClassRectangle(){
		}

		//ポイントヒット
		bool IsHitPoint(double x1, double y1, double x2, double  y2, double tolerance){
			bool isHit  = false;
			double r = CalcDistance2P(x1, y1, x2, y2);

			if( r < tolerance ){
				isHit = true;
			}

			return isHit;
		}

		//線分ヒット
		bool IsHitSegment(double x, double y, double px1, double py1, double px2, double py2, double tolerance)
		{
			bool isHit = false;
			double a, b, c;
			double d;
			double d1, d2, d2p;

			//線分 to 直線
			CalcLine(px1, py1, px2, py2, out a, out b, out c);

			//点と直線の距離をチェック
			d = DistancePointToLine(x, y, a, b, c);

			if (d < (double)NUM._SGMT_HIT){

				//線分を構成する2点間にあるかをチェック
				d1 = CalcDistance2P(x, y, px1, py1);
				d2 = CalcDistance2P(x, y, px2, py2);
				d2p = CalcDistance2P(px1, py1,px2, py2);
				if( d1 < d2p && d2 < d2p ){
					isHit = true;
				}
			}

			 return isHit;
		}

		//2点間の距離を計算
		double CalcDistance2P(double px1 , double py1 , double px2 , double py2){
			double diffX = px1 - px2;
			double diffY = py1 - py2;
			return Math.Sqrt(diffX * diffX + diffY * diffY);
		}

		//2点から直線係数を求める(エラーチェックなし)
		void CalcLine(double px1, double py1, double px2, double py2, out double a, out double b, out double c)
		{
			a = py1 - py2;
			b = px2 - px1;
			c = (px1 - px2) * py2 - (py1 - py2) * px2;
		}

		// 点と直線の距離
		double DistancePointToLine(double X, double Y, double A, double B , double C){
			return Math.Abs(A * X + B * Y + C) / Math.Sqrt(A * A + B * B);
		}

		//当たり判定
		public HitID JudgeHitRectangle( double posX, double posY){
			HitID hit = HitID.NONE;
			double[] x = new double[(int)NUM._RECT_CORNERS];
			double[] y = new double[(int)NUM._RECT_CORNERS];
			double centX, centY;
			x[0] = Rect.X;
			y[0] = Rect.Y;
			x[1] = Rect.X + Rect.Width - 1;
			y[1] = Rect.Y;
			x[2] = Rect.X + Rect.Width - 1;
			y[2] = Rect.Y + Rect.Height - 1;
			x[3] = Rect.X;
			y[3] = Rect.Y + Rect.Height - 1;
			centX = (x[0] + x[2]) / 2;
			centY = (y[0] + y[2]) / 2;

			//コーナー
			for (int i = 0; i < (int)NUM._RECT_CORNERS; i++)
			{
				if( IsHitPoint(posX, posY, x[i], y[i], (double)NUM._CORNER_HIT) ){
					hit = HitID.CORNER_LU + i;
				}
			}
			
			//各辺
			for (int i = 0; i < (int)NUM._RECT_CORNERS; i++)
			{
				if (IsHitSegment(posX, posY, x[i], y[i], x[(i + 1) % 4], y[(i + 1) % 4], (double)NUM._SGMT_HIT))
				{
					hit = HitID.SIDE_U + i;
				}
			}
			
			//センター
			if( IsHitPoint(posX, posY, centX, centY, (double)NUM._CENTER_HIT) ){
				hit = HitID.CENTER_MOVE;
			}

			return hit;
		}
	}
}
