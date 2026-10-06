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
    public partial class FormSystemParam : Form
    {
        public ClassSystemParam SystemParam;
        public bool operatorMode = true;
        private string InitialDirectory = Application.StartupPath;

        public FormSystemParam()
        {
            InitializeComponent();
            SystemParam = new ClassSystemParam();
        }

        private void FormSystemParam_Load(object sender, EventArgs e)
        {
            checkEnableLineCamera.Checked = SystemParam.CamInfo.Enable;
            checkLeftCameraEnable.Checked = SystemParam.CamInfo.FrontCamera.Enable;
            checkRightCameraEnable.Checked = SystemParam.CamInfo.RearCamera.Enable;
            groupCameraInfoCH1.Enabled = SystemParam.CamInfo.Enable;
            numLineRate.Value = SystemParam.CamInfo.FrontCamera.LineRate;
            numExposureTImeLeftCamera.Value = SystemParam.CamInfo.FrontCamera.ExposureTime;
            numExposureTImeRightCamera.Value = SystemParam.CamInfo.RearCamera.ExposureTime;
            numGainLeftCamera.Value = (decimal)SystemParam.CamInfo.FrontCamera.Gain;
			numGainRightCamera.Value = (decimal)SystemParam.CamInfo.RearCamera.Gain;
			textIniFilename.Text = SystemParam.CamInfo.IniFilename;
            numGrabHeight.Value = SystemParam.CamInfo.GrabHeight;
            numResolution.Value = (decimal)SystemParam.CamInfo.Resolution;
            numPosXLeftCamera.Value = SystemParam.CamInfo.FrontCamera.BasePosX;
            numPosXRightCamera.Value = SystemParam.CamInfo.RearCamera.BasePosX;
            //numTargetGlayLevel.Value = (decimal)SystemParam.TargetGrayLevel;

            textRecipePath.Text = SystemParam.RecipePath;
            textImageSavePath.Text = SystemParam.ImageSavePath;
            chkNGImageSave.Checked = SystemParam.NGImageSave;
            textNGImageSavePath.Text = SystemParam.NGImageSavePath;
            chkImageSavePNG.Checked = SystemParam.ImageSavePng;
            numCompRatio.Value = SystemParam.ImageCompRatio;
            numDefectImageWidth.Value = SystemParam.DefectImageWidth;
            numDefectImageHeight.Value = SystemParam.DefectImageHeight;
            numHDDCapacity.Value = SystemParam.HDDCapacityLimit;
            tbPassword.Text = SystemParam.Password;
            checkDioEnabled.Checked = SystemParam.DioEnabled;
            numBuzzerNo.Value = SystemParam.BuzzerNo;
            numCutterTimeDelay.Value = SystemParam.CutterTimerDelay;
            numCutterTime.Value = SystemParam.CutterTimer;
			numRegistPosRange.Value = SystemParam.RegistPosRange;
			numRegistTimeRange.Value = SystemParam.RegistTimeRange;

			numLightLifeHourLimit.Value = (decimal)SystemParam.LightLifeHourLimit;
            numGrayMeanlLimit.Value = (decimal)SystemParam.GrayMeanLimit;

            UpdateUI();
        }

        private void UpdateUI()
        {
            tbPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private bool ValidateValues()
        {
            if (!textImageSavePath.Text.EndsWith("\\"))
            {
                textImageSavePath.Text += "\\";
            }

            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SystemParam.CamInfo.Enable = checkEnableLineCamera.Checked;
            SystemParam.CamInfo.FrontCamera.Enable = checkLeftCameraEnable.Checked;
            SystemParam.CamInfo.RearCamera.Enable = checkRightCameraEnable.Checked;
            SystemParam.CamInfo.FrontCamera.LineRate = (int)numLineRate.Value;
			SystemParam.CamInfo.RearCamera.LineRate = (int)numLineRate.Value;
            SystemParam.CamInfo.FrontCamera.ExposureTime = (int)numExposureTImeLeftCamera.Value;
            SystemParam.CamInfo.RearCamera.ExposureTime = (int)numExposureTImeRightCamera.Value;
            SystemParam.CamInfo.FrontCamera.Gain = (double)numGainLeftCamera.Value;
			SystemParam.CamInfo.RearCamera.Gain = (double)numGainRightCamera.Value;
			SystemParam.CamInfo.IniFilename = textIniFilename.Text;
            SystemParam.CamInfo.GrabHeight = (int)numGrabHeight.Value;
            SystemParam.CamInfo.FrontCamera.LineRate = (int)numLineRate.Value;
			SystemParam.CamInfo.RearCamera.LineRate = (int)numLineRate.Value;
			SystemParam.CamInfo.Resolution = (double)numResolution.Value;
            SystemParam.CamInfo.FrontCamera.BasePosX = (int)numPosXLeftCamera.Value;
            SystemParam.CamInfo.RearCamera.BasePosX = (int)numPosXRightCamera.Value;
            //SystemParam.TargetGrayLevel = (int)numTargetGlayLevel.Value;


			SystemParam.RecipePath = textRecipePath.Text;
            SystemParam.ImageSavePath = textImageSavePath.Text;
            SystemParam.NGImageSave = chkNGImageSave.Checked;
            SystemParam.NGImageSavePath = textNGImageSavePath.Text;
            SystemParam.ImageSavePng = chkImageSavePNG.Checked;
            SystemParam.ImageCompRatio = (int)numCompRatio.Value;
            SystemParam.DefectImageWidth = (int)numDefectImageWidth.Value;
            SystemParam.DefectImageHeight = (int)numDefectImageHeight.Value;
            SystemParam.HDDCapacityLimit = (int)numHDDCapacity.Value;
            SystemParam.Password = tbPassword.Text;
            SystemParam.DioEnabled = checkDioEnabled.Checked;
            SystemParam.BuzzerNo = (int)numBuzzerNo.Value;
            SystemParam.CutterTimerDelay = (int)numCutterTimeDelay.Value;
            SystemParam.CutterTimer = (int)numCutterTime.Value;

            SystemParam.LightLifeHourLimit =(double)numLightLifeHourLimit.Value;
            SystemParam.GrayMeanLimit = (double)numGrayMeanlLimit.Value;
			SystemParam.RegistPosRange = (int)numRegistPosRange.Value;
			SystemParam.RegistTimeRange = (int)numRegistTimeRange.Value;
		}


		private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void btnImageSavePath_Click(object sender, EventArgs e)
        {
            string s;

            if (GetDir(lblImageSavePath.Text, textImageSavePath.Text, out s))
            {
                textImageSavePath.Text = s;
            }
        }

        private bool GetDir(string desc, string current, out string dir)
        {
            dir = "";

            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = desc;
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            dlg.SelectedPath = current;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                dir = dlg.SelectedPath;
                return true;
            }

            return false;
        }

        private void FormSystemParam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (!ValidateValues())
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

		private void chkNGImageSave_CheckedChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

        private void chkShowPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void checkEnableLineCamera_CheckedChanged(object sender, EventArgs e)
        {
            groupCameraInfoCH1.Enabled = checkEnableLineCamera.Checked;
        }

        private void btnCameraCH1CcfFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.InitialDirectory = InitialDirectory;
            //ofd.Filter = "BMPファイル(*.bmp)|*.bmp|PNGファイル(*.png)|*.png";
            ofd.Filter = "すべてのファイル(*.*) | *.*";

            ofd.FilterIndex = 0;
            ofd.Title = "画像ファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                InitialDirectory = ofd.FileName;
                textIniFilename.Text = ofd.FileName;
            }

        }

        private void btnGetRecipePath_Click(object sender, EventArgs e)
        {
            string s;

            if (GetDir(labelRecipePath.Text, textRecipePath.Text, out s))
            {
                textRecipePath.Text = s;
            }
        }

        private void checkCamera0Enable_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnNGImageSavePath_Click(object sender, EventArgs e)
        {
            string s;

            if (GetDir("NG画像保存フォルダ", textNGImageSavePath.Text, out s))
            {
                textNGImageSavePath.Text = s;
            }
        }
    }
}
