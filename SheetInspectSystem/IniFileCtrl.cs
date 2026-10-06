using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;


namespace HFVisionInspect
{

	class IniFileCtrl
	{
		[DllImport("kernel32.dll")]
		private static extern int GetPrivateProfileString(
			string lpApplicationName,
			string lpKeyName,
			string lpDefault,
			StringBuilder lpReturnedstring,
			int nSize,
			string lpFileName);

		[DllImport("kernel32.dll")]
		private static extern int WritePrivateProfileString(
			string lpApplicationName,
			string lpKeyName,
			string lpstring,
			string lpFileName);

		public string filePath;
		public string SectionName = "SYSTEM";

		public IniFileCtrl(string filePath)
		{
			string CrtDir = System.IO.Directory.GetCurrentDirectory();
			this.filePath = CrtDir + "\\" + filePath;
		}
		public string this[string key]
		{
			set
			{
				string section = "SYSTEM";
				WritePrivateProfileString(section, key, value, filePath);
			}
			get
			{
				string section = "SYSTEM";
				StringBuilder buffer = new StringBuilder(256);
				GetPrivateProfileString(section, key, string.Empty, buffer, buffer.Capacity, filePath);
				return buffer.ToString();
			}
		}

		public string this[string section, string key]
		{
			set
			{
				WritePrivateProfileString(section, key, value, filePath);
			}
			get
			{
				StringBuilder buffer = new StringBuilder(256);
				GetPrivateProfileString(section, key, string.Empty, buffer, buffer.Capacity, filePath);
				return buffer.ToString();
			}
		}
	}

    internal static class Helpers
    {
        public static bool Export<T>(string path, T obj)
        {
            bool result = true;

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false, new System.Text.UTF8Encoding(false)))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    s.Serialize(sw, obj);
                    sw.Close();
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Export(): " + ex.Message);
                result = false;
            }

            return result;
        }

        public static bool Import<T>(string path, ref T obj)
        {
            if (!System.IO.File.Exists(path))
            {
                System.Diagnostics.Trace.WriteLine("Import(): File '" + path + "' not found.");
                return false;
            }

            bool result = true;

            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path, new System.Text.UTF8Encoding(false)))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = (T)s.Deserialize(sr);
                    sr.Close();
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Import(): " + ex.Message);
                result = false;
            }

            return result;
        }

		/// <summary>
		/// HDD容量の取得 [GB]
		/// </summary>
		/// <returns></returns>
		public static long HDDFreeSpace(string path)
		{
			long iHDDSpace = 0;

			//System.IO.DriveInfo cDrive = new System.IO.DriveInfo("D:\\");
			System.IO.DriveInfo cDrive = new System.IO.DriveInfo(path);
			iHDDSpace = cDrive.AvailableFreeSpace / 1024 / 1024 / 1024; // GByte単位
			//iHDDSpace = cDrive.TotalFreeSpace / 1000 / 1000 / 1000; // GByte単位
//			iHDDSpace = cDrive.TotalFreeSpace / 1000 / 1000; // MByte単位
			
			return iHDDSpace;
		}

		/// <summary>
		/// 指定サイズ分、古いファイルから削除
		/// </summary>
		/// <param name="DeletePath"></param>
		/// <param name="DeleteSize"></param>
		public static void DeleteFileSize(string DeletePath, long DeleteSize)
		{
			long totalSize = 0;
			long DeleteByte = DeleteSize * 1000 * 1000;   // MByte

			IEnumerable<string> files = Directory.EnumerateFileSystemEntries(DeletePath, "*", SearchOption.AllDirectories).OrderByDescending(f => File.GetLastWriteTime(f));
			foreach (string filename in files)
			{
				FileInfo fileinfo = new FileInfo(filename);
				if (totalSize < DeleteByte)
				{
					if (Directory.Exists(filename))
					{
						totalSize += GetSize(filename);
					}else
						totalSize += fileinfo.Length;
					continue;
				}
				if (File.Exists(filename))
				{
					fileinfo.Delete();
				}
				else if (Directory.Exists(filename))
				{
					Directory.Delete(filename, true);
				}
			}

		}

		/// <summary>
		/// ディレクトリサイズを返す
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static long GetSize(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			return DirectorySize(directoryInfo);
		}

		private static long DirectorySize(DirectoryInfo directoryInfo)
		{
			long totalSize = directoryInfo.EnumerateFiles().Sum(file => file.Length);
			totalSize += directoryInfo.EnumerateDirectories().Sum(dir => DirectorySize(dir));

			return totalSize;
		}

		/// <summary>
		/// 古いファイルから削除
		///   (注意：フォルダかファイルかの確認がしていない)
		/// </summary>
		public static void DeleteFileDate(string path, int count)
		{
			Directory.GetFiles(path)
			  .OrderByDescending(f => f)
			  .Skip(count)
			  .ToList()
			  .ForEach(f => File.Delete(f));
		}

		/// <summary>
		/// ログ出力
		/// </summary>
		/// <param name="LogText"></param>
		public static void WriteLog(string LogText)
		{
#if DEBUG
			DateTime NowDate = DateTime.Now;
#else
			//string LogPath = System.IO.Directory.GetCurrentDirectory() + "\\Log";
			//if (!System.IO.File.Exists(LogPath))
			//{
			//	System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(LogPath);
			//}

			//string sLogFilename = LogPath + "\\" + NowDate.ToString("yyyyMMdd") + ".txt";
#endif
			try
			{
				string str;
#if DEBUG
				str = NowDate.ToString("HH:mm:ss.fff") + "," + LogText;
#else
				str = LogText;
#endif
#if (DEBUG == false)
//				bool Append;
//				if (System.IO.File.Exists(sLogFilename))
//				{
//					Append = true;     //　追記(true)
//				}
//				else
//				{
//					Append = false;     // 新規(false) ファイル
//					DeleteFileDate(LogPath, 30);		// 30日を超えた分のみ削除
//				}
//				using (var sw = new StreamWriter(sLogFilename, Append, System.Text.Encoding.GetEncoding("sjis")))
//				{
////					sw.WriteLine("時間,内容");
//					str = NowDate.ToString("HH:mm:ss.fff") + "," + LogText;
//					sw.WriteLine(str);
//				}
#endif
				Trace.WriteLine(str);
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.Message);
			}
		}

		public static void WriteData(string DataText)
		{
			DateTime NowDate = DateTime.Now;
			string LogPath = System.IO.Directory.GetCurrentDirectory() + "\\Data";
			if (!System.IO.File.Exists(LogPath))
			{
				System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(LogPath);
			}

			string sLogFilename = LogPath + "\\" + NowDate.ToString("yyyyMMdd") + ".csv";
			try
			{
				string str;
#if DEBUG
				str = NowDate.ToString("HH:mm:ss.fff") + "," + DataText;
#else
				bool Append;
				if (System.IO.File.Exists(sLogFilename))
				{
					Append = true;     //　追記(true)
				}
				else
				{
					Append = false;     // 新規(false) ファイル
					//					DeleteFileDate(LogPath, 30);        // 30日を超えた分のみ削除
				}
				using (var sw = new StreamWriter(sLogFilename, Append, System.Text.Encoding.GetEncoding("sjis")))
				{
					if(Append == false)
					{
						str = "時刻,検査項目,面積,輝度,幅,高さ,円形度,角度";
						sw.WriteLine(str);
					}
					str = NowDate.ToString("HH:mm:ss.fff") + "," + DataText;
					sw.WriteLine(str);
				}
#endif
				Trace.WriteLine(str);
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.Message);
			}
		}

		public static string GetVersionInfo()
		{
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			System.Version ver = asm.GetName().Version;			//バージョンの取得

			return ver.ToString();
		}
	}
}
