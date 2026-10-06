namespace HFVisionInspect
{
    partial class FormInspectSetting
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
			HFVisionInspect.BlobJudgeParam blobJudgeParam1 = new HFVisionInspect.BlobJudgeParam();
			HFVisionInspect.BlobJudgeParam blobJudgeParam2 = new HFVisionInspect.BlobJudgeParam();
			HFVisionInspect.BlobJudgeParam blobJudgeParam3 = new HFVisionInspect.BlobJudgeParam();
			HFVisionInspect.BlobJudgeParam blobJudgeParam4 = new HFVisionInspect.BlobJudgeParam();
			HFVisionInspect.BlobJudgeParam blobJudgeParam5 = new HFVisionInspect.BlobJudgeParam();
			this.splitSetting = new System.Windows.Forms.SplitContainer();
			this.btGetProcArea = new System.Windows.Forms.Button();
			this.textRecipeName = new System.Windows.Forms.TextBox();
			this.labelRecipeName = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkDifferenceImage = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numGaussAlpha = new System.Windows.Forms.NumericUpDown();
			this.checkGaussEnable = new System.Windows.Forms.CheckBox();
			this.tabDefectLevel = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.userDefectLevel0 = new HFVisionInspect.UserSetDefectLevel();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.userDefectLevel1 = new HFVisionInspect.UserSetDefectLevel();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.userDefectLevel2 = new HFVisionInspect.UserSetDefectLevel();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.userDefectLevel3 = new HFVisionInspect.UserSetDefectLevel();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.userDefectLevel4 = new HFVisionInspect.UserSetDefectLevel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioBinarySimple = new System.Windows.Forms.RadioButton();
			this.label17 = new System.Windows.Forms.Label();
			this.numBlackDotSauvolaScale = new System.Windows.Forms.NumericUpDown();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioBinLight = new System.Windows.Forms.RadioButton();
			this.radioBinDark = new System.Windows.Forms.RadioButton();
			this.buttonMorphology = new System.Windows.Forms.Button();
			this.numBinaryThresholdHi = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.numBinaryThresholdLo = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.numBlackDotSauvolaMaskSize = new System.Windows.Forms.NumericUpDown();
			this.radioBinarySauvola = new System.Windows.Forms.RadioButton();
			this.panelParam = new System.Windows.Forms.Panel();
			this.splitImageView = new System.Windows.Forms.SplitContainer();
			this.panelView = new System.Windows.Forms.Panel();
			this.checkShowProfile = new System.Windows.Forms.CheckBox();
			this.labelLocate = new System.Windows.Forms.Label();
			this.hCamImage = new HalconDotNet.HSmartWindowControl();
			this.panelInfo = new System.Windows.Forms.Panel();
			this.radioShowImgBase = new System.Windows.Forms.RadioButton();
			this.butRecordBase = new System.Windows.Forms.Button();
			this.groupImage = new System.Windows.Forms.GroupBox();
			this.radioShowImgProc = new System.Windows.Forms.RadioButton();
			this.radioShowImgSrc = new System.Windows.Forms.RadioButton();
			this.listDefect = new System.Windows.Forms.ListView();
			this.columnHeaderNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnLotate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnBright = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.checkBinaryImg = new System.Windows.Forms.CheckBox();
			this.checkShowResult = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.checkShowArea = new System.Windows.Forms.CheckBox();
			this.butProcTrial = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitSetting)).BeginInit();
			this.splitSetting.Panel1.SuspendLayout();
			this.splitSetting.Panel2.SuspendLayout();
			this.splitSetting.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGaussAlpha)).BeginInit();
			this.tabDefectLevel.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBlackDotSauvolaScale)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryThresholdHi)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryThresholdLo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBlackDotSauvolaMaskSize)).BeginInit();
			this.panelParam.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitImageView)).BeginInit();
			this.splitImageView.Panel1.SuspendLayout();
			this.splitImageView.Panel2.SuspendLayout();
			this.splitImageView.SuspendLayout();
			this.panelView.SuspendLayout();
			this.panelInfo.SuspendLayout();
			this.groupImage.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitSetting
			// 
			this.splitSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitSetting.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitSetting.Location = new System.Drawing.Point(12, 12);
			this.splitSetting.Name = "splitSetting";
			// 
			// splitSetting.Panel1
			// 
			this.splitSetting.Panel1.Controls.Add(this.btGetProcArea);
			this.splitSetting.Panel1.Controls.Add(this.textRecipeName);
			this.splitSetting.Panel1.Controls.Add(this.labelRecipeName);
			this.splitSetting.Panel1.Controls.Add(this.panel1);
			// 
			// splitSetting.Panel2
			// 
			this.splitSetting.Panel2.Controls.Add(this.panelParam);
			this.splitSetting.Size = new System.Drawing.Size(1300, 801);
			this.splitSetting.SplitterDistance = 394;
			this.splitSetting.TabIndex = 0;
			// 
			// btGetProcArea
			// 
			this.btGetProcArea.Location = new System.Drawing.Point(15, 731);
			this.btGetProcArea.Name = "btGetProcArea";
			this.btGetProcArea.Size = new System.Drawing.Size(140, 55);
			this.btGetProcArea.TabIndex = 13;
			this.btGetProcArea.Text = "処理範囲";
			this.btGetProcArea.UseVisualStyleBackColor = true;
			this.btGetProcArea.Click += new System.EventHandler(this.btGetProcArea_Click);
			// 
			// textRecipeName
			// 
			this.textRecipeName.Location = new System.Drawing.Point(99, 17);
			this.textRecipeName.Name = "textRecipeName";
			this.textRecipeName.Size = new System.Drawing.Size(281, 19);
			this.textRecipeName.TabIndex = 17;
			// 
			// labelRecipeName
			// 
			this.labelRecipeName.Location = new System.Drawing.Point(24, 17);
			this.labelRecipeName.Name = "labelRecipeName";
			this.labelRecipeName.Size = new System.Drawing.Size(85, 17);
			this.labelRecipeName.TabIndex = 16;
			this.labelRecipeName.Text = "品種名称";
			this.labelRecipeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.tabDefectLevel);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Location = new System.Drawing.Point(15, 50);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(365, 581);
			this.panel1.TabIndex = 14;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.textBox1);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Location = new System.Drawing.Point(11, 313);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(341, 69);
			this.groupBox4.TabIndex = 42;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "学習";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(94, 28);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(229, 19);
			this.textBox1.TabIndex = 34;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(22, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 17);
			this.label5.TabIndex = 33;
			this.label5.Text = "検査モデル";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkDifferenceImage);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.numGaussAlpha);
			this.groupBox2.Controls.Add(this.checkGaussEnable);
			this.groupBox2.Location = new System.Drawing.Point(12, 10);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(341, 87);
			this.groupBox2.TabIndex = 41;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "前処理";
			// 
			// checkDifferenceImage
			// 
			this.checkDifferenceImage.AutoSize = true;
			this.checkDifferenceImage.Location = new System.Drawing.Point(165, 18);
			this.checkDifferenceImage.Name = "checkDifferenceImage";
			this.checkDifferenceImage.Size = new System.Drawing.Size(158, 16);
			this.checkDifferenceImage.TabIndex = 0;
			this.checkDifferenceImage.Text = "画像上下で差分処理を行う";
			this.checkDifferenceImage.UseVisualStyleBackColor = true;
			this.checkDifferenceImage.Visible = false;
			this.checkDifferenceImage.CheckedChanged += new System.EventHandler(this.checkDifferenceImage_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 16);
			this.label1.TabIndex = 34;
			this.label1.Text = "Filter Alpha";
			// 
			// numGaussAlpha
			// 
			this.numGaussAlpha.Location = new System.Drawing.Point(146, 47);
			this.numGaussAlpha.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numGaussAlpha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numGaussAlpha.Name = "numGaussAlpha";
			this.numGaussAlpha.Size = new System.Drawing.Size(65, 19);
			this.numGaussAlpha.TabIndex = 35;
			this.numGaussAlpha.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// checkGaussEnable
			// 
			this.checkGaussEnable.AutoSize = true;
			this.checkGaussEnable.Location = new System.Drawing.Point(17, 27);
			this.checkGaussEnable.Name = "checkGaussEnable";
			this.checkGaussEnable.Size = new System.Drawing.Size(84, 16);
			this.checkGaussEnable.TabIndex = 2;
			this.checkGaussEnable.Text = "画像平滑化";
			this.checkGaussEnable.UseVisualStyleBackColor = true;
			// 
			// tabDefectLevel
			// 
			this.tabDefectLevel.Controls.Add(this.tabPage1);
			this.tabDefectLevel.Controls.Add(this.tabPage2);
			this.tabDefectLevel.Controls.Add(this.tabPage3);
			this.tabDefectLevel.Controls.Add(this.tabPage4);
			this.tabDefectLevel.Controls.Add(this.tabPage5);
			this.tabDefectLevel.Location = new System.Drawing.Point(10, 400);
			this.tabDefectLevel.Name = "tabDefectLevel";
			this.tabDefectLevel.SelectedIndex = 0;
			this.tabDefectLevel.Size = new System.Drawing.Size(342, 165);
			this.tabDefectLevel.TabIndex = 40;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.userDefectLevel0);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(334, 139);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Level 1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// userDefectLevel0
			// 
			this.userDefectLevel0.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userDefectLevel0.Location = new System.Drawing.Point(0, 0);
			this.userDefectLevel0.Name = "userDefectLevel0";
			this.userDefectLevel0.Param = blobJudgeParam1;
			this.userDefectLevel0.Size = new System.Drawing.Size(334, 139);
			this.userDefectLevel0.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.userDefectLevel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(334, 139);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Level 2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// userDefectLevel1
			// 
			this.userDefectLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userDefectLevel1.Location = new System.Drawing.Point(0, 0);
			this.userDefectLevel1.Name = "userDefectLevel1";
			this.userDefectLevel1.Param = blobJudgeParam2;
			this.userDefectLevel1.Size = new System.Drawing.Size(334, 139);
			this.userDefectLevel1.TabIndex = 1;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.userDefectLevel2);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(334, 139);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Level 3";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// userDefectLevel2
			// 
			this.userDefectLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userDefectLevel2.Location = new System.Drawing.Point(0, 0);
			this.userDefectLevel2.Name = "userDefectLevel2";
			this.userDefectLevel2.Param = blobJudgeParam3;
			this.userDefectLevel2.Size = new System.Drawing.Size(334, 139);
			this.userDefectLevel2.TabIndex = 1;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.userDefectLevel3);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(334, 139);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Level 4";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// userDefectLevel3
			// 
			this.userDefectLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userDefectLevel3.Location = new System.Drawing.Point(0, 0);
			this.userDefectLevel3.Name = "userDefectLevel3";
			this.userDefectLevel3.Param = blobJudgeParam4;
			this.userDefectLevel3.Size = new System.Drawing.Size(334, 139);
			this.userDefectLevel3.TabIndex = 1;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.userDefectLevel4);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(334, 139);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "Level 5";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// userDefectLevel4
			// 
			this.userDefectLevel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userDefectLevel4.Location = new System.Drawing.Point(0, 0);
			this.userDefectLevel4.Name = "userDefectLevel4";
			this.userDefectLevel4.Param = blobJudgeParam5;
			this.userDefectLevel4.Size = new System.Drawing.Size(334, 139);
			this.userDefectLevel4.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioBinarySimple);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.numBlackDotSauvolaScale);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.buttonMorphology);
			this.groupBox1.Controls.Add(this.numBinaryThresholdHi);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.numBinaryThresholdLo);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.numBlackDotSauvolaMaskSize);
			this.groupBox1.Controls.Add(this.radioBinarySauvola);
			this.groupBox1.Location = new System.Drawing.Point(10, 103);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(345, 204);
			this.groupBox1.TabIndex = 38;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "2値化";
			// 
			// radioBinarySimple
			// 
			this.radioBinarySimple.AutoSize = true;
			this.radioBinarySimple.Location = new System.Drawing.Point(21, 27);
			this.radioBinarySimple.Name = "radioBinarySimple";
			this.radioBinarySimple.Size = new System.Drawing.Size(77, 16);
			this.radioBinarySimple.TabIndex = 28;
			this.radioBinarySimple.TabStop = true;
			this.radioBinarySimple.Text = "単純2値化";
			this.radioBinarySimple.UseVisualStyleBackColor = true;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(216, 51);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(13, 14);
			this.label17.TabIndex = 37;
			this.label17.Text = "～";
			// 
			// numBlackDotSauvolaScale
			// 
			this.numBlackDotSauvolaScale.DecimalPlaces = 2;
			this.numBlackDotSauvolaScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numBlackDotSauvolaScale.Location = new System.Drawing.Point(146, 122);
			this.numBlackDotSauvolaScale.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numBlackDotSauvolaScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numBlackDotSauvolaScale.Name = "numBlackDotSauvolaScale";
			this.numBlackDotSauvolaScale.Size = new System.Drawing.Size(65, 19);
			this.numBlackDotSauvolaScale.TabIndex = 35;
			this.numBlackDotSauvolaScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioBinLight);
			this.groupBox3.Controls.Add(this.radioBinDark);
			this.groupBox3.Location = new System.Drawing.Point(240, 76);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(83, 72);
			this.groupBox3.TabIndex = 25;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "検出色";
			// 
			// radioBinLight
			// 
			this.radioBinLight.AutoSize = true;
			this.radioBinLight.Location = new System.Drawing.Point(17, 44);
			this.radioBinLight.Name = "radioBinLight";
			this.radioBinLight.Size = new System.Drawing.Size(47, 16);
			this.radioBinLight.TabIndex = 1;
			this.radioBinLight.TabStop = true;
			this.radioBinLight.Text = "明側";
			this.radioBinLight.UseVisualStyleBackColor = true;
			// 
			// radioBinDark
			// 
			this.radioBinDark.AutoSize = true;
			this.radioBinDark.Location = new System.Drawing.Point(17, 22);
			this.radioBinDark.Name = "radioBinDark";
			this.radioBinDark.Size = new System.Drawing.Size(47, 16);
			this.radioBinDark.TabIndex = 0;
			this.radioBinDark.TabStop = true;
			this.radioBinDark.Text = "暗側";
			this.radioBinDark.UseVisualStyleBackColor = true;
			// 
			// buttonMorphology
			// 
			this.buttonMorphology.Location = new System.Drawing.Point(19, 156);
			this.buttonMorphology.Name = "buttonMorphology";
			this.buttonMorphology.Size = new System.Drawing.Size(122, 33);
			this.buttonMorphology.TabIndex = 28;
			this.buttonMorphology.Text = "モルフォロジ";
			this.buttonMorphology.UseVisualStyleBackColor = true;
			this.buttonMorphology.Click += new System.EventHandler(this.buttonMorphology_Click);
			// 
			// numBinaryThresholdHi
			// 
			this.numBinaryThresholdHi.Location = new System.Drawing.Point(240, 49);
			this.numBinaryThresholdHi.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numBinaryThresholdHi.Name = "numBinaryThresholdHi";
			this.numBinaryThresholdHi.Size = new System.Drawing.Size(65, 19);
			this.numBinaryThresholdHi.TabIndex = 36;
			this.numBinaryThresholdHi.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(48, 125);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 16);
			this.label4.TabIndex = 34;
			this.label4.Text = "Filter Scale";
			// 
			// numBinaryThresholdLo
			// 
			this.numBinaryThresholdLo.Location = new System.Drawing.Point(146, 49);
			this.numBinaryThresholdLo.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numBinaryThresholdLo.Name = "numBinaryThresholdLo";
			this.numBinaryThresholdLo.Size = new System.Drawing.Size(65, 19);
			this.numBinaryThresholdLo.TabIndex = 30;
			this.numBinaryThresholdLo.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(47, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "2値化 Filter Size";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(48, 52);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(102, 15);
			this.label13.TabIndex = 29;
			this.label13.Text = "2値化しきい値";
			// 
			// numBlackDotSauvolaMaskSize
			// 
			this.numBlackDotSauvolaMaskSize.Location = new System.Drawing.Point(146, 98);
			this.numBlackDotSauvolaMaskSize.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numBlackDotSauvolaMaskSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.numBlackDotSauvolaMaskSize.Name = "numBlackDotSauvolaMaskSize";
			this.numBlackDotSauvolaMaskSize.Size = new System.Drawing.Size(65, 19);
			this.numBlackDotSauvolaMaskSize.TabIndex = 33;
			this.numBlackDotSauvolaMaskSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// radioBinarySauvola
			// 
			this.radioBinarySauvola.AutoSize = true;
			this.radioBinarySauvola.Location = new System.Drawing.Point(21, 76);
			this.radioBinarySauvola.Name = "radioBinarySauvola";
			this.radioBinarySauvola.Size = new System.Drawing.Size(128, 16);
			this.radioBinarySauvola.TabIndex = 31;
			this.radioBinarySauvola.TabStop = true;
			this.radioBinarySauvola.Text = "局所平均による2値化";
			this.radioBinarySauvola.UseVisualStyleBackColor = true;
			// 
			// panelParam
			// 
			this.panelParam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelParam.Controls.Add(this.splitImageView);
			this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelParam.Location = new System.Drawing.Point(0, 0);
			this.panelParam.Name = "panelParam";
			this.panelParam.Size = new System.Drawing.Size(902, 801);
			this.panelParam.TabIndex = 13;
			// 
			// splitImageView
			// 
			this.splitImageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitImageView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitImageView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitImageView.Location = new System.Drawing.Point(0, 0);
			this.splitImageView.Name = "splitImageView";
			this.splitImageView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitImageView.Panel1
			// 
			this.splitImageView.Panel1.Controls.Add(this.panelView);
			this.splitImageView.Panel1MinSize = 300;
			// 
			// splitImageView.Panel2
			// 
			this.splitImageView.Panel2.Controls.Add(this.panelInfo);
			this.splitImageView.Panel2MinSize = 200;
			this.splitImageView.Size = new System.Drawing.Size(898, 797);
			this.splitImageView.SplitterDistance = 577;
			this.splitImageView.TabIndex = 0;
			// 
			// panelView
			// 
			this.panelView.Controls.Add(this.checkShowProfile);
			this.panelView.Controls.Add(this.labelLocate);
			this.panelView.Controls.Add(this.hCamImage);
			this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelView.Location = new System.Drawing.Point(0, 0);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(896, 575);
			this.panelView.TabIndex = 0;
			// 
			// checkShowProfile
			// 
			this.checkShowProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowProfile.AutoSize = true;
			this.checkShowProfile.Location = new System.Drawing.Point(785, 548);
			this.checkShowProfile.Name = "checkShowProfile";
			this.checkShowProfile.Size = new System.Drawing.Size(100, 16);
			this.checkShowProfile.TabIndex = 18;
			this.checkShowProfile.Text = "プロファイル波形";
			this.checkShowProfile.UseVisualStyleBackColor = true;
			this.checkShowProfile.CheckedChanged += new System.EventHandler(this.checkShowProfile_CheckedChanged);
			// 
			// labelLocate
			// 
			this.labelLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelLocate.AutoSize = true;
			this.labelLocate.Location = new System.Drawing.Point(16, 548);
			this.labelLocate.Name = "labelLocate";
			this.labelLocate.Size = new System.Drawing.Size(39, 12);
			this.labelLocate.TabIndex = 3;
			this.labelLocate.Text = "(X,Y,B)";
			// 
			// hCamImage
			// 
			this.hCamImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hCamImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hCamImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hCamImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.hCamImage.HDoubleClickToFitContent = true;
			this.hCamImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hCamImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hCamImage.HKeepAspectRatio = true;
			this.hCamImage.HMoveContent = true;
			this.hCamImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hCamImage.Location = new System.Drawing.Point(11, 15);
			this.hCamImage.Margin = new System.Windows.Forms.Padding(0);
			this.hCamImage.Name = "hCamImage";
			this.hCamImage.Size = new System.Drawing.Size(874, 528);
			this.hCamImage.TabIndex = 2;
			this.hCamImage.WindowSize = new System.Drawing.Size(874, 528);
			this.hCamImage.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hCamImage_HMouseMove);
			this.hCamImage.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hCamImage_HMouseUp);
			this.hCamImage.HMouseDoubleClick += new HalconDotNet.HMouseEventHandler(this.hCamImage_HMouseDoubleClick);
			this.hCamImage.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hCamImage_HMouseWheel);
			// 
			// panelInfo
			// 
			this.panelInfo.Controls.Add(this.radioShowImgBase);
			this.panelInfo.Controls.Add(this.butRecordBase);
			this.panelInfo.Controls.Add(this.groupImage);
			this.panelInfo.Controls.Add(this.listDefect);
			this.panelInfo.Controls.Add(this.checkBinaryImg);
			this.panelInfo.Controls.Add(this.checkShowResult);
			this.panelInfo.Controls.Add(this.btnCancel);
			this.panelInfo.Controls.Add(this.btnOk);
			this.panelInfo.Controls.Add(this.checkShowArea);
			this.panelInfo.Controls.Add(this.butProcTrial);
			this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelInfo.Location = new System.Drawing.Point(0, 0);
			this.panelInfo.Name = "panelInfo";
			this.panelInfo.Size = new System.Drawing.Size(896, 214);
			this.panelInfo.TabIndex = 0;
			// 
			// radioShowImgBase
			// 
			this.radioShowImgBase.AutoSize = true;
			this.radioShowImgBase.Location = new System.Drawing.Point(500, 122);
			this.radioShowImgBase.Name = "radioShowImgBase";
			this.radioShowImgBase.Size = new System.Drawing.Size(83, 16);
			this.radioShowImgBase.TabIndex = 2;
			this.radioShowImgBase.TabStop = true;
			this.radioShowImgBase.Text = "差分元画像";
			this.radioShowImgBase.UseVisualStyleBackColor = true;
			this.radioShowImgBase.Visible = false;
			// 
			// butRecordBase
			// 
			this.butRecordBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRecordBase.Location = new System.Drawing.Point(500, 147);
			this.butRecordBase.Name = "butRecordBase";
			this.butRecordBase.Size = new System.Drawing.Size(69, 31);
			this.butRecordBase.TabIndex = 18;
			this.butRecordBase.Text = "記録";
			this.butRecordBase.UseVisualStyleBackColor = true;
			this.butRecordBase.Visible = false;
			this.butRecordBase.Click += new System.EventHandler(this.butRecordBase_Click);
			// 
			// groupImage
			// 
			this.groupImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupImage.Controls.Add(this.radioShowImgProc);
			this.groupImage.Controls.Add(this.radioShowImgSrc);
			this.groupImage.Location = new System.Drawing.Point(480, 9);
			this.groupImage.Name = "groupImage";
			this.groupImage.Size = new System.Drawing.Size(122, 78);
			this.groupImage.TabIndex = 17;
			this.groupImage.TabStop = false;
			this.groupImage.Text = "画像";
			// 
			// radioShowImgProc
			// 
			this.radioShowImgProc.AutoSize = true;
			this.radioShowImgProc.Location = new System.Drawing.Point(20, 44);
			this.radioShowImgProc.Name = "radioShowImgProc";
			this.radioShowImgProc.Size = new System.Drawing.Size(71, 16);
			this.radioShowImgProc.TabIndex = 1;
			this.radioShowImgProc.TabStop = true;
			this.radioShowImgProc.Text = "処理画像";
			this.radioShowImgProc.UseVisualStyleBackColor = true;
			this.radioShowImgProc.CheckedChanged += new System.EventHandler(this.radioShowImgProc_CheckedChanged);
			// 
			// radioShowImgSrc
			// 
			this.radioShowImgSrc.AutoSize = true;
			this.radioShowImgSrc.Location = new System.Drawing.Point(20, 22);
			this.radioShowImgSrc.Name = "radioShowImgSrc";
			this.radioShowImgSrc.Size = new System.Drawing.Size(59, 16);
			this.radioShowImgSrc.TabIndex = 0;
			this.radioShowImgSrc.TabStop = true;
			this.radioShowImgSrc.Text = "元画像";
			this.radioShowImgSrc.UseVisualStyleBackColor = true;
			this.radioShowImgSrc.CheckedChanged += new System.EventHandler(this.radioShowImgSrc_CheckedChanged);
			// 
			// listDefect
			// 
			this.listDefect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listDefect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNo,
            this.columnLotate,
            this.columnWidth,
            this.columnLength,
            this.columnHeaderArea,
            this.columnBright});
			this.listDefect.FullRowSelect = true;
			this.listDefect.GridLines = true;
			this.listDefect.HideSelection = false;
			this.listDefect.Location = new System.Drawing.Point(11, 16);
			this.listDefect.Name = "listDefect";
			this.listDefect.Size = new System.Drawing.Size(456, 186);
			this.listDefect.TabIndex = 16;
			this.listDefect.UseCompatibleStateImageBehavior = false;
			this.listDefect.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderNo
			// 
			this.columnHeaderNo.Text = "No";
			this.columnHeaderNo.Width = 30;
			// 
			// columnLotate
			// 
			this.columnLotate.Text = "位置";
			this.columnLotate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnLotate.Width = 70;
			// 
			// columnWidth
			// 
			this.columnWidth.Text = "幅";
			this.columnWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnLength
			// 
			this.columnLength.Text = "長さ";
			this.columnLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeaderArea
			// 
			this.columnHeaderArea.Text = "面積";
			this.columnHeaderArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeaderArea.Width = 70;
			// 
			// columnBright
			// 
			this.columnBright.Text = "輝度値";
			this.columnBright.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnBright.Width = 70;
			// 
			// checkBinaryImg
			// 
			this.checkBinaryImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBinaryImg.AutoSize = true;
			this.checkBinaryImg.Location = new System.Drawing.Point(616, 38);
			this.checkBinaryImg.Name = "checkBinaryImg";
			this.checkBinaryImg.Size = new System.Drawing.Size(66, 16);
			this.checkBinaryImg.TabIndex = 15;
			this.checkBinaryImg.Text = "2値処理";
			this.checkBinaryImg.UseVisualStyleBackColor = true;
			this.checkBinaryImg.CheckedChanged += new System.EventHandler(this.checkBinaryImg_CheckedChanged);
			// 
			// checkShowResult
			// 
			this.checkShowResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowResult.AutoSize = true;
			this.checkShowResult.Checked = true;
			this.checkShowResult.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowResult.Location = new System.Drawing.Point(616, 60);
			this.checkShowResult.Name = "checkShowResult";
			this.checkShowResult.Size = new System.Drawing.Size(72, 16);
			this.checkShowResult.TabIndex = 14;
			this.checkShowResult.Text = "処理結果";
			this.checkShowResult.UseVisualStyleBackColor = true;
			this.checkShowResult.CheckedChanged += new System.EventHandler(this.checkShowResult_CheckedChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(779, 161);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(106, 41);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(667, 161);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(106, 41);
			this.btnOk.TabIndex = 11;
			this.btnOk.Text = "保存";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// checkShowArea
			// 
			this.checkShowArea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowArea.AutoSize = true;
			this.checkShowArea.Location = new System.Drawing.Point(616, 16);
			this.checkShowArea.Name = "checkShowArea";
			this.checkShowArea.Size = new System.Drawing.Size(72, 16);
			this.checkShowArea.TabIndex = 7;
			this.checkShowArea.Text = "検査範囲";
			this.checkShowArea.UseVisualStyleBackColor = true;
			this.checkShowArea.CheckedChanged += new System.EventHandler(this.checkShowArea_CheckedChanged);
			// 
			// butProcTrial
			// 
			this.butProcTrial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butProcTrial.Location = new System.Drawing.Point(724, 16);
			this.butProcTrial.Name = "butProcTrial";
			this.butProcTrial.Size = new System.Drawing.Size(161, 55);
			this.butProcTrial.TabIndex = 6;
			this.butProcTrial.Text = "試 行";
			this.butProcTrial.UseVisualStyleBackColor = true;
			this.butProcTrial.Click += new System.EventHandler(this.butProcTrial_Click);
			// 
			// FormInspectSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1324, 825);
			this.Controls.Add(this.splitSetting);
			this.MinimumSize = new System.Drawing.Size(900, 680);
			this.Name = "FormInspectSetting";
			this.ShowIcon = false;
			this.Text = "検査設定";
			this.Activated += new System.EventHandler(this.FormInspectSetting_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInspectSetting_FormClosing);
			this.Load += new System.EventHandler(this.FormInspectSetting_Load);
			this.splitSetting.Panel1.ResumeLayout(false);
			this.splitSetting.Panel1.PerformLayout();
			this.splitSetting.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitSetting)).EndInit();
			this.splitSetting.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGaussAlpha)).EndInit();
			this.tabDefectLevel.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBlackDotSauvolaScale)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryThresholdHi)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryThresholdLo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBlackDotSauvolaMaskSize)).EndInit();
			this.panelParam.ResumeLayout(false);
			this.splitImageView.Panel1.ResumeLayout(false);
			this.splitImageView.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitImageView)).EndInit();
			this.splitImageView.ResumeLayout(false);
			this.panelView.ResumeLayout(false);
			this.panelView.PerformLayout();
			this.panelInfo.ResumeLayout(false);
			this.panelInfo.PerformLayout();
			this.groupImage.ResumeLayout(false);
			this.groupImage.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitSetting;
        private System.Windows.Forms.SplitContainer splitImageView;
        private System.Windows.Forms.Button butProcTrial;
        private System.Windows.Forms.Button btGetProcArea;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox checkShowArea;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelView;
        private HalconDotNet.HSmartWindowControl hCamImage;
        private System.Windows.Forms.CheckBox checkBinaryImg;
        private System.Windows.Forms.CheckBox checkShowResult;
        private System.Windows.Forms.TextBox textRecipeName;
        private System.Windows.Forms.Label labelRecipeName;
        private System.Windows.Forms.Label labelLocate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBinarySimple;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numBlackDotSauvolaScale;
        private System.Windows.Forms.NumericUpDown numBinaryThresholdHi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBinaryThresholdLo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numBlackDotSauvolaMaskSize;
        private System.Windows.Forms.RadioButton radioBinarySauvola;
        private System.Windows.Forms.Button buttonMorphology;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioBinLight;
        private System.Windows.Forms.RadioButton radioBinDark;
        private System.Windows.Forms.TabControl tabDefectLevel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private UserSetDefectLevel userDefectLevel0;
        private UserSetDefectLevel userDefectLevel1;
        private UserSetDefectLevel userDefectLevel2;
        private UserSetDefectLevel userDefectLevel3;
        private UserSetDefectLevel userDefectLevel4;
        private System.Windows.Forms.ListView listDefect;
        private System.Windows.Forms.ColumnHeader columnHeaderNo;
        private System.Windows.Forms.ColumnHeader columnHeaderArea;
        private System.Windows.Forms.ColumnHeader columnBright;
        private System.Windows.Forms.ColumnHeader columnLotate;
        private System.Windows.Forms.ColumnHeader columnWidth;
        private System.Windows.Forms.ColumnHeader columnLength;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkDifferenceImage;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.RadioButton radioShowImgProc;
        private System.Windows.Forms.RadioButton radioShowImgSrc;
		private System.Windows.Forms.CheckBox checkShowProfile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numGaussAlpha;
		private System.Windows.Forms.CheckBox checkGaussEnable;
		private System.Windows.Forms.RadioButton radioShowImgBase;
		private System.Windows.Forms.Button butRecordBase;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
	}
}