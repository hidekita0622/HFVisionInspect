using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFVisionInspect
{
	public partial class FormAutoExposure : Form
	{
		public AutoExposureParam param {  get; set; }

		public FormAutoExposure()
		{
			InitializeComponent();

			param = new AutoExposureParam();
		}
		private void FormAutoExposure_Load(object sender, EventArgs e)
		{
			SetParameter();
			groupAutoSet.Enabled = checkAutoEnable.Checked;
			//numExposureTime.Enabled= !checkAutoEnable.Checked;
		}

		private void GetParameter()
		{
			param.TargetGrayLevel = (double)numTargetGraylevel.Value;
			param.TargetGrayRange = (double)numTargetRange.Value;
			param.AutoExposureTimeLow = (int)numExposuTimeLow.Value;
			param.AutoExposureTimeHigh = (int)numExposuTimeHigh.Value;
		}

		private void SetParameter()
		{
			numTargetGraylevel.Value = (decimal)param.TargetGrayLevel;
			numTargetRange.Value = (decimal)param.TargetGrayRange;
			numExposuTimeLow.Value = (decimal)param.AutoExposureTimeLow;
			numExposuTimeHigh.Value = (decimal)param.AutoExposureTimeHigh;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			GetParameter();
		}

		private void checkAutoEnable_CheckedChanged(object sender, EventArgs e)
		{
			groupAutoSet.Enabled = checkAutoEnable.Checked;
			//numExposureTime.Enabled = !checkAutoEnable.Checked;
		}
	}
}
 