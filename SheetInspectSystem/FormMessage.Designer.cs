namespace HFVisionInspect
{
	partial class FormMessage
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
			this.labelMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelMessage
			// 
			this.labelMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.labelMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelMessage.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMessage.Location = new System.Drawing.Point(0, 0);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(731, 134);
			this.labelMessage.TabIndex = 7;
			this.labelMessage.Text = "カメラ待機中";
			this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelMessage.DoubleClick += new System.EventHandler(this.labelMessage_DoubleClick);
			// 
			// FormMessage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 134);
			this.ControlBox = false;
			this.Controls.Add(this.labelMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FormMessage";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelMessage;
	}
}