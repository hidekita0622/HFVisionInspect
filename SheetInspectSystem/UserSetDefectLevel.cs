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
    public partial class UserSetDefectLevel : UserControl
    {
        private  double _cameraResolution;
        public BlobJudgeParam Param { get; set; }
        public UserSetDefectLevel()
        {
            InitializeComponent();
            Param = new BlobJudgeParam();
        }

        public void SetParam(double resolution)
        {
            _cameraResolution = resolution;
            checkJudgeEnable.Checked = Param.Enable;
            numMinArea.Value = (decimal)Param.MinArea;
            numMaxArea.Value = (decimal)Param.MaxArea;
            numMinGray.Value = (decimal)Param.MinGray;
            numMaxGray.Value = (decimal)Param.MaxGray;
            checkJudgeNG.Checked = Param.NGJudgeEnable;

            int value = (int)numMinArea.Value;
            double area = value * _cameraResolution * _cameraResolution;
            labelMinAreaMM.Text = area.ToString("0.00");
            value = (int)numMaxArea.Value;
            area = value * _cameraResolution * _cameraResolution;
            labelMaxAreaMM.Text = area.ToString("0.00");
        }

        public void GetParam()
        {
            Param.Enable = checkJudgeEnable.Checked;
            Param.MinArea = (int)numMinArea.Value;
            Param.MaxArea = (int)numMaxArea.Value;
            Param.MinGray = (int)numMinGray.Value;
            Param.MaxGray = (int)numMaxGray.Value;

            Param.NGJudgeEnable = checkJudgeNG.Checked;
        }

        private void numMinArea_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)numMinArea.Value;
            double area = value * _cameraResolution * _cameraResolution;
            labelMinAreaMM.Text = area.ToString("0.00");
        }

        private void numMaxArea_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)numMaxArea.Value;
            double area = value * _cameraResolution * _cameraResolution;
            labelMaxAreaMM.Text = area.ToString("0.00");
        }
    }
}
