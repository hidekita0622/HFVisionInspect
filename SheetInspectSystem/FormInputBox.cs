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
    public partial class FormInputBox : Form
    {
        public string sInputText = "";
        public FormInputBox(string sMessage, string sTitle, string sDefault)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;

            this.Text = sTitle;
            this.textMessage.Text = sMessage;
            this.textInput.Text = sDefault;

        }

		public FormInputBox(string sMessage, string sTitle, string sDefault, int iMode)
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;

			this.Text = sTitle;
			this.textMessage.Text = sMessage;
			this.textInput.Text = sDefault;
			if(iMode == 1)
			{
				textInput.PasswordChar = '\0'; 
			}

		}

		private void butOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            sInputText = textInput.Text;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            sInputText = "";
            this.Close();
        }

        private void textMessage_Enter(object sender, EventArgs e)
        {
            this.textInput.Focus();
        }
    }
}
