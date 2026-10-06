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
    public partial class UserHistoryView : UserControl
    {
		const double ImageRotatePhi = 0;        // 20250925 回転無しに変更
		private ResultData _resultData = null;

        public Action<int> OnSplitMoved;
        public int ViewNo { get; set; }
        private HImage _blobImage = new HImage();
        private HImage _blobProcImage = new HImage();

        public UserHistoryView()
        {
            InitializeComponent();

        }

        private void UserHistoryView_Load(object sender, EventArgs e)
        {
            if (ViewNo < 0)
            {
                hWinGrabImage.Visible = false;
                hWinProcImage.Visible = false;
                splitContainerImage.IsSplitterFixed = false;
            }
            else
            {
                labelHistoryNo.Text = (ViewNo + 1).ToString();
            }

        }

        private void splitContainerImage_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (ViewNo < 0)
            {
                int h = splitContainerImage.SplitterDistance;
                OnSplitMoved.Invoke(h);
            }
        }

        public void SetSplitHeight(int height)
        {
            splitContainerImage.SplitterDistance = height;
        }

        public int GetSplitHeight()
        {
            return splitContainerImage.SplitterDistance;
        }

        public void SetResultData(ResultData data)
        {
            if (_resultData != null)
                _resultData.Dispose();
            _resultData = new ResultData();
            _resultData.CopyFrom(data);
        }
        public void UpdateDataView()
        {
            _blobImage.GenEmptyObj();
            _blobProcImage.GenEmptyObj();
            if (_resultData.No >= 0)
            {
                labelTime.Text = _resultData.Date.ToLongTimeString();
                labelHistoryNo.Text = _resultData.No.ToString();
                DispResultImage(_resultData);
            }
        }

        private void DispResultImage(ResultData resultData)
        {
            _blobImage.Dispose();
            _blobImage = resultData.BlobImage.RotateImage(ImageRotatePhi, "nearest_neighbor");
            int w, h;
            hWinGrabImage.HalconWindow.ClearWindow();
            _blobImage.GetImageSize(out w, out h);
            hWinGrabImage.HalconWindow.DispImage(_blobImage);
            hWinGrabImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
            hWinGrabImage.SetFullImagePart();

            _blobProcImage.Dispose();
            _blobProcImage = resultData.BlobProcImage.RotateImage(ImageRotatePhi, "nearest_neighbor");
            hWinProcImage.HalconWindow.ClearWindow();
            _blobProcImage.GetImageSize(out w, out h);
            hWinProcImage.HalconWindow.DispImage(_blobProcImage);
            hWinProcImage.HalconWindow.SetPart((int)0, 0, h - 1, w - 1);
            hWinProcImage.SetFullImagePart();
        }

    }
}
