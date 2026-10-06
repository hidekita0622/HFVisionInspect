using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;


namespace HFVisionInspect
{
    class ClassImageWindow
    {
        public bool IsEnabled { get; set; }
        private HWindow _winID;

        public ClassImageWindow(int w, int h)
        {
            HSystem.SetWindowAttr("background_color", "black");
            _winID = new HWindow();
            _winID.OpenWindow(0, 0, w, h, 0, "buffer", "");
            HDevWindowStack.Push(_winID);
            _winID.SetWindowExtents(0, 0, w, h);

            _winID.SetPart( 0, 0, h - 1, w - 1);
            _winID.SetDraw( "fill");
            _winID.SetLineWidth(1);
            IsEnabled = true;
        }

        public void ShowImage(HImage img)
        {
            if (IsEnabled == false) return;

            _winID.DispImage(img);
        }

        public void ShowImage(HRegion rgn, string color, string mode)
        {
            SetColor(color, mode);
            _winID.DispObj(rgn);
        }

        public void SetColor(string color, string mode)
        {
            _winID.SetColor(color);
            _winID.SetDraw(mode);
        }

        public void GetWinImage(out HImage imgDst)
        {
            imgDst = new HImage();
            imgDst = _winID.DumpWindowImage();
        }

        public void SaveWinImage(string path)
        {
            HImage img = new HImage();
            img = _winID.DumpWindowImage();
            img.WriteImage("bmp", 0, path);
        }
    }
}
