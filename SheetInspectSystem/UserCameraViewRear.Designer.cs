namespace HFVisionInspect
{
	partial class UserCameraViewRear
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerFR = new System.Windows.Forms.Timer(this.components);
			this.labelViewName = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelFR = new System.Windows.Forms.Label();
			this.checkDOut = new System.Windows.Forms.CheckBox();
			this.labelProcTime = new System.Windows.Forms.Label();
			this.checkShowResult = new System.Windows.Forms.CheckBox();
			this.checkInspectOff = new System.Windows.Forms.CheckBox();
			this.checkRedrawImage = new System.Windows.Forms.CheckBox();
			this.checkShowInspArea = new System.Windows.Forms.CheckBox();
			this.butTryInspect = new System.Windows.Forms.Button();
			this.contextMouseRear = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuMouseSnap = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMouseGrab = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMouseGrabstop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuMouseIReadImage = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMouseIWriteImage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuMouseProcExecute = new System.Windows.Forms.ToolStripMenuItem();
			this.hCamImage = new HalconDotNet.HSmartWindowControl();
			this.checkDispProc = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.contextMouseRear.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerFR
			// 
			this.timerFR.Interval = 1000;
			this.timerFR.Tick += new System.EventHandler(this.timerFR_Tick);
			// 
			// labelViewName
			// 
			this.labelViewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelViewName.Location = new System.Drawing.Point(18, 9);
			this.labelViewName.Name = "labelViewName";
			this.labelViewName.Size = new System.Drawing.Size(86, 18);
			this.labelViewName.TabIndex = 29;
			this.labelViewName.Text = "裏面カメラ ";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.checkDispProc);
			this.panel1.Controls.Add(this.labelViewName);
			this.panel1.Controls.Add(this.labelFR);
			this.panel1.Controls.Add(this.checkDOut);
			this.panel1.Controls.Add(this.labelProcTime);
			this.panel1.Controls.Add(this.checkShowResult);
			this.panel1.Controls.Add(this.checkInspectOff);
			this.panel1.Controls.Add(this.checkRedrawImage);
			this.panel1.Controls.Add(this.checkShowInspArea);
			this.panel1.Controls.Add(this.butTryInspect);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(664, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(121, 339);
			this.panel1.TabIndex = 5;
			// 
			// labelFR
			// 
			this.labelFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFR.AutoSize = true;
			this.labelFR.Location = new System.Drawing.Point(20, 314);
			this.labelFR.Name = "labelFR";
			this.labelFR.Size = new System.Drawing.Size(35, 12);
			this.labelFR.TabIndex = 27;
			this.labelFR.Text = "label1";
			// 
			// checkDOut
			// 
			this.checkDOut.Location = new System.Drawing.Point(20, 218);
			this.checkDOut.Name = "checkDOut";
			this.checkDOut.Size = new System.Drawing.Size(81, 15);
			this.checkDOut.TabIndex = 26;
			this.checkDOut.Text = "出力OFF";
			this.checkDOut.UseVisualStyleBackColor = true;
			// 
			// labelProcTime
			// 
			this.labelProcTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelProcTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelProcTime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelProcTime.Location = new System.Drawing.Point(20, 283);
			this.labelProcTime.Name = "labelProcTime";
			this.labelProcTime.Size = new System.Drawing.Size(81, 21);
			this.labelProcTime.TabIndex = 25;
			this.labelProcTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkShowResult
			// 
			this.checkShowResult.Checked = true;
			this.checkShowResult.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowResult.Location = new System.Drawing.Point(20, 135);
			this.checkShowResult.Name = "checkShowResult";
			this.checkShowResult.Size = new System.Drawing.Size(72, 16);
			this.checkShowResult.TabIndex = 24;
			this.checkShowResult.Text = "結果表示";
			this.checkShowResult.UseVisualStyleBackColor = true;
			this.checkShowResult.CheckedChanged += new System.EventHandler(this.checkShowResult_CheckedChanged);
			// 
			// checkInspectOff
			// 
			this.checkInspectOff.Location = new System.Drawing.Point(20, 197);
			this.checkInspectOff.Name = "checkInspectOff";
			this.checkInspectOff.Size = new System.Drawing.Size(81, 15);
			this.checkInspectOff.TabIndex = 23;
			this.checkInspectOff.Text = "検査OFF";
			this.checkInspectOff.UseVisualStyleBackColor = true;
			// 
			// checkRedrawImage
			// 
			this.checkRedrawImage.Checked = true;
			this.checkRedrawImage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRedrawImage.Location = new System.Drawing.Point(20, 92);
			this.checkRedrawImage.Name = "checkRedrawImage";
			this.checkRedrawImage.Size = new System.Drawing.Size(81, 15);
			this.checkRedrawImage.TabIndex = 20;
			this.checkRedrawImage.Text = "表示更新";
			this.checkRedrawImage.UseVisualStyleBackColor = true;
			// 
			// checkShowInspArea
			// 
			this.checkShowInspArea.Location = new System.Drawing.Point(20, 113);
			this.checkShowInspArea.Name = "checkShowInspArea";
			this.checkShowInspArea.Size = new System.Drawing.Size(72, 16);
			this.checkShowInspArea.TabIndex = 17;
			this.checkShowInspArea.Text = "処理範囲";
			this.checkShowInspArea.UseVisualStyleBackColor = true;
			this.checkShowInspArea.CheckedChanged += new System.EventHandler(this.checkShowInspArea_CheckedChanged);
			// 
			// butTryInspect
			// 
			this.butTryInspect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butTryInspect.Location = new System.Drawing.Point(20, 30);
			this.butTryInspect.Name = "butTryInspect";
			this.butTryInspect.Size = new System.Drawing.Size(84, 40);
			this.butTryInspect.TabIndex = 1;
			this.butTryInspect.Text = "試　行";
			this.butTryInspect.UseVisualStyleBackColor = true;
			this.butTryInspect.Click += new System.EventHandler(this.butTryInspect_Click);
			// 
			// contextMouseRear
			// 
			this.contextMouseRear.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMouseSnap,
            this.menuMouseGrab,
            this.menuMouseGrabstop,
            this.toolStripSeparator1,
            this.menuMouseIReadImage,
            this.menuMouseIWriteImage,
            this.toolStripSeparator2,
            this.menuMouseProcExecute});
			this.contextMouseRear.Name = "contextMouseLight";
			this.contextMouseRear.Size = new System.Drawing.Size(162, 148);
			// 
			// menuMouseSnap
			// 
			this.menuMouseSnap.Name = "menuMouseSnap";
			this.menuMouseSnap.Size = new System.Drawing.Size(161, 22);
			this.menuMouseSnap.Text = "1枚取込(&S)";
			this.menuMouseSnap.Click += new System.EventHandler(this.menuMouseSnap_Click);
			// 
			// menuMouseGrab
			// 
			this.menuMouseGrab.Name = "menuMouseGrab";
			this.menuMouseGrab.Size = new System.Drawing.Size(161, 22);
			this.menuMouseGrab.Text = "連続取込(&G)";
			this.menuMouseGrab.Click += new System.EventHandler(this.menuMouseGrab_Click);
			// 
			// menuMouseGrabstop
			// 
			this.menuMouseGrabstop.Enabled = false;
			this.menuMouseGrabstop.Name = "menuMouseGrabstop";
			this.menuMouseGrabstop.Size = new System.Drawing.Size(161, 22);
			this.menuMouseGrabstop.Text = "連続取込終了(&C)";
			this.menuMouseGrabstop.Click += new System.EventHandler(this.menuMouseGrabstop_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
			// 
			// menuMouseIReadImage
			// 
			this.menuMouseIReadImage.Name = "menuMouseIReadImage";
			this.menuMouseIReadImage.Size = new System.Drawing.Size(161, 22);
			this.menuMouseIReadImage.Text = "画像読込(&R)";
			this.menuMouseIReadImage.Click += new System.EventHandler(this.menuMouseIReadImage_Click);
			// 
			// menuMouseIWriteImage
			// 
			this.menuMouseIWriteImage.Name = "menuMouseIWriteImage";
			this.menuMouseIWriteImage.Size = new System.Drawing.Size(161, 22);
			this.menuMouseIWriteImage.Text = "画像保存(&W)";
			this.menuMouseIWriteImage.Click += new System.EventHandler(this.menuMouseIWriteImage_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
			// 
			// menuMouseProcExecute
			// 
			this.menuMouseProcExecute.Name = "menuMouseProcExecute";
			this.menuMouseProcExecute.Size = new System.Drawing.Size(161, 22);
			this.menuMouseProcExecute.Text = "処理実行(&E)";
			this.menuMouseProcExecute.Click += new System.EventHandler(this.menuMouseProcExecute_Click);
			// 
			// hCamImage
			// 
			this.hCamImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hCamImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hCamImage.ContextMenuStrip = this.contextMouseRear;
			this.hCamImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hCamImage.HDoubleClickToFitContent = true;
			this.hCamImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hCamImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hCamImage.HKeepAspectRatio = true;
			this.hCamImage.HMoveContent = true;
			this.hCamImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hCamImage.Location = new System.Drawing.Point(0, 0);
			this.hCamImage.Margin = new System.Windows.Forms.Padding(0);
			this.hCamImage.Name = "hCamImage";
			this.hCamImage.Size = new System.Drawing.Size(785, 339);
			this.hCamImage.TabIndex = 4;
			this.hCamImage.WindowSize = new System.Drawing.Size(785, 339);
			this.hCamImage.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hCamImage_HMouseMove);
			// 
			// checkDispProc
			// 
			this.checkDispProc.Location = new System.Drawing.Point(20, 157);
			this.checkDispProc.Name = "checkDispProc";
			this.checkDispProc.Size = new System.Drawing.Size(72, 16);
			this.checkDispProc.TabIndex = 32;
			this.checkDispProc.Text = "処理画像";
			this.checkDispProc.UseVisualStyleBackColor = true;
			this.checkDispProc.CheckedChanged += new System.EventHandler(this.checkDispProc_CheckedChanged);
			// 
			// UserCameraViewRear
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.hCamImage);
			this.Name = "UserCameraViewRear";
			this.Size = new System.Drawing.Size(785, 339);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.contextMouseRear.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timerFR;
		private System.Windows.Forms.Label labelViewName;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelFR;
		private System.Windows.Forms.CheckBox checkDOut;
		private System.Windows.Forms.Label labelProcTime;
		private System.Windows.Forms.CheckBox checkShowResult;
		private System.Windows.Forms.CheckBox checkInspectOff;
		private System.Windows.Forms.CheckBox checkRedrawImage;
		private System.Windows.Forms.CheckBox checkShowInspArea;
		private System.Windows.Forms.Button butTryInspect;
		private System.Windows.Forms.ContextMenuStrip contextMouseRear;
		private System.Windows.Forms.ToolStripMenuItem menuMouseSnap;
		private System.Windows.Forms.ToolStripMenuItem menuMouseGrab;
		private System.Windows.Forms.ToolStripMenuItem menuMouseGrabstop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuMouseIReadImage;
		private System.Windows.Forms.ToolStripMenuItem menuMouseIWriteImage;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem menuMouseProcExecute;
		private HalconDotNet.HSmartWindowControl hCamImage;
		private System.Windows.Forms.CheckBox checkDispProc;
	}
}
