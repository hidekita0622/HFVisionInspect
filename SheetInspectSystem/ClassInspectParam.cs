using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HFVisionInspect
{
    public enum ImageBinaryMode
    {
        BINARY_SIMPLE = 0,
        BINARY_SAUVOLA,
        BINARY_VAR,             // 標準偏差による2値化
        BINARY_NUM,
    }

    public enum ImageBinaryColor
    {
        BINARY_DARK = 0,
        BINARY_LIGHT,
        BINARY_UNION,
    }

    public enum enMorphologyType
    {
        None = 0,
        Erosion,
        Dilation,
        Closing,
        Opening,
    }

    /// <summary>
    /// モルフォロジーパラメータ
    /// </summary>
    public class MorphologyParam
    {
        public const int MaxMorphology = 4;
        public enMorphologyType[] Type;
        public int[] Length;

        public MorphologyParam()
        {
            Type = new enMorphologyType[MaxMorphology];
            Length = new int[MaxMorphology];

            for(int i= 0; i < MaxMorphology; i++)
            {
                Type[i] = enMorphologyType.None;
                Length[i] = 0;
            }
        }
        public void CopyFrom(MorphologyParam src)
        {
            for (int i = 0; i < MaxMorphology; i++)
            {
                Type[i] = src.Type[i];
                Length[i] = src.Length[i];
            }

        }
    }

    /// <summary>
    /// 2値化パラメータ
    /// </summary>
    public class BinaryProcParam
    {
        public ImageBinaryColor BinaryColor;        // 検出色
        public ImageBinaryMode BinaryMode;
        public int ThresholdLo;
        public int ThresholdHi;
        public double SauvolaMaskSize; // Sauvola Mask Size
        public double SauvolaScale;    // Sauvola Scale
        public int VarMaskWidth;        // 標準偏差による2値化 マスク幅
        public int VarMaskHeight;       // マスク高さ
        public double VarDevScale;      // 偏差用係数
        public double VarThreshold;     // しきい値


        public BinaryProcParam()
        {
            BinaryColor = ImageBinaryColor.BINARY_DARK;
            BinaryMode = ImageBinaryMode.BINARY_SIMPLE;
            ThresholdLo = 64;
            ThresholdHi = 200;
            SauvolaMaskSize = 30;
            SauvolaScale = 0.3;
            VarMaskWidth = 11;
            VarMaskHeight = 11;
            VarDevScale = 0.1;
            VarThreshold = 2;
        }
        public void CopyFrom(BinaryProcParam src)
        {
            BinaryColor = src.BinaryColor;
            BinaryMode = src.BinaryMode;
            ThresholdLo = src.ThresholdLo;
            ThresholdHi = src.ThresholdHi;
            SauvolaMaskSize = src.SauvolaMaskSize;
            SauvolaScale = src.SauvolaScale;
            VarMaskWidth = src.VarMaskWidth;
            VarMaskHeight = src.VarMaskHeight;
            VarDevScale = src.VarDevScale;
            VarThreshold = src.VarThreshold;
        }
    }

    /// <summary>
    /// 欠陥判定パラメータ
    /// </summary>
    public class BlobJudgeParam
    {
        public bool Enable;
        public int MinArea;             // 最小面積 
        public int MaxArea;             // 最大面積 
        public int MinGray;             // 最小輝度値
        public int MaxGray;             // 最大輝度値

        public bool NGJudgeEnable;      // 検出した際にNGと判定するか

        public BlobJudgeParam()
        {
            Enable = false;
            MinArea = 100;
            MaxArea = 1000;
            MinGray = 50;
            MaxGray = 200;

            NGJudgeEnable = false;
        }
        public void CopyFrom(BlobJudgeParam src)
        {
            Enable = src.Enable;
            MinArea = src.MinArea;
            MaxArea = src.MaxArea;
            MinGray = src.MinGray;
            MaxGray = src.MaxGray;

            NGJudgeEnable = src.NGJudgeEnable;
        }
    }

	/// <summary>
	/// 欠陥検出パラメーター
	/// </summary>
	public class BlobParameter
	{
		public const int BlobJudgeNum = 5;

		public bool GaussEnable;
		public int GaussAlpha;
		public bool DiffImage = false;
        public int DiffMult;

        public bool ProcEnable;         // 有効／無効
        public BinaryProcParam BinaryParam;
        public MorphologyParam MorphologyParam;
        public bool ConvexEnabled;      // 凸包有効
        public bool FillupEnabled;      // 穴を埋める
        public BlobJudgeParam[] JudgeParam;

        public BlobParameter()
        {
            GaussEnable = true;
            GaussAlpha = 3;
			DiffImage = false;
            DiffMult = 1;
			ProcEnable = true;
            BinaryParam = new BinaryProcParam();
            MorphologyParam = new MorphologyParam();
            ConvexEnabled = false;
            FillupEnabled = false;
            JudgeParam = new BlobJudgeParam[BlobJudgeNum];
            for (int i = 0; i < BlobJudgeNum; i++)
            {
                JudgeParam[i] = new BlobJudgeParam();
            }
        }
        public void CopyFrom(BlobParameter src)
        {
			GaussEnable = src.GaussEnable;
            GaussAlpha = src.GaussAlpha;
			DiffImage = src.DiffImage;
            DiffMult = src.DiffMult;

            BinaryParam.CopyFrom(src.BinaryParam);
            MorphologyParam.CopyFrom(src.MorphologyParam);
            ConvexEnabled = src.ConvexEnabled;
            FillupEnabled = src.FillupEnabled;
            ProcEnable = src.ProcEnable;
            for (int i = 0; i < BlobJudgeNum; i++)
            {
                this.JudgeParam[i].CopyFrom(src.JudgeParam[i]);
            }
        }
    }

    /// <summary>
    /// 検査範囲パラメータ
    /// </summary>
    public class InspectAreaParameter
    {
        public Rectangle AreaRect;
        public bool IsFindEdgeLeft;
        public EdgeSearchParameter LeftEdgeParam;
        public bool IsFindEdgeRight;
        public EdgeSearchParameter RightEdgeParam;

        public InspectAreaParameter()
        {
            AreaRect = new Rectangle(100, 100, 200, 200);
            IsFindEdgeLeft = false;
            LeftEdgeParam = new EdgeSearchParameter();
            IsFindEdgeRight = false;
            RightEdgeParam = new EdgeSearchParameter();
        }

        public void CopyFrom(InspectAreaParameter src)
        {
            AreaRect = src.AreaRect;
            IsFindEdgeLeft = src.IsFindEdgeLeft;
            LeftEdgeParam.CopyFrom(src.LeftEdgeParam);
            IsFindEdgeRight = src.IsFindEdgeRight;
            RightEdgeParam.CopyFrom(src.RightEdgeParam);
        }

    }

    /// <summary>
    /// 自動露光設定パラメータ
    /// </summary>
    public class AutoExposureParam
    {
        public double TargetGrayLevel;
        public double TargetGrayRange;
        public int AutoExposureTimeLow;
        public int AutoExposureTimeHigh;

        public AutoExposureParam()
        {
            TargetGrayLevel = 150;
            TargetGrayRange = 10;
            AutoExposureTimeLow = 100;
            AutoExposureTimeHigh = 300;
        }

		public void CopyFrom(AutoExposureParam src)
        {
            TargetGrayLevel = src.TargetGrayLevel;
            TargetGrayRange = src.TargetGrayRange;
            AutoExposureTimeLow = src.AutoExposureTimeLow;
            AutoExposureTimeHigh= src.AutoExposureTimeHigh;
		}

	}

	/// <summary>
	/// 品種パラメータ
	/// </summary>
	public class InspectParameter
	{
        public string RecipeName;           // 品種名

        public Rectangle InspAreaRight;
		public EdgeSearchParameter EdgeParamRight;
        public AutoExposureParam AutoExposureRight;

		public Rectangle InspAreaLeft;
		public EdgeSearchParameter EdgeParamLeft;
		public AutoExposureParam AutoExposureLeft;

		public BlobParameter DarkBlob;

		public InspectParameter()
		{
            RecipeName = "新規品種";

			//検査エリア
			InspAreaRight.X = 400;
			InspAreaRight.Y = 200;
			InspAreaRight.Width = 700;
			InspAreaRight.Height = 770;
			EdgeParamRight = new EdgeSearchParameter();
            EdgeParamRight.BoxArea = new Rectangle(100, InspAreaRight.Y, InspAreaRight.X - 100, InspAreaRight.Height);
			AutoExposureRight = new AutoExposureParam();

			InspAreaLeft.X = 100;
            InspAreaLeft.Y = 200;
            InspAreaLeft.Width = 700;
            InspAreaLeft.Height = 770;
			EdgeParamLeft = new EdgeSearchParameter();
			EdgeParamLeft.BoxArea = new Rectangle(InspAreaLeft.X + InspAreaLeft.Width, InspAreaLeft.Y, Define.CameraWidth - InspAreaLeft.Right - 100, InspAreaLeft.Height);
			AutoExposureLeft = new AutoExposureParam();

			DarkBlob = new BlobParameter();
        }
        public void CopyFrom(InspectParameter src)
		{
            RecipeName = src.RecipeName;

			InspAreaRight = src.InspAreaRight;
			EdgeParamRight.CopyFrom(src.EdgeParamRight);
			AutoExposureRight.CopyFrom(src.AutoExposureRight);

			InspAreaLeft = src.InspAreaLeft;
			EdgeParamLeft.CopyFrom(src.EdgeParamLeft);
            AutoExposureLeft.CopyFrom(src.AutoExposureLeft);

			//Parameter Blob
			DarkBlob.CopyFrom(src.DarkBlob);
        }
    }
}
