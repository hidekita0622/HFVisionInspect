namespace HFVisionInspect
{
	partial class FormInputBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.butOK = new System.Windows.Forms.Button();
			this.butCancel = new System.Windows.Forms.Button();
			this.textMessage = new System.Windows.Forms.TextBox();
			this.textInput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Location = new System.Drawing.Point(442, 11);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(108, 28);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(442, 45);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(108, 28);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textMessage
			// 
			this.textMessage.BackColor = System.Drawing.SystemColors.Control;
			this.textMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textMessage.Location = new System.Drawing.Point(14, 11);
			this.textMessage.Multiline = true;
			this.textMessage.Name = "textMessage";
			this.textMessage.Size = new System.Drawing.Size(415, 62);
			this.textMessage.TabIndex = 3;
			this.textMessage.Enter += new System.EventHandler(this.textMessage_Enter);
			// 
			// textInput
			// 
			this.textInput.Location = new System.Drawing.Point(13, 86);
			this.textInput.Name = "textInput";
			this.textInput.PasswordChar = '*';
			this.textInput.Size = new System.Drawing.Size(537, 19);
			this.textInput.TabIndex = 0;
			// 
			// FormInputBox
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(566, 120);
			this.Controls.Add(this.textInput);
			this.Controls.Add(this.textMessage);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInputBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "FormInputBox";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Button butCancel;
		private System.Windows.Forms.TextBox textMessage;
		private System.Windows.Forms.TextBox textInput;
	}
}