using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using HalconDotNet;

namespace HFVisionInspect
{
	public class ClassImageSave
	{
		public bool SaveEnable { get; set; }
		private static readonly int ThreadInterval = 5;
		public string DefaultPath { get; set; }
        public int CompRatio { get; set; }

		public bool SaveModePng { get; set; }
        bool saveFile;
        bool saveFileJpg;
        bool saveFileFullpath;

        string savePathname;
		string saveFilename;
		HObject imgSave = new HObject();

		public ClassImageSave()
		{
            CompRatio = 5;
            SaveEnable = true;
			//HOperatorSet.GenEmptyObj(out imgSave);

			Task.Run(() => SaveThread());
            SaveModePng = false;
            saveFile = false;
            saveFileJpg = false;
            saveFileFullpath = false;

        }

        public void Save(HObject img, string filename)
		{
			savePathname = DefaultPath;
			saveFilename = filename;
			imgSave = img;
		}
		public void SavePathname(HImage img, string pathname, string filename)
		{
            if (saveFile)
                return;
			savePathname = pathname;
			saveFilename = filename;
			if (imgSave != null)
				imgSave.Dispose();
            imgSave = img.CopyImage();
            saveFile = true;
            saveFileJpg = false;
        }

        public void SavePathnameJpg(HObject img, string pathname, string filename)
        {
            savePathname = pathname;
            saveFilename = filename;
            if (imgSave != null)
                imgSave.Dispose();
            imgSave = img;
            saveFile = false;
            saveFileJpg = true;
        }

        public void SaveFile(HObject img, string filename)
		{
			savePathname = "";
			saveFilename = filename;
			if (imgSave != null)
				imgSave.Dispose();
			imgSave = img.Clone();
            saveFileFullpath = true;
		}

		private void SaveThread()
		{
			Trace.WriteLine($"Starting ImageSaveThread.");

			while (SaveEnable)
			{
				Thread.Sleep(ThreadInterval);

				if (saveFile)
				{
                    if (SaveModePng)
                        SaveImagePng(saveFilename);
                    else
                        SaveImage(saveFilename);
                    saveFile = false;
                }
                else if (saveFileJpg)
                {
                    //SaveImage(saveFilename);
                    SaveImageJpg(saveFilename);
                    saveFileJpg = false;
                }
                else if (saveFileFullpath)
				{
					SaveImagePath(saveFilename);
                    saveFileFullpath = false;
                }

            }

			Trace.WriteLine($"Exit ImageSaveThread.");
		}
		private void SaveImage(string saveFilename)
		{
			DateTime NowDate = DateTime.Now;
			string datestr = NowDate.ToString("yyyyMMdd");
			//string savePath = string.Format("{0}\\{1}", savePathname, datestr);
			string savePath = savePathname;

			if (!System.IO.File.Exists(savePath))
			{
				System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(savePath);
			}

			string pathname;
			pathname = savePath + "\\" + saveFilename;
			try
			{
				if (imgSave != null)
				{
					//画像ファイル書込み
					HOperatorSet.WriteImage(imgSave, "bmp", 0, pathname);
					string msg = string.Format("SaveImageFin : {0}", pathname);
					Trace.WriteLine(msg);
				}
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}
		}

        private void SaveImagePng(string saveFilename)
        {
            DateTime NowDate = DateTime.Now;
            string datestr = NowDate.ToString("yyyyMMdd");
            //string savePath = string.Format("{0}\\{1}", savePathname, datestr);
            string savePath = savePathname;

            if (!System.IO.File.Exists(savePath))
            {
                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(savePath);
            }

            string pathname;
            pathname = savePath + "\\" + saveFilename;
            try
            {
                if (imgSave != null)
                {
                    //画像ファイル書込み
                    string format = string.Format("png {0}", CompRatio);
                    HOperatorSet.WriteImage(imgSave, format, 0, pathname);
                    string msg = string.Format("SaveImagePngFin : {0}", pathname);
                    Trace.WriteLine(msg);
                }
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
            }
        }

        private void SaveImageJpg(string saveFilename)
        {
            DateTime NowDate = DateTime.Now;
            string datestr = NowDate.ToString("yyyyMMdd");
            //string savePath = string.Format("{0}\\{1}", savePathname, datestr);
            string savePath = savePathname;

            if (!System.IO.File.Exists(savePath))
            {
                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(savePath);
            }

            string pathname;
            pathname = savePath + "\\" + saveFilename;
            try
            {
                if (imgSave != null)
                {
                    //画像ファイル書込み
                    string format = string.Format("jpeg");
                    HOperatorSet.WriteImage(imgSave, format, 0, pathname);
                    string msg = string.Format("SaveImageJpgFin : {0}", pathname);
                    Trace.WriteLine(msg);
                }
            }
            catch (HOperatorException oe)
            {
                Trace.WriteLine(oe.Message);
            }
        }

        private void SaveImagePath(string savePathname)
		{
			try
			{
				if (imgSave != null)
				{
					//画像ファイル書込み
					HOperatorSet.WriteImage(imgSave, "bmp", 0, savePathname);
					string msg = string.Format("SaveImage : {0}", savePathname);
					Trace.WriteLine(msg);
				}
			}
			catch (HOperatorException oe)
			{
				Trace.WriteLine(oe.Message);
			}
		}
	}
}
