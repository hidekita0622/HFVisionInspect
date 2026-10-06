using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using HalconDotNet;

namespace HFVisionInspect
{
	public partial class FormGetArea : Form
	{
		public enum enAdjustAmount
		{
			_1,				//
			_10,			//
			_100,			//
		}
		
		private HImage _imgSrc;		// ファイルから読み込んだ状態のHalcon Image object
		private string m_initialDirectory = Application.StartupPath;
		private double m_dispMag = 1.0;
		int preCursorX;
		int preCursorY;
		public bool ShowWorkEdgePos = true;

		ClassRectangle m_RUI = new ClassRectangle(); 

		bool m_isDrag = false;
		ClassRectangle.HitID m_dragHit = ClassRectangle.HitID.NONE;
		
		Rectangle m_rect = new Rectangle();
		public EdgeSearchParameter EdgeParam {  get; set; }
		public ViewID ControlID { get; set; }

		public Rectangle Rect
		{
			get { return this.m_rect; }
			set { this.m_rect = value; }
		}
		
		Rectangle m_rectLimit = new Rectangle();
		public int WorkEdgePos = 0;

		public FormGetArea()
		{
			InitializeComponent();

			_imgSrc = new HImage();
			_imgSrc.GenEmptyObj();
			m_rect.X = 10;
			m_rect.Y = 10;
			m_rect.Width = 50;
			m_rect.Height = 50;

			EdgeParam = new EdgeSearchParameter();

			hWindowControlDisplayImage.Size = panelDisplayImage.Size;


            RadioButtonAdjustDecr.Visible = false;	//つけてはみたものの、ボタンを押した際に直感的な挙動が不明
		}


        /// <summary>
        /// 画像セット
        /// </summary>
        /// <param name="ho_Img"></param>
        public void SetImage(HImage ho_Img)
		{
			try
			{
				if (_imgSrc != null)
					_imgSrc.Dispose();
                _imgSrc = ho_Img.CopyImage();

				HTuple hv_width, hv_height;
				HOperatorSet.GetImageSize(_imgSrc, out hv_width, out hv_height);

				m_rectLimit.X = 0;
				m_rectLimit.Y = 0;
				m_rectLimit.Width = hv_width;
				m_rectLimit.Height = hv_height;
			}
			catch(Exception ex) {
				Trace.WriteLine( ex.Message );
			}
		}
		private void FormSnipRectangleUI_Load(object sender, EventArgs e)
		{
			comboBoxAdjustAmount.Items.Add("  1");
			comboBoxAdjustAmount.Items.Add(" 10");
			comboBoxAdjustAmount.Items.Add("100");
			comboBoxAdjustAmount.SelectedIndex = (int)enAdjustAmount._1;

			textWorkEdgePos.Text = WorkEdgePos.ToString();

			m_RUI.Rect = m_rect;
            numAreaLeft.Value = (decimal)m_RUI.Rect.Left;
            numAreaRight.Value = (decimal)m_RUI.Rect.Right;
            numAreaTop.Value = (decimal)m_RUI.Rect.Top;
            numAreaBottom.Value = (decimal)m_RUI.Rect.Bottom;

            SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);

			textWorkEdgePos.Visible = ShowWorkEdgePos;
			labelWorkEdgePos.Visible = ShowWorkEdgePos;

			propertyEdgeParameter.SelectedObject = EdgeParam;
			propertyEdgeParameter.ExpandAllGridItems();
		}

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="ho_Img"></param>
		/// <param name="isFit"></param>
		private void SetDisplayImage(HObject ho_Img, bool isFit)
		{
			if (ho_Img == null) return;

			try
			{
				//ちらつき http://linx.jp/image/news/express/2010/linx_express_i10094.shtml
				HOperatorSet.SetSystem("flush_graphic", "false");

				hWindowControlDisplayImage.HalconWindow.ClearWindow();

				HTuple ht_imgW, ht_imgH, ht_imgT, ht_imgP;
				HOperatorSet.GetImagePointer1(ho_Img, out ht_imgP, out ht_imgT, out ht_imgW, out ht_imgH);
				int imgW = ht_imgW[0];
				int imgH = ht_imgH[0];

				Rectangle dr = hWindowControlDisplayImage.DisplayRectangle;
				Rectangle ip = hWindowControlDisplayImage.ImagePart;

				m_dispMag = 1.0;
				int windowW = dr.Width;
				int windowH = dr.Height;
				double magImgW = imgW * m_dispMag;
				double magImgH = imgH * m_dispMag;

				Size sz = new Size();
				sz = panelDisplayImage.Size;
				if (isFit)
				{
					//fit(aspect fit)
					ip.X = 0;
					ip.Y = 0;

					int margin = 3;

					double mag_w = (double)(sz.Width - margin) / imgW;
					double mag_h = (double)(sz.Height - margin) / imgH;
					if (mag_h > mag_w)
					{
						m_dispMag = mag_w;
					}
					else
					{
						m_dispMag = mag_h;
					}
					ip.Width = (int)(imgW);
					ip.Height = (int)(imgH);
					sz.Width = (int)(imgW * m_dispMag);
					sz.Height = (int)(imgH * m_dispMag);
					hWindowControlDisplayImage.WindowSize = sz;
					hWindowControlDisplayImage.ImagePart = ip;
                    //sz.Width = (int)(imgW * m_dispMag) + 10;
                    //sz.Height = (int)(imgH * m_dispMag) + 10;
                    //panelDisplayImage.Size = sz;
                }
                else
				{
					//表示サイズ
					dr.Width = (int)magImgW;
					dr.Height = (int)magImgH;

					//画像の領域
					ip.X = 0;
					ip.Y = 0;
					ip.Width = imgW;
					ip.Height = imgH;
					hWindowControlDisplayImage.ImagePart = ip;
					sz.Width = (int)magImgW;
					sz.Height = (int)magImgH;
					hWindowControlDisplayImage.Size = sz;
                    panelDisplayImage.Size = hWindowControlDisplayImage.WindowSize;
                }
                labelMousePos.Location = new Point(labelMousePos.Location.X, hWindowControlDisplayImage.Location.Y + hWindowControlDisplayImage.Size.Height + 20);

                HOperatorSet.DispObj(ho_Img, hWindowControlDisplayImage.HalconWindow);
				HOperatorSet.SetSystem("flush_graphic", "true");
				hWindowControlDisplayImage.Refresh();

				int sx = m_rect.Left;
				int sy = m_rect.Top;
				int ex = m_rect.Left + m_rect.Width -1;
				int ey = m_rect.Top + m_rect.Height - 1 ;

				HOperatorSet.SetColor(hWindowControlDisplayImage.HalconWindow, "green");
				HOperatorSet.SetDraw(hWindowControlDisplayImage.HalconWindow, "margin");		//塗りつぶさない
				HOperatorSet.DispRectangle1(hWindowControlDisplayImage.HalconWindow, sy, sx, ey, ex);

				HOperatorSet.SetColor(hWindowControlDisplayImage.HalconWindow, "yellow");
				HOperatorSet.DispLine(hWindowControlDisplayImage.HalconWindow, 0, WorkEdgePos, magImgH, WorkEdgePos);

				TextBoxRectDispSX.Text = sx.ToString();
				TextBoxRectDispSY.Text = sy.ToString();
				TextBoxRectDispEX.Text = ex.ToString();
				TextBoxRectDispEY.Text = ey.ToString();
				textBoxRectW.Text = (ex - sx + 1).ToString();
				textBoxRectH.Text = (ey - sy + 1).ToString();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void hWindowControlDisplayImage_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				int curImgX = (int)Math.Abs(e.X / m_dispMag);
				int curImgY = (int)Math.Abs(e.Y / m_dispMag);

				int imgW = m_rectLimit.Width;
				int imgH = m_rectLimit.Height;

				if (0 <= curImgX && 0 <= curImgY && curImgX < imgW && curImgY < imgH)
				{
					HTuple grayval;
					grayval = _imgSrc.GetGrayval(curImgY, curImgX);
					labelMousePos.Text = String.Format("({0},{1},{2})", curImgX, curImgY, grayval.D.ToString());
				}

				if (preCursorX != curImgX || preCursorY != curImgY)
				{

					if (m_isDrag)
					{
						if (curImgX < 0 || curImgY < 0 || imgW <= curImgX || imgH <= curImgY)
						{
							m_isDrag = false;
							hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.Default;
							m_dragHit = ClassRectangle.HitID.NONE;
						}
						else
						{
							UpdateRectangle(curImgX, curImgY, m_dragHit);
							SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
						}

					}
					else
					{

						m_dragHit = m_RUI.JudgeHitRectangle(curImgX, curImgY);

						if (m_dragHit != ClassRectangle.HitID.NONE)
						{
							switch (m_dragHit)
							{
								case ClassRectangle.HitID.CORNER_LU:
								case ClassRectangle.HitID.CORNER_RD:
									hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
									break;
								case ClassRectangle.HitID.CORNER_LD:
								case ClassRectangle.HitID.CORNER_RU:
									hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.SizeNESW;
									break;
								case ClassRectangle.HitID.SIDE_D:
								case ClassRectangle.HitID.SIDE_U:
									hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.SizeNS;
									break;
								case ClassRectangle.HitID.SIDE_L:
								case ClassRectangle.HitID.SIDE_R:
									hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.SizeWE;
									break;
								case ClassRectangle.HitID.CENTER_MOVE:
									hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.SizeAll;
									break;
							}
						}
						else
						{
							hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.Default;
						}
					}
				}

				preCursorX = curImgX;
				preCursorY = curImgY;
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		void UpdateRectangle( int imgPosX, int imgPosY, ClassRectangle.HitID hit ){
			int moveX, moveY;

			int rect_sx = m_rect.X;
			int rect_sy = m_rect.Y;
			int rect_ex = m_rect.X + m_rect.Width - 1;
			int rect_ey = m_rect.Y + m_rect.Height - 1;

			switch( hit ){
				case ClassRectangle.HitID.CORNER_LU:
					moveX = imgPosX - rect_sx;
					moveY = imgPosY - rect_sy;
					m_rect.X = imgPosX;
					m_rect.Y = imgPosY;
					m_rect.Width = m_rect.Width - moveX;
					m_rect.Height = m_rect.Height - moveY;
					break;
				case ClassRectangle.HitID.CORNER_RU:
					moveX = imgPosX - (rect_sx + m_rect.Width - 1);
					moveY = imgPosY - rect_sy;
					m_rect.Y = imgPosY;
					m_rect.Width = m_rect.Width + moveX;
					m_rect.Height = m_rect.Height - moveY;
					break;
				case ClassRectangle.HitID.CORNER_RD:
					moveX = imgPosX - (rect_sx + m_rect.Width - 1);
					moveY = imgPosY - (rect_sy + m_rect.Height - 1);
					m_rect.Width = m_rect.Width + moveX;
					m_rect.Height = m_rect.Height + moveY;
					break;
				case ClassRectangle.HitID.CORNER_LD:
					moveX = imgPosX -  rect_sx;
					moveY = imgPosY - (rect_sy + m_rect.Height - 1);
					m_rect.X = imgPosX;
					m_rect.Width = m_rect.Width - moveX;
					m_rect.Height = m_rect.Height + moveY;
					break;
				case ClassRectangle.HitID.SIDE_U:
					moveY = imgPosY - rect_sy;
					m_rect.Y = imgPosY;
					m_rect.Height = m_rect.Height - moveY;
					break;
				case ClassRectangle.HitID.SIDE_R:
					moveX = imgPosX - (rect_sx + m_rect.Width - 1);
					m_rect.Width = m_rect.Width + moveX;
					break;
				case ClassRectangle.HitID.SIDE_D:
					moveY = imgPosY - (rect_sy + m_rect.Height - 1);
					m_rect.Height = m_rect.Height + moveY;
					break;
				case ClassRectangle.HitID.SIDE_L:
					moveX = imgPosX - rect_sx;
					m_rect.X = imgPosX;
					m_rect.Width = m_rect.Width - moveX;
					break;
				case ClassRectangle.HitID.CENTER_MOVE:
					moveX = imgPosX - (rect_sx + m_rect.Width / 2);
					moveY = imgPosY - (rect_sy + m_rect.Height / 2);
					m_rect.X = rect_sx + moveX;
					m_rect.Y = rect_sy + moveY;
					break;
				default:
					break;
			}
			
			rect_sx = m_rect.X;
			rect_sy = m_rect.Y;
			rect_ex = m_rect.X + m_rect.Width - 1;
			rect_ey = m_rect.Y + m_rect.Height - 1;

			if (m_rect.Width < (int)ClassRectangle.NUM._MIN_RECT_W)
			{
				switch( hit ){
					case ClassRectangle.HitID.CORNER_LU:
					case ClassRectangle.HitID.CORNER_LD:
					case ClassRectangle.HitID.SIDE_L:
						m_rect.X = rect_ex - (int)ClassRectangle.NUM._MIN_RECT_W+1;
						m_rect.Width = (int)ClassRectangle.NUM._MIN_RECT_W;
						m_isDrag = false;
					break;
					case ClassRectangle.HitID.CORNER_RU:
					case ClassRectangle.HitID.CORNER_RD:
					case ClassRectangle.HitID.SIDE_R:
						m_rect.Width = (int)ClassRectangle.NUM._MIN_RECT_W;
						m_isDrag = false;
					break;
				}
			}

			if( m_rect.Height < (int)ClassRectangle.NUM._MIN_RECT_H ){
				switch ( hit ){
					case ClassRectangle.HitID.CORNER_LD:
					case ClassRectangle.HitID.CORNER_RD:
					case ClassRectangle.HitID.SIDE_D:
						m_rect.Height = (int)ClassRectangle.NUM._MIN_RECT_H;
						m_isDrag = false;
					break;
					case ClassRectangle.HitID.CORNER_RU:
					case ClassRectangle.HitID.CORNER_LU:
					case ClassRectangle.HitID.SIDE_U:
					m_rect.Y = rect_ey - (int)ClassRectangle.NUM._MIN_RECT_H + 1;
						m_rect.Height = (int)ClassRectangle.NUM._MIN_RECT_H;
						m_isDrag = false;
					break;
				}
			}

			rect_sx = m_rect.X;
			rect_sy = m_rect.Y;
			rect_ex = m_rect.X + m_rect.Width - 1;
			rect_ey = m_rect.Y + m_rect.Height - 1;
			
			int imgW = m_rectLimit.Width;
			int imgH = m_rectLimit.Height;
			
			if (m_rect.Left < 0)
			{
				m_rect.X = 0;
			}
			if( m_rect.Top < 0  ){
				m_rect.Y = 0;
			}
			if (imgW - 1 <= rect_ex)
			{
				m_rect.X -= rect_ex - (imgW - 1);
			}
			if (imgH - 1 <= rect_ey)
			{
				m_rect.Y -= rect_ey - (imgH - 1);
			}

			if( !m_isDrag ){
				hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.Default;
				m_dragHit = ClassRectangle.HitID.NONE;
			}
			
			m_RUI.Rect = m_rect;
            numAreaLeft.Value = (decimal)m_RUI.Rect.Left;
            numAreaRight.Value = (decimal)m_RUI.Rect.Right;
            numAreaTop.Value = (decimal)m_RUI.Rect.Top;
            numAreaBottom.Value = (decimal)m_RUI.Rect.Bottom;

        }

        private void hWindowControlDisplayImage_MouseDown(object sender, MouseEventArgs e)
		{
			if( m_dragHit != ClassRectangle.HitID.NONE ){
				m_isDrag = true;
			}
		}

		private void hWindowControlDisplayImage_MouseUp(object sender, MouseEventArgs e)
		{
			m_isDrag = false;
			m_dragHit = ClassRectangle.HitID.NONE;
			hWindowControlDisplayImage.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private void ButtonRectUp_Click(object sender, EventArgs e)
		{
			int adjAmount = GetAdjustAmount();

			m_rect.Y = m_rect.Y - adjAmount;
			if( RadioButtonAdjustIncri.Checked ){
				m_rect.Height = m_rect.Height + adjAmount;
			}
			
			AdjustRectangle();
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private void ButtonLeft_Click(object sender, EventArgs e)
		{
			if( m_rect.X == 0 ){
				return;
			}

			int adjAmount = GetAdjustAmount();
			m_rect.X = m_rect.X - adjAmount;
			if ( RadioButtonAdjustIncri.Checked ){
				m_rect.Width = m_rect.Width + adjAmount;
			}

			AdjustRectangle();
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private void ButtonRight_Click(object sender, EventArgs e)
		{
			int endX  = m_rect.X + m_rect.Width - 1;

			if(m_rectLimit.Width < endX ){
				return;
			}

			int adjAmount = GetAdjustAmount();

			if( RadioButtonAdjustPosition.Checked ){
				m_rect.X = m_rect.X + adjAmount;
			}else{
				m_rect.Width = m_rect.Width + adjAmount;
			}

			AdjustRectangle();
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private void ButtonDown_Click(object sender, EventArgs e)
		{
			int endY = m_rect.Y + m_rect.Height - 1;

			if(m_rectLimit.Height < endY ){
				return;
			}

			int adjAmount = GetAdjustAmount();
			
			if (RadioButtonAdjustPosition.Checked)
			{
				m_rect.Y = m_rect.Y + adjAmount;
			}else{
				m_rect.Height = m_rect.Height + adjAmount;
			}

			AdjustRectangle();
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private int GetAdjustAmount() {
			return int.Parse(comboBoxAdjustAmount.Items[comboBoxAdjustAmount.SelectedIndex].ToString());
		}
		
		void AdjustRectangle()
		{
			int rect_sx = m_rect.X;
			int rect_sy = m_rect.Y;
			int rect_ex = m_rect.X + m_rect.Width - 1;
			int rect_ey = m_rect.Y + m_rect.Height - 1;

			if (RadioButtonAdjustPosition.Checked)
			{
				if (rect_sx < m_rectLimit.X)
				{
					m_rect.X = m_rectLimit.X;
				}
				else if (m_rectLimit.Width - 1 <= rect_ex)
				{
					m_rect.X -= rect_ex - (m_rectLimit.Width - 1);
				}
				if (rect_sy < m_rectLimit.Y)
				{
					m_rect.Y = m_rectLimit.Y;
				}
				else if (m_rectLimit.Height - 1 <= rect_ey)
				{
					m_rect.Y -= rect_ey - (m_rectLimit.Height - 1);
				}
			}
			else
			{
				if (rect_sx < m_rectLimit.X)
				{
					m_rect.X = m_rectLimit.X;
				}
				if (m_rectLimit.Width - 1 <= rect_ex)
				{
					m_rect.Width = (m_rectLimit.Width - 1) - m_rect.X + 1;
				}
				if (rect_sy < m_rectLimit.Y)
				{
					m_rect.Y = m_rectLimit.Y;
				}
				if (m_rectLimit.Height - 1 <= rect_ey)
				{
					m_rect.Height = (m_rectLimit.Height - 1) - m_rect.Y + 1;
				}
			}
			
			m_RUI.Rect = m_rect;
            numAreaLeft.Value = (decimal)m_RUI.Rect.Left;
            numAreaRight.Value = (decimal)m_RUI.Rect.Right;
            numAreaTop.Value = (decimal)m_RUI.Rect.Top;
            numAreaBottom.Value = (decimal)m_RUI.Rect.Bottom;
        }

        private void checkBoxFitImage_CheckedChanged(object sender, EventArgs e)
		{
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private void buttonLoadImage_Click(object sender, EventArgs e)
		{
			//OpenFileDialog
			OpenFileDialog ofd = new OpenFileDialog();

			//初期パラメータ
			ofd.FileName = "";
			ofd.InitialDirectory = m_initialDirectory;
			ofd.Filter = "BMPファイル(*.bmp)|*.bmp|JPEGファイル(*.jpg;*.jpeg)|*.jpg;*.jpeg";
			ofd.Filter = "BMPファイル(*.bmp)|*.bmp";

			ofd.FilterIndex = 0;
			ofd.Title = "画像ファイルの選択";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;

			//ダイアログ表示
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//OKボタン
				m_initialDirectory = ofd.FileName;

				//画像ファイル読み込み
				HImage image = new HImage();
				image.ReadImage(ofd.FileName);
				SetImage(image);

				SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
			}

		}

		private void panelDisplayImage_Resize(object sender, EventArgs e)
		{
			SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			//if (DialogResult.OK != MessageBox.Show("領域を決定して終了しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
			//{
			//	return;
			//}
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			//if (DialogResult.OK != MessageBox.Show("領域を決定せずに終了しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
			//{
			//	return;
			//}
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void butAutoAdjustTry_Click(object sender, EventArgs e)
		{
			AutoAdjust();
		}

		private bool AutoAdjust()
		{
			PointDatas edgedata;
			ErrorCode ret = ClassEdgeSearch.MeasureBoxEdge(_imgSrc, EdgeParam.EdgeSearchParam, out edgedata);
			if (ret == ErrorCode.Success && edgedata.pnts.Count > 0)
			{
				edgedata.CalcMean();
				Rectangle area = m_rect;
				if (ControlID == ViewID.ViewRear)
				{
					int right = (int)edgedata.meanX - EdgeParam.AreaOffset;
					m_rect.Width = (int)right - m_rect.X;
					//int right = m_rect.Right;
					//m_rect.X = (int)edgedata.meanX + EdgeParam.AreaOffset;
					//m_rect.Width = right - m_rect.X;
				}
				else
				{
					int right = m_rect.Right;
					m_rect.X = (int)edgedata.meanX + EdgeParam.AreaOffset;
					m_rect.Width = right - m_rect.X;
					//int right = (int)edgedata.meanX - EdgeParam.AreaOffset;
					//m_rect.Width = (int)right - m_rect.X;
				}
				AdjustRectangle();
				SetDisplayImage(_imgSrc, checkBoxFitImage.Checked);
				edgedata.Draw(hWindowControlDisplayImage.HalconWindow);

				labelError.Visible = false;
			}
			else
			{
				labelError.Text = $"エッジ検出エラー";
				labelError.Visible = true;
			}

			return true;
		}
	}
}
