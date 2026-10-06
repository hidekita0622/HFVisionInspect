using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFVisionInspect
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
			//bool logExceptions = System.IO.File.Exists(Application.StartupPath + "\\LogExceptions.true");
			//bool allowMultipleInstance = System.IO.File.Exists(Application.StartupPath + "\\AllowMultipleInstance.true");

//			bool b;
//			System.Threading.Mutex m = new System.Threading.Mutex(false, Application.ProductName, out b);
//#if DEBUG
//			b = true;
//#endif
//			if (allowMultipleInstance | b)
			{
				//if (logExceptions)
				//{
				//	Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ThreadExceptionHandler);
				//	System.Threading.Thread.GetDomain().UnhandledException += new System.UnhandledExceptionEventHandler(UnhandledExceptionHandler);
				//	MessageBox.Show("Exception logging enabled.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
				//}
				// 64bit プロセスで動作しているか確認（AcapLibPlus.dll / HALCON は x64 版を使用する前提）
				if (!Environment.Is64BitProcess)
				{
					MessageBox.Show("32bit で起動しています。プロジェクトのプラットフォームを x64 にしてください。",
									"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FormMain());
			}
			//else
			//{
			//	MessageBox.Show("Already runnging.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			//}

			//m.Close();
			
			//Application.EnableVisualStyles();
   //         Application.SetCompatibleTextRenderingDefault(false);
   //         Application.Run(new FormMain());
        }

		public static void ThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			NotifyException("ThreadException", e.Exception);
		}

		public static void UnhandledExceptionHandler(object sender, System.UnhandledExceptionEventArgs e)
		{
			System.Exception exception = (System.Exception)e.ExceptionObject;
			if (exception != null)
			{
				NotifyException("UnhandledException", exception);
			}
		}

		public static void NotifyException(string msg, System.Exception e)
		{
			string path = Application.StartupPath + "\\Exception.log";

			DateTime now = DateTime.Now;
			string time = now.ToString("yyyy/MM/dd HH:mm:ss");

			System.Text.Encoding sjis = System.Text.Encoding.GetEncoding("Shift_JIS");
			System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true, sjis);

			writer.WriteLine(time);
			writer.WriteLine(msg);
			writer.WriteLine(e.ToString());

			writer.Close();
		}
	}
}
