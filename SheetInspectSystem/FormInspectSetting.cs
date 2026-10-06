using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Diagnostics;

namespace HFVisionInspect
{
    public partial class FormInspectSetting : Form
    {
        private enum InspectItem
        {
            DarkBlob = 0,
            LightBlob,
        }

        public InspectParameter InspParam { get; set; }
        public HImage ImgSrc = new HImage();
        private InspectControl _inspCtrl;
        bool _executeTryal = false;
        //FormInspectResult FormResult = null;
        private Point MousePos = new Point(10, 10);
        private HImage ImgDisp = new HImage();
        public double CameraResolution;
        public ViewID SetControlID = ViewID.ViewFront;
        private bool _mouseKeep = false;

        public HImage ImgDiffBase;

        public FormInspectSetting()
        {
            InitializeComponent();

            InspParam = new InspectParameter();
            ImgSrc.GenEmptyObj();
            _inspCtrl = new InspectControl();
            this.MouseWheel += hSmartWinMouseWheel;
            ImgDisp.GenEmptyObj();

			ImgDiffBase = new HImage();
		}

        private void FormInspectSetting_Load(object sender, EventArgs e)
        {
            ImgDisp = ImgSrc.CopyImage();
            SetParameter();
            DispImage();

        }
        private void hSmartWinMouseWheel(object sender, MouseEventArgs e)
        {
            Point pt = hCamImage.Location;
            MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
            hCamImage.HSmartWindowControl_MouseWheel(sender, newe);
        }

        void DispImageSelect()
        {
            ImgDisp.Dispose();
            if (radioShowImgProc.Checked)
            {
                hCamImage.HalconWindow.DispImage(_inspCtrl.ImgProc);
                ImgDisp = _inspCtrl.ImgProc.CopyImage();
            }
			else if (radioShowImgBase.Checked)
			{
				hCamImage.HalconWindow.DispImage(_inspCtrl.ImgDiffBase);
				//ImgDisp = _inspCtrl.ImgDiffBase.CopyImage();
				ImgDisp = ImgDiffBase.CopyImage();
			}
			else
			{
                hCamImage.HalconWindow.DispImage(ImgSrc);
                ImgDisp = ImgSrc.CopyImage();
            }
            //int w, h;
            //ImgDisp.GetImageSize(out w, out h);
            //hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
            //hCamImage.SetFullImagePart();
        }

        private void DispImage()
        {
            try
            {
                hCamImage.HalconWindow.ClearWindow();
                if (!ImgSrc.IsInitialized())
                    return;
                int w, h;
                ImgSrc.GetImageSize(out w, out h);
                if (_executeTryal == false)
                {
                    hCamImage.HalconWindow.DispImage(ImgSrc);
                    //hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
                    hCamImage.SetFullImagePart();

                }
                else
                {
                    DispImageSelect();
                    if (checkBinaryImg.Checked)
                    {
                        hCamImage.HalconWindow.SetColor("#00ff0080");
                        hCamImage.HalconWindow.SetDraw("fill");
                        _inspCtrl.DrawRegion(hCamImage.HalconWindow, enShowRegion.Binary);
                    }
                    if (checkShowResult.Checked)
                    {
                        //hCamImage.HalconWindow.SetColor("red");
                        //hCamImage.HalconWindow.SetDraw("fill");
                        _inspCtrl.DrawBlobs(hCamImage.HalconWindow);
                    }
                    ImgDisp.GetImageSize(out w, out h);
                }
                hCamImage.HalconWindow.SetDraw("margin");
                hCamImage.HalconWindow.SetColor("maroon");
                hCamImage.HalconWindow.DispRectangle1(0, 0, (double)h - 1, (double)w - 1);

                if (checkShowArea.Checked)
                {
                    hCamImage.HalconWindow.SetColor("cyan");
                    int left, top, right, bottom;
//                    if (radioShowImgSrc.Checked)
                    {
                        if (SetControlID == ViewID.ViewRear)
                        {
                            left = InspParam.InspAreaRight.Left;
                            right = InspParam.InspAreaRight.Right;
                            top = InspParam.InspAreaRight.Top;
                            bottom = InspParam.InspAreaRight.Bottom;
                        }
                        else
                        {
                            left = InspParam.InspAreaLeft.Left;
                            right = InspParam.InspAreaLeft.Right;
                            top = InspParam.InspAreaLeft.Top;
                            bottom = InspParam.InspAreaLeft.Bottom;
                        }
                    }
                    //else
                    //{
                    //    left = _inspCtrl.ImageProcArea.X;
                    //    right = _inspCtrl.ImageProcArea.Right;
                    //    top = _inspCtrl.ImageProcArea.Y;
                    //    bottom = _inspCtrl.ImageProcArea.Bottom;
                    //}
                    hCamImage.HalconWindow.SetDraw("margin");
                    hCamImage.HalconWindow.DispRectangle1((double)top, (double)left, (double)bottom, (double)right);
                }
                //hCamImage.Refresh();
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
            }
        }

        private void FormInspectSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ImgSrc != null)
                ImgSrc.Dispose();

            if (ImgDisp != null)
                ImgDisp.Dispose();
            _inspCtrl.Dispose();
        }

        private void btGetProcArea_Click(object sender, EventArgs e)
        {
            try
            {
				FormGetArea frmGetArea = new FormGetArea();
				Rectangle rect = InspParam.InspAreaRight;
				if (SetControlID == ViewID.ViewRear)
				{
					rect = InspParam.InspAreaRight;
					frmGetArea.EdgeParam.CopyFrom(InspParam.EdgeParamRight);
				}
				else
				{
					rect = InspParam.InspAreaLeft;
					frmGetArea.EdgeParam.CopyFrom(InspParam.EdgeParamLeft);
				}
				int w, h;
				ImgSrc.GetImageSize(out w, out h);
				if (rect.X < 0) rect.X = 0;
				if (rect.Y < 0) rect.Y = 0;
				if (rect.Right > w) rect.Width = w - rect.X;
				if (rect.Bottom > w) rect.Height = h - rect.Y;
				frmGetArea.Rect = rect;
				frmGetArea.SetImage(ImgSrc);
				frmGetArea.Text = "検査範囲設定";
				frmGetArea.ControlID = SetControlID;

				frmGetArea.ShowWorkEdgePos = false;
				if (frmGetArea.ShowDialog() == DialogResult.OK)
				{
					rect = frmGetArea.Rect;
					if (SetControlID == ViewID.ViewRear)
					{
						InspParam.InspAreaRight = rect;
						InspParam.EdgeParamRight.CopyFrom(frmGetArea.EdgeParam);
					}
					else
					{
						InspParam.InspAreaLeft = rect;
						InspParam.EdgeParamLeft.CopyFrom(frmGetArea.EdgeParam);
					}
					DispImage();
				}
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}

        }

        private void checkShowArea_CheckedChanged(object sender, EventArgs e)
        {
            DispImage();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            InspParam.RecipeName = textRecipeName.Text;
            GetParameter();
        }

        private void butSetDotInspect_Click(object sender, EventArgs e)
        {
            //FormSetDotInspect frmDotInsp = new FormSetDotInspect();
            //frmDotInsp.DotInspparam.CopyFrom(InspParam.DarkBlob);
            //frmDotInsp.InspColorBlack = true;
            //if (frmDotInsp.ShowDialog() == DialogResult.OK)
            //{
            //    InspParam.DarkBlob.CopyFrom(frmDotInsp.DotInspparam);
            //}
        }

        private void butProcTrial_Click(object sender, EventArgs e)
        {
            if (ImgSrc != null)
            {
                enInspectItem item;
                item = enInspectItem.RULE_INSPECT;
                GetParameter();

                _inspCtrl.ControlID = SetControlID;
                _inspCtrl.InspParam.CopyFrom(InspParam);
				_inspCtrl.RecordDiffBase(ImgDiffBase);

				if (_inspCtrl.BlobInspection(ImgSrc, item) == ErrorCode.None)
                {
                    //groupImage.Enabled = checkDifferenceImage.Checked;
                    //radioShowImgProc.Checked = true;
                    _executeTryal = true;
                    DispImage();
                    int w, h;
                    ImgDisp.GetImageSize(out w, out h);
                    //hCamImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
                    hCamImage.SetFullImagePart();


					_inspCtrl.DispBlobList(listDefect);
                }
            }

        }


        private void checkBinaryImg_CheckedChanged(object sender, EventArgs e)
        {
            DispImage();
        }

        private void checkShowResult_CheckedChanged(object sender, EventArgs e)
        {
            DispImage();
        }


        private void FormInspectSetting_Activated(object sender, EventArgs e)
        {
            textRecipeName.Text = InspParam.RecipeName;
        }

        private void hCamImage_HMouseDoubleClick(object sender, HMouseEventArgs e)
        {
            DispImage();
        }

        private void hCamImage_HMouseWheel(object sender, HMouseEventArgs e)
        {
            DispImage();
			if (checkShowProfile.Checked)
				ShowProfile(MousePos.X, MousePos.Y);
		}

		private void hCamImage_HMouseMove(object sender, HMouseEventArgs e)
        {
            double dispMag = 1;
            try
            {
                if (_mouseKeep) return;

				DispImage();
				int curImgX = (int)(e.X / dispMag);
                int curImgY = (int)(e.Y / dispMag);
                MousePos.X = curImgX;
                MousePos.Y = curImgY;
                int w, h;
                ImgDisp.GetImageSize(out w, out h);
                if (curImgX < 0 || curImgY < 0 || curImgX >= w || curImgY >= h)
                    return;
                string sloc;
                HTuple grayval;
                grayval = ImgDisp.GetGrayval(MousePos.Y, MousePos.X);
                sloc = string.Format("( {0}, {1}, {2} )", MousePos.X.ToString(), MousePos.Y.ToString(), grayval.D.ToString());
                labelLocate.Text = sloc;

                if (checkShowProfile.Checked)
                {
                    ShowProfile(MousePos.X, MousePos.Y);
				}
				//hCamImage.Refresh();
			}
			catch (HalconException ex)
            {
                Helpers.WriteLog($"hCamImage_HMouseMove Exception : {ex.Message}");
            }

        }

		private void hCamImage_HMouseUp(object sender, HMouseEventArgs e)
		{
            _mouseKeep = !_mouseKeep;
		}

		/// <summary>
		/// A-Scope(プロファイル波形表示）
		/// </summary>
		/// <param name="posX"></param>
		/// <param name="posY"></param>
		private void ShowProfile(int posX, int posY)
        {
			int w, h;
			ImgDisp.GetImageSize(out w, out h);
            if (posX < 0 || posY < 0 || posX >= w || posY >= h)
                return;

			hCamImage.HalconWindow.SetColor("coral");

			HTuple grayval;
			// X方向プロファイル
			double yOff = (double)h - 10;
			int x, y = posY;
			//double dx = 0;
			grayval = ImgDisp.GetGrayval(y, 0);
			double dy0 = yOff - grayval.D / 2, dy;
			HTuple rows = new HTuple(), cols = new HTuple();
			for (x = 1; x < w; x++)
			{
				grayval = ImgDisp.GetGrayval(y, x);
				dy = yOff - grayval.D / 2;
				rows.Append(dy);
				cols.Append(x);

				//hCamImage.HalconWindow.DispLine(dy0, x - 1, dy, x);
				//dy0 = dy;
			}
			hCamImage.HalconWindow.DispPolygon(rows, cols);

			// Y方向プロファイル
			//double xOff = 10;
			//x = posX;
			//dy = 0;
			//grayval = ImgSrc.GetGrayval(0, x);
			//double dx0 = yOff - grayval.D / 2;
			//for (y = 1; y < h; y++)
			//{
			//    grayval = ImgSrc.GetGrayval(y, x);
			//    dx = grayval.D / 2;
			//    hCamImage.HalconWindow.DispLine(y - 1, dx0, y, dx);
			//    dx0 = dx;
			//}

			// Min,Max線表示
			hCamImage.HalconWindow.SetColor("#ff7f50a0");
			HTuple style = new HTuple(4, 2);
			hCamImage.HalconWindow.SetLineStyle(style);
			hCamImage.HalconWindow.DispLine(yOff, 0, yOff, w);
			hCamImage.HalconWindow.DispLine(yOff - 128, 0, yOff - 128, w);
			//hCamImage.HalconWindow.DispLine(0, xOff, h, xOff);
			//hCamImage.HalconWindow.DispLine(0, xOff + 128, h, xOff + 128);

			// 中心線表示
			style = new HTuple();
			hCamImage.HalconWindow.SetLineStyle(style);
			hCamImage.HalconWindow.SetColor("yellow");
			hCamImage.HalconWindow.DispLine((double)posY, 0, (double)posY, w);
			hCamImage.HalconWindow.DispLine(0, (double)posX, h, (double)posX);
		}

        void SetParameter()
        {
            BlobParameter dotInspect = InspParam.DarkBlob;
            if (dotInspect.BinaryParam.BinaryMode == ImageBinaryMode.BINARY_SIMPLE)
            {
                radioBinarySimple.Checked = true;
                radioBinarySauvola.Checked = false;
                //radioBinaryVar.Checked = false;
            }
            else if (dotInspect.BinaryParam.BinaryMode == ImageBinaryMode.BINARY_SAUVOLA)
            {
                radioBinarySimple.Checked = false;
                radioBinarySauvola.Checked = true;
                //radioBinaryVar.Checked = false;
            }
            else
            {
                //radioBinarySimple.Checked = false;
                //radioBinarySauvola.Checked = false;
                //radioBinaryVar.Checked = true;
            }
            if (dotInspect.BinaryParam.BinaryColor == ImageBinaryColor.BINARY_DARK)
            {
                radioBinDark.Checked = true;
                radioBinLight.Checked = false;
                //radioBinBoth.Checked = false;
            }
            else if (dotInspect.BinaryParam.BinaryColor == ImageBinaryColor.BINARY_LIGHT)
            {
                radioBinDark.Checked = false;
                radioBinLight.Checked = true;
                //radioBinBoth.Checked = false;
            }
            else
            {
                //radioBinDark.Checked = false;
                //radioBinLight.Checked = false;
                //radioBinBoth.Checked = true;
            }

   //         if (SetControlID == ViewID.ViewLeft)
   //         {
   //             numExposureTime.Value = (int)InspParam.ExposureTimeLeft;
   //         }
   //         else
   //         {
			//	numExposureTime.Value = (int)InspParam.ExposureTimeRight;
			//}

			checkGaussEnable.Checked = dotInspect.GaussEnable;
			numGaussAlpha.Value = (decimal)dotInspect.GaussAlpha;
			
            numBinaryThresholdLo.Value = (decimal)dotInspect.BinaryParam.ThresholdLo;
            numBinaryThresholdHi.Value = (decimal)dotInspect.BinaryParam.ThresholdHi;
            numBlackDotSauvolaMaskSize.Value = (decimal)dotInspect.BinaryParam.SauvolaMaskSize;
            numBlackDotSauvolaScale.Value = (decimal)dotInspect.BinaryParam.SauvolaScale;

            //checkBinaryConvex.Checked = dotInspect.ConvexEnabled;
            //checkBinaryFillup.Checked = dotInspect.FillupEnabled;

            userDefectLevel0.Param.CopyFrom(dotInspect.JudgeParam[0]);
            userDefectLevel0.SetParam(CameraResolution);
            userDefectLevel1.Param.CopyFrom(dotInspect.JudgeParam[1]);
            userDefectLevel1.SetParam(CameraResolution);
            userDefectLevel2.Param.CopyFrom(dotInspect.JudgeParam[2]);
            userDefectLevel2.SetParam(CameraResolution);
            userDefectLevel3.Param.CopyFrom(dotInspect.JudgeParam[3]);
            userDefectLevel3.SetParam(CameraResolution);
            userDefectLevel4.Param.CopyFrom(dotInspect.JudgeParam[4]);
            userDefectLevel4.SetParam(CameraResolution);
        }

        void GetParameter()
        {
            InspParam.DarkBlob.GaussEnable = checkGaussEnable.Checked;
            InspParam.DarkBlob.GaussAlpha = (int)numGaussAlpha.Value;
			
            InspParam.DarkBlob.ProcEnable = true;
            if (radioBinarySimple.Checked)
                InspParam.DarkBlob.BinaryParam.BinaryMode = ImageBinaryMode.BINARY_SIMPLE;
            else
                InspParam.DarkBlob.BinaryParam.BinaryMode = ImageBinaryMode.BINARY_SAUVOLA;

            if (radioBinDark.Checked)
                InspParam.DarkBlob.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_DARK;
            else
                InspParam.DarkBlob.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_LIGHT;

            InspParam.DarkBlob.BinaryParam.ThresholdLo = (int)numBinaryThresholdLo.Value;
            InspParam.DarkBlob.BinaryParam.ThresholdHi = (int)numBinaryThresholdHi.Value;
            InspParam.DarkBlob.BinaryParam.SauvolaMaskSize = (double)numBlackDotSauvolaMaskSize.Value;
            InspParam.DarkBlob.BinaryParam.SauvolaScale = (double)numBlackDotSauvolaScale.Value;

            //InspParam.DarkBlob.ConvexEnabled = (bool)checkBinaryConvex.Checked;
            //InspParam.DarkBlob.FillupEnabled = (bool)checkBinaryFillup.Checked;

            userDefectLevel0.GetParam();
            InspParam.DarkBlob.JudgeParam[0].CopyFrom(userDefectLevel0.Param);
            userDefectLevel1.GetParam();
            InspParam.DarkBlob.JudgeParam[1].CopyFrom(userDefectLevel1.Param);
            userDefectLevel2.GetParam();
            InspParam.DarkBlob.JudgeParam[2].CopyFrom(userDefectLevel2.Param);
            userDefectLevel3.GetParam();
            InspParam.DarkBlob.JudgeParam[3].CopyFrom(userDefectLevel3.Param);
            userDefectLevel4.GetParam();
            InspParam.DarkBlob.JudgeParam[4].CopyFrom(userDefectLevel4.Param);

			//if (SetControlID == ViewID.ViewLeft)
			//{
			//	InspParam.ExposureTimeLeft = (int)numExposureTime.Value;
			//}
			//else
			//{
			//	InspParam.ExposureTimeRight = (int)numExposureTime.Value;
			//}

		}

		private void buttonMorphology_Click(object sender, EventArgs e)
        {
            FormSetMorphology frmMorphology = new FormSetMorphology();
            frmMorphology.Param.CopyFrom(InspParam.DarkBlob.MorphologyParam);
            if (frmMorphology.ShowDialog() == DialogResult.OK)
            {
                InspParam.DarkBlob.MorphologyParam.CopyFrom(frmMorphology.Param);
            }
        }

        private void radioShowImgSrc_CheckedChanged(object sender, EventArgs e)
        {
            DispImage();
        }

        private void radioShowImgProc_CheckedChanged(object sender, EventArgs e)
        {
            DispImage();
        }

		private void checkDifferenceImage_CheckedChanged(object sender, EventArgs e)
		{
			//groupImage.Enabled = checkDifferenceImage.Checked;
		}

		private void checkShowProfile_CheckedChanged(object sender, EventArgs e)
		{
			if (checkShowProfile.Checked)
				ShowProfile(MousePos.X, MousePos.Y);
		}

		private void butRecordBase_Click(object sender, EventArgs e)
		{
            _inspCtrl.RecordDiffBase(ImgSrc);
		}

		private void btExposureSet_Click(object sender, EventArgs e)
		{
            FormAutoExposure frmAutoExposure = new FormAutoExposure();
			if (SetControlID == ViewID.ViewFront)
			{
				frmAutoExposure.param.CopyFrom(InspParam.AutoExposureLeft);
			}
			else
			{
				frmAutoExposure.param.CopyFrom(InspParam.AutoExposureRight);
			}
            if (frmAutoExposure.ShowDialog() == DialogResult.OK)
            {
				if (SetControlID == ViewID.ViewFront)
					InspParam.AutoExposureLeft.CopyFrom(frmAutoExposure.param);
                else
					InspParam.AutoExposureRight.CopyFrom(frmAutoExposure.param);
			}

		}
	}
}
