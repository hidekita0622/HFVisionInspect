namespace HFVisionInspect
{
	partial class FormAutoExposure
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
			this.lblExposureTime = new System.Windows.Forms.Label();
			this.numExposureTime = new System.Windows.Forms.NumericUpDown();
			this.groupAutoSet = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numExposuTimeHigh = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.numExposuTimeLow = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numTargetRange = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.numTargetGraylevel = new System.Windows.Forms.NumericUpDown();
			this.checkAutoEnable = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numExposureTime)).BeginInit();
			this.groupAutoSet.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numExposuTimeHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numExposuTimeLow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTargetRange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTargetGraylevel)).BeginInit();
			this.SuspendLayout();
			// 
			// lblExposureTime
			// 
			this.lblExposureTime.Location = new System.Drawing.Point(19, 17);
			this.lblExposureTime.Name = "lblExposureTime";
			this.lblExposureTime.Size = new System.Drawing.Size(95, 16);
			this.lblExposureTime.TabIndex = 38;
			this.lblExposureTime.Text = "露光時間 [μsec]";
			this.lblExposureTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numExposureTime
			// 
			this.numExposureTime.Location = new System.Drawing.Point(141, 15);
			this.numExposureTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.numExposureTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numExposureTime.Name = "numExposureTime";
			this.numExposureTime.Size = new System.Drawing.Size(65, 19);
			this.numExposureTime.TabIndex = 39;
			this.numExposureTime.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// groupAutoSet
			// 
			this.groupAutoSet.Controls.Add(this.label3);
			this.groupAutoSet.Controls.Add(this.numExposuTimeHigh);
			this.groupAutoSet.Controls.Add(this.label4);
			this.groupAutoSet.Controls.Add(this.numExposuTimeLow);
			this.groupAutoSet.Controls.Add(this.label2);
			this.groupAutoSet.Controls.Add(this.numTargetRange);
			this.groupAutoSet.Controls.Add(this.label1);
			this.groupAutoSet.Controls.Add(this.numTargetGraylevel);
			this.groupAutoSet.Location = new System.Drawing.Point(23, 54);
			this.groupAutoSet.Name = "groupAutoSet";
			this.groupAutoSet.Size = new System.Drawing.Size(290, 104);
			this.groupAutoSet.TabIndex = 40;
			this.groupAutoSet.TabStop = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(185, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(22, 18);
			this.label3.TabIndex = 47;
			this.label3.Text = "～";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numExposuTimeHigh
			// 
			this.numExposuTimeHigh.Location = new System.Drawing.Point(210, 57);
			this.numExposuTimeHigh.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numExposuTimeHigh.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numExposuTimeHigh.Name = "numExposuTimeHigh";
			this.numExposuTimeHigh.Size = new System.Drawing.Size(65, 19);
			this.numExposuTimeHigh.TabIndex = 46;
			this.numExposuTimeHigh.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(20, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(85, 17);
			this.label4.TabIndex = 44;
			this.label4.Text = "露光時間範囲";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numExposuTimeLow
			// 
			this.numExposuTimeLow.Location = new System.Drawing.Point(111, 57);
			this.numExposuTimeLow.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numExposuTimeLow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numExposuTimeLow.Name = "numExposuTimeLow";
			this.numExposuTimeLow.Size = new System.Drawing.Size(65, 19);
			this.numExposuTimeLow.TabIndex = 45;
			this.numExposuTimeLow.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(185, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(22, 18);
			this.label2.TabIndex = 43;
			this.label2.Text = "±";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numTargetRange
			// 
			this.numTargetRange.Location = new System.Drawing.Point(210, 27);
			this.numTargetRange.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numTargetRange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numTargetRange.Name = "numTargetRange";
			this.numTargetRange.Size = new System.Drawing.Size(65, 19);
			this.numTargetRange.TabIndex = 42;
			this.numTargetRange.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 17);
			this.label1.TabIndex = 40;
			this.label1.Text = "目標輝度値";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numTargetGraylevel
			// 
			this.numTargetGraylevel.Location = new System.Drawing.Point(111, 27);
			this.numTargetGraylevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numTargetGraylevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numTargetGraylevel.Name = "numTargetGraylevel";
			this.numTargetGraylevel.Size = new System.Drawing.Size(65, 19);
			this.numTargetGraylevel.TabIndex = 41;
			this.numTargetGraylevel.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
			// 
			// checkAutoEnable
			// 
			this.checkAutoEnable.AutoSize = true;
			this.checkAutoEnable.Location = new System.Drawing.Point(19, 53);
			this.checkAutoEnable.Name = "checkAutoEnable";
			this.checkAutoEnable.Size = new System.Drawing.Size(72, 16);
			this.checkAutoEnable.TabIndex = 41;
			this.checkAutoEnable.Text = "自動露光";
			this.checkAutoEnable.UseVisualStyleBackColor = true;
			this.checkAutoEnable.CheckedChanged += new System.EventHandler(this.checkAutoEnable_CheckedChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(167, 218);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(106, 41);
			this.btnCancel.TabIndex = 43;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(55, 218);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(106, 41);
			this.btnOk.TabIndex = 42;
			this.btnOk.Text = "保存";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// FormAutoExposure
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(336, 271);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.checkAutoEnable);
			this.Controls.Add(this.groupAutoSet);
			this.Controls.Add(this.lblExposureTime);
			this.Controls.Add(this.numExposureTime);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAutoExposure";
			this.Text = "自動露光設定";
			this.Load += new System.EventHandler(this.FormAutoExposure_Load);
			((System.ComponentModel.ISupportInitialize)(this.numExposureTime)).EndInit();
			this.groupAutoSet.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numExposuTimeHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numExposuTimeLow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTargetRange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTargetGraylevel)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblExposureTime;
		private System.Windows.Forms.NumericUpDown numExposureTime;
		private System.Windows.Forms.GroupBox groupAutoSet;
		private System.Windows.Forms.CheckBox checkAutoEnable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numTargetGraylevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numTargetRange;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numExposuTimeHigh;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numExposuTimeLow;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
	}
}