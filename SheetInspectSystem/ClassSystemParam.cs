using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFVisionInspect
{
	public static class Define
	{
		public const string AppTitle = "中空糸外観検査装置";
		public const int CameraWidth = 1024;
		public const string SystemFilename = "\\HFVisionInspect.xml";
		public const string InspectParamFilename = "\\InspectionParam.xml";
		public const string LightLifedata = "\\LPLife.xml";

		public const string LastImageFilenameLeft = "\\LastImageCam0.bmp";
		public const string LastImageFilenameRight = "\\LastImageCam1.bmp";
		public const string DiffImageFilenameLeft = "\\DiffImageCam0.bmp";
		public const string DiffImageFilenameRight = "\\DiffImageCam1.bmp";
	}

    public class CameraViewInfo
    {
        public ViewID ID { get; set; }
        public int CameraNo { get; set; }
		public string Name { get; set; }

		public CameraViewInfo()
        {
            ID = ViewID.ViewFront;
            CameraNo = 0;
            Name = "";
        }

		public void CopyFrom(CameraViewInfo src)
        {
            ID = src.ID;
            CameraNo = src.CameraNo;
            Name = src.Name;
        }
	}

	public class ClassRecipeInfo
    {
        public string RecipeName { get; set; }
        public string RecipeFilename { get; set; }
        //public InspectParameter InspectParam = new InspectParameter();
        public int LightVal0 {get; set;}
        public int LightVal1 { get; set; }
        public int LightVal2 { get; set; }

        public ClassRecipeInfo()
        {
            RecipeName = "新規品種";
            RecipeFilename = "";
        }

        public ClassRecipeInfo(ClassRecipeInfo src)
        {
            this.RecipeName = src.RecipeName;
            this.RecipeFilename = src.RecipeFilename;
            //this.InspectParam.CopyFrom(src.InspectParam);
            this.LightVal0 = src.LightVal0;
            this.LightVal1 = src.LightVal1;
			this.LightVal2 = src.LightVal2;
		}
	}

    public class CameraParameter
    {
        public bool Enable { get; set; }
        public int ExposureTime { get; set; }
        public double Gain { get; set; }
        public int BasePosX { get; set; }
        public int LineRate { get; set; }

        public CameraParameter()
        {
			Enable = true;
			ExposureTime = 50;
			Gain = 100;
			LineRate = 1000;
			BasePosX = 0;
		}

		public void CopyFrom(CameraParameter src)
        {
            Enable = src.Enable;
            ExposureTime = src.ExposureTime;
            Gain = src.Gain;
            BasePosX = src.BasePosX;
            LineRate = src.LineRate;
        }
	}

	public class LineCameraInfo
    {

        public bool Enable { get; set; }
		public CameraParameter FrontCamera { get; set; }
        public CameraParameter RearCamera { get; set; }

		public int PanelHeight { get; set; }
        public string IniFilename { get; set; }
        public int GrabHeight { get; set; }

        public double Resolution { get; set; }      // 分解能 [mm/pix]

        public LineCameraInfo() : base()
        {
            Enable = true;
			FrontCamera = new CameraParameter();
			RearCamera = new CameraParameter();

			PanelHeight = 0;
            IniFilename = "";
            GrabHeight = 1024;
            Resolution = 0.01;
        }

        public void CopyFrom(LineCameraInfo src)
        {
            Enable = src.Enable;
			FrontCamera.CopyFrom(src.FrontCamera);
			RearCamera.CopyFrom(src.RearCamera);

            IniFilename = src.IniFilename;
            this.GrabHeight = src.GrabHeight;
            PanelHeight = src.PanelHeight;
            Resolution = src.Resolution;
        }
    }

    public class ClassSystemParam
	{
        public CameraViewInfo FrontCamera { get; set; }
        public CameraViewInfo RearCamera { get; set; }

        public string RecipePath { get; set; }
        public string ImageSavePath { get; set; }           // 結果保存先
        public bool NGImageSave { get; set; }
        public string NGImageSavePath { get; set; }
    
        public int DefectImageWidth { get; set; }
        public int DefectImageHeight { get; set; }

        public bool ImageSavePng { get; set; }
        public int ImageCompRatio { get; set; }
        public string Password { get; set; }

        public int HDDCapacityLimit { get; set; }

        public string ActiveRecipefile = "";

        public LineCameraInfo CamInfo { get; set; }

        public bool LightPowerOff { get; set; }

        public bool DioEnabled { get; set; }

        public int BuzzerNo { get; set; }

        public int CutterTimerDelay { get; set; }
        public int CutterTimer { get; set; }

        public int CameraViewHeight { get; set; }

        public string InspectID { get; set; }

        public double LightLifeHourLimit { get; set; }
        public double GrayMeanLimit { get; set; }

        public int RegistPosRange { get; set; }
		public int RegistTimeRange { get; set; }

        //public int TargetGrayLevel {  get; set; }

		public ClassSystemParam()
		{
            FrontCamera = new CameraViewInfo();
			RearCamera = new CameraViewInfo();

            FrontCamera.ID = ViewID.ViewFront;
			FrontCamera.CameraNo = 0;
            FrontCamera.Name = "操作側";

            RearCamera.ID = ViewID.ViewRear;
            RearCamera.CameraNo = 1;
			RearCamera.Name = "反操作側";

			RecipePath = Application.StartupPath;
            ImageSavePath = Application.StartupPath;
            ImageSavePng = false;
            ImageCompRatio = 5;
            DefectImageWidth = 512;
            DefectImageHeight = 512;

            NGImageSavePath = Application.StartupPath;
            NGImageSave = false;

            Password = "";
            HDDCapacityLimit = 50;

            CamInfo = new LineCameraInfo();

            LightPowerOff = true;

            DioEnabled = false;
            BuzzerNo = 0;
            CutterTimerDelay = 30000;
            CutterTimer = 60000;
            CameraViewHeight = 500;
            InspectID = "";

            LightLifeHourLimit = 8000;
            GrayMeanLimit = 100;

            RegistPosRange = 50;
            RegistTimeRange = 1;

            //TargetGrayLevel = 140;
		}

		public ClassSystemParam(ClassSystemParam src)
        {
			this.CopyFrom(src);
        }

        public void CopyFrom(ClassSystemParam src)
        {
            FrontCamera.CopyFrom(src.FrontCamera);
			RearCamera.CopyFrom(src.RearCamera);

			this.RecipePath = src.RecipePath;
            this.ImageSavePath = src.ImageSavePath;
            this.ImageSavePng = src.ImageSavePng;
            this.ImageCompRatio = src.ImageCompRatio;
            this.DefectImageWidth = src.DefectImageWidth;
            this.DefectImageHeight = src.DefectImageHeight;

            this.Password = src.Password;
            this.HDDCapacityLimit = src.HDDCapacityLimit;
			this.ActiveRecipefile = src.ActiveRecipefile;
			this.CamInfo.CopyFrom(src.CamInfo);
            this.LightPowerOff = src.LightPowerOff;

            NGImageSave = src.NGImageSave;
            NGImageSavePath = src.NGImageSavePath;

            DioEnabled = src.DioEnabled;
            BuzzerNo = src.BuzzerNo;
            CutterTimerDelay = src.CutterTimerDelay;
            CutterTimer = src.CutterTimer;
            CameraViewHeight = src.CameraViewHeight;
            InspectID = src.InspectID;

			LightLifeHourLimit = src.LightLifeHourLimit;
            GrayMeanLimit = src.GrayMeanLimit;

			RegistPosRange = src.RegistPosRange;
            RegistTimeRange = src.RegistTimeRange;

            //TargetGrayLevel = src.TargetGrayLevel;
		}
	}

    public class LightLifeControl
    {
        //public   string StartDate;
        public double TotalHours;
        private  DateTime StartDateTime;

        public LightLifeControl()
        {
            StartDateTime = DateTime.Now;
            //StartDate = "2022/07/08 01:01:00";
            //DateTime.TryParse(StartDate, out LightDate);
            //LightDate = new DateTime(2022, 7, 7, 0, 0, 0);
        }

        public void UpdateLife()
        {
            double hour = GetLifeHour();
            TotalHours = hour;
            StartDateTime = DateTime.Now;
        }

        public void ResetLife()
        {
            //DateTime dt = DateTime.Now;
            //StartDate = dt.ToString("yyyy/MM/dd HH:mm:ss");
            StartDateTime = DateTime.Now;
            TotalHours = 0;
        }

        public double GetLifeHour()
        {
            double lHour = 0;

            //DateTime dt0;
            //DateTime.TryParse(StartDate, out dt0);
            DateTime dt1 = DateTime.Now;
            TimeSpan dtS = dt1 - StartDateTime;
            lHour = dtS.TotalHours;

            return lHour + TotalHours;
        }
    }
}
