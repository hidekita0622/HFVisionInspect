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


namespace HFVisionInspect
{
	public partial class FormSetDotInspect : Form
	{
		public BlobParameter DotInspparam = new BlobParameter();
		public HObject HoProcImg;
		public int WorkEdgePos;
		public int BaseEdgePos = 0;
        public bool InspColorBlack { get; set; } = true;

		public FormSetDotInspect()
		{
			InitializeComponent();
		}

		private void FormSetBlackDot_Load(object sender, EventArgs e)
		{

            SetParameter();
            if (InspColorBlack)
            {
                this.Text = "黒点検査設定";
            }
            else
            {
                this.Text = "白点検査設定";
            }
		}

		void SetParameter()
		{
            checkEnabled.Checked = DotInspparam.ProcEnable;
			if (DotInspparam.BinaryParam.BinaryMode == ImageBinaryMode.BINARY_SIMPLE)
			{
				radioBinarySimple.Checked = true;
				radioBinarySauvola.Checked = false;
                radioBinaryVar.Checked = false;
            }
            else if (DotInspparam.BinaryParam.BinaryMode == ImageBinaryMode.BINARY_SAUVOLA)
			{
				radioBinarySimple.Checked = false;
				radioBinarySauvola.Checked = true;
                radioBinaryVar.Checked = false;
            }
            else
            {
                radioBinarySimple.Checked = false;
                radioBinarySauvola.Checked = false;
                radioBinaryVar.Checked = true;
            }
            if (DotInspparam.BinaryParam.BinaryColor == ImageBinaryColor.BINARY_DARK)
            {
                radioBinDark.Checked = true;
                radioBinLight.Checked = false;
                radioBinBoth.Checked = false;
            }
            else if (DotInspparam.BinaryParam.BinaryColor == ImageBinaryColor.BINARY_LIGHT)
            {
                radioBinDark.Checked = false;
                radioBinLight.Checked = true;
                radioBinBoth.Checked = false;
            }
            else
            {
                radioBinDark.Checked = false;
                radioBinLight.Checked = false;
                radioBinBoth.Checked = true;
            }


            numBinaryThresholdLo.Value = (decimal)DotInspparam.BinaryParam.ThresholdLo;
            numBinaryThresholdHi.Value = (decimal)DotInspparam.BinaryParam.ThresholdHi;
            numBlackDotSauvolaMaskSize.Value = (decimal)DotInspparam.BinaryParam.SauvolaMaskSize;
			numBlackDotSauvolaScale.Value = (decimal)DotInspparam.BinaryParam.SauvolaScale;
            numVarThresholdMaskWidth.Value = (decimal)DotInspparam.BinaryParam.VarMaskWidth;
            numVarThresholdMaskHeight.Value = (decimal)DotInspparam.BinaryParam.VarMaskHeight;
            numVarThresholdDevScale.Value = (decimal)DotInspparam.BinaryParam.VarDevScale;
            numVarThresholdAbsThreshold.Value = (decimal)DotInspparam.BinaryParam.VarThreshold;

            checkBinaryConvex.Checked = DotInspparam.ConvexEnabled;
            checkBinaryFillup.Checked = DotInspparam.FillupEnabled;

            checkJudgeEnable0.Checked = DotInspparam.JudgeParam[0].Enable;
            numMinArea0.Value = DotInspparam.JudgeParam[0].MinArea;
            numMaxArea0.Value = DotInspparam.JudgeParam[0].MaxArea;
            numMinGray0.Value = DotInspparam.JudgeParam[0].MinGray;
            numMaxGray0.Value = DotInspparam.JudgeParam[0].MaxGray;

            checkJudgeEnable1.Checked = DotInspparam.JudgeParam[1].Enable;
            numMinArea1.Value = DotInspparam.JudgeParam[1].MinArea;
            numMaxArea1.Value = DotInspparam.JudgeParam[1].MaxArea;
            numMinGray1.Value = DotInspparam.JudgeParam[1].MinGray;
            numMaxGray1.Value = DotInspparam.JudgeParam[1].MaxGray;

            checkJudgeEnable2.Checked = DotInspparam.JudgeParam[2].Enable;
            numMinArea2.Value = DotInspparam.JudgeParam[2].MinArea;
			numMaxArea2.Value = DotInspparam.JudgeParam[2].MaxArea;
			numMinGray2.Value = DotInspparam.JudgeParam[2].MinGray;
			numMaxGray2.Value = DotInspparam.JudgeParam[2].MaxGray;
        }

        void GetParameter()
		{

            DotInspparam.ProcEnable = checkEnabled.Checked;
            if (radioBinarySimple.Checked)
				DotInspparam.BinaryParam.BinaryMode = ImageBinaryMode.BINARY_SIMPLE;
			else if (radioBinarySauvola.Checked)
				DotInspparam.BinaryParam.BinaryMode = ImageBinaryMode.BINARY_SAUVOLA;
            else
                DotInspparam.BinaryParam.BinaryMode = ImageBinaryMode.BINARY_VAR;
            if (radioBinDark.Checked)
                DotInspparam.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_DARK;
            else if (radioBinLight.Checked)
                DotInspparam.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_LIGHT;
            else
                DotInspparam.BinaryParam.BinaryColor = ImageBinaryColor.BINARY_UNION;

            DotInspparam.BinaryParam.ThresholdLo = (int)numBinaryThresholdLo.Value;
            DotInspparam.BinaryParam.ThresholdHi = (int)numBinaryThresholdHi.Value;
            DotInspparam.BinaryParam.SauvolaMaskSize = (double)numBlackDotSauvolaMaskSize.Value;
			DotInspparam.BinaryParam.SauvolaScale = (double)numBlackDotSauvolaScale.Value;
            DotInspparam.BinaryParam.VarMaskWidth = (int)numVarThresholdMaskWidth.Value;
            DotInspparam.BinaryParam.VarMaskHeight = (int)numVarThresholdMaskHeight.Value;
            DotInspparam.BinaryParam.VarDevScale = (double)numVarThresholdDevScale.Value;
            DotInspparam.BinaryParam.VarThreshold = (double)numVarThresholdAbsThreshold.Value;

            DotInspparam.ConvexEnabled = (bool)checkBinaryConvex.Checked;
            DotInspparam.FillupEnabled = (bool)checkBinaryFillup.Checked;

            DotInspparam.JudgeParam[0].Enable = checkJudgeEnable0.Checked;
            DotInspparam.JudgeParam[0].MinArea = (int)numMinArea0.Value;
            DotInspparam.JudgeParam[0].MaxArea = (int)numMaxArea0.Value;
            DotInspparam.JudgeParam[0].MinGray = (int)numMinGray0.Value;
            DotInspparam.JudgeParam[0].MaxGray = (int)numMaxGray0.Value;

            DotInspparam.JudgeParam[1].Enable = checkJudgeEnable1.Checked;
            DotInspparam.JudgeParam[1].MinArea = (int)numMinArea1.Value;
            DotInspparam.JudgeParam[1].MaxArea = (int)numMaxArea1.Value;
            DotInspparam.JudgeParam[1].MinGray = (int)numMinGray1.Value;
            DotInspparam.JudgeParam[1].MaxGray = (int)numMaxGray1.Value;

            DotInspparam.JudgeParam[2].Enable = checkJudgeEnable2.Checked;
            DotInspparam.JudgeParam[2].MinArea = (int)numMinArea2.Value;
			DotInspparam.JudgeParam[2].MaxArea = (int)numMaxArea2.Value;
			DotInspparam.JudgeParam[2].MinGray = (int)numMinGray2.Value;
			DotInspparam.JudgeParam[2].MaxGray = (int)numMaxGray2.Value;

        }

        private void btnOk_Click(object sender, EventArgs e)
		{
			GetParameter();
		}

        private void groupBlobSelect_Enter(object sender, EventArgs e)
        {

        }

        private void radioBinarySimple_CheckedChanged(object sender, EventArgs e)
        {
            selectBinaryLightDark();
        }

        private void selectBinaryLightDark()
        {
            groupBinaryColor.Enabled =radioBinarySauvola.Checked;
        }

        private void radioBinarySauvola_CheckedChanged(object sender, EventArgs e)
        {
            selectBinaryLightDark();
        }

        private void radioBinaryVar_CheckedChanged(object sender, EventArgs e)
        {
            selectBinaryLightDark();
        }

        private void butSetMorphology_Click(object sender, EventArgs e)
        {
            FormSetMorphology frmMorphology = new FormSetMorphology();
            frmMorphology.Param.CopyFrom(DotInspparam.MorphologyParam);
            if (frmMorphology.ShowDialog() == DialogResult.OK)
            {
                DotInspparam.MorphologyParam.CopyFrom(frmMorphology.Param);
            }

        }

 
        private void butSetMorphology1_Click(object sender, EventArgs e)
        {
            FormSetMorphology frmMorphology = new FormSetMorphology();
            frmMorphology.Param.CopyFrom(DotInspparam.MorphologyParam);
            if (frmMorphology.ShowDialog() == DialogResult.OK)
            {
                DotInspparam.MorphologyParam.CopyFrom(frmMorphology.Param);
            }

        }
    }
}
