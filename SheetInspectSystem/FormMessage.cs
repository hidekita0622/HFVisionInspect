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
	public partial class FormMessage : Form
	{
		public FormMessage()
		{
			InitializeComponent();
		}

		public void Show(string message)
		{
			labelMessage.Text = message;
			Show();
		}

		public void FormHide()
		{
			Hide();
		}

		private void labelMessage_DoubleClick(object sender, EventArgs e)
		{
			Hide();
		}
	}
}
