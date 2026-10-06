namespace HFVisionInspect
{
    partial class FormGetArea
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
			this.labelMousePos = new System.Windows.Forms.Label();
			this.checkBoxFitImage = new System.Windows.Forms.CheckBox();
			this.panelDisplayImage = new System.Windows.Forms.Panel();
			this.hWindowControlDisplayImage = new HalconDotNet.HWindowControl();
			this.GroupBoxRectAdjust = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboBoxAdjustAmount = new System.Windows.Forms.ComboBox();
			this.RadioButtonAdjustDecr = new System.Windows.Forms.RadioButton();
			this.RadioButtonAdjustIncri = new System.Windows.Forms.RadioButton();
			this.GroupBoxRectLogomarkPosition = new System.Windows.Forms.GroupBox();
			this.textBoxRectH = new System.Windows.Forms.TextBox();
			this.textBoxRectW = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.TextBoxRectDispEY = new System.Windows.Forms.TextBox();
			this.TextBoxRectDispEX = new System.Windows.Forms.TextBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.TextBoxRectDispSY = new System.Windows.Forms.TextBox();
			this.TextBoxRectDispSX = new System.Windows.Forms.TextBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.RadioButtonAdjustPosition = new System.Windows.Forms.RadioButton();
			this.ButtonRectUp = new System.Windows.Forms.Button();
			this.ButtonDown = new System.Windows.Forms.Button();
			this.ButtonRight = new System.Windows.Forms.Button();
			this.ButtonLeft = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.textWorkEdgePos = new System.Windows.Forms.TextBox();
			this.labelWorkEdgePos = new System.Windows.Forms.Label();
			this.numAreaLeft = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.numAreaRight = new System.Windows.Forms.NumericUpDown();
			this.label12 = new System.Windows.Forms.Label();
			this.numAreaTop = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.numAreaBottom = new System.Windows.Forms.NumericUpDown();
			this.groupEdge = new System.Windows.Forms.GroupBox();
			this.labelError = new System.Windows.Forms.Label();
			this.butAutoAdjustTry = new System.Windows.Forms.Button();
			this.propertyEdgeParameter = new System.Windows.Forms.PropertyGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panelDisplayImage.SuspendLayout();
			this.GroupBoxRectAdjust.SuspendLayout();
			this.GroupBoxRectLogomarkPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAreaLeft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaRight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaTop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaBottom)).BeginInit();
			this.groupEdge.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelMousePos
			// 
			this.labelMousePos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMousePos.Location = new System.Drawing.Point(237, 437);
			this.labelMousePos.Name = "labelMousePos";
			this.labelMousePos.Size = new System.Drawing.Size(81, 14);
			this.labelMousePos.TabIndex = 6;
			this.labelMousePos.Text = "(---,---)";
			this.labelMousePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkBoxFitImage
			// 
			this.checkBoxFitImage.AutoSize = true;
			this.checkBoxFitImage.Checked = true;
			this.checkBoxFitImage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxFitImage.Location = new System.Drawing.Point(140, 28);
			this.checkBoxFitImage.Name = "checkBoxFitImage";
			this.checkBoxFitImage.Size = new System.Drawing.Size(54, 16);
			this.checkBoxFitImage.TabIndex = 1;
			this.checkBoxFitImage.Text = "フィット";
			this.checkBoxFitImage.UseVisualStyleBackColor = true;
			this.checkBoxFitImage.Visible = false;
			this.checkBoxFitImage.CheckedChanged += new System.EventHandler(this.checkBoxFitImage_CheckedChanged);
			// 
			// panelDisplayImage
			// 
			this.panelDisplayImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelDisplayImage.AutoScroll = true;
			this.panelDisplayImage.Controls.Add(this.hWindowControlDisplayImage);
			this.panelDisplayImage.Location = new System.Drawing.Point(239, 12);
			this.panelDisplayImage.Name = "panelDisplayImage";
			this.panelDisplayImage.Size = new System.Drawing.Size(794, 422);
			this.panelDisplayImage.TabIndex = 5;
			this.panelDisplayImage.Resize += new System.EventHandler(this.panelDisplayImage_Resize);
			// 
			// hWindowControlDisplayImage
			// 
			this.hWindowControlDisplayImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hWindowControlDisplayImage.BackColor = System.Drawing.Color.Black;
			this.hWindowControlDisplayImage.BorderColor = System.Drawing.Color.Black;
			this.hWindowControlDisplayImage.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWindowControlDisplayImage.Location = new System.Drawing.Point(0, 0);
			this.hWindowControlDisplayImage.Name = "hWindowControlDisplayImage";
			this.hWindowControlDisplayImage.Size = new System.Drawing.Size(498, 301);
			this.hWindowControlDisplayImage.TabIndex = 0;
			this.hWindowControlDisplayImage.WindowSize = new System.Drawing.Size(498, 301);
			this.hWindowControlDisplayImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hWindowControlDisplayImage_MouseDown);
			this.hWindowControlDisplayImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hWindowControlDisplayImage_MouseMove);
			this.hWindowControlDisplayImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hWindowControlDisplayImage_MouseUp);
			// 
			// GroupBoxRectAdjust
			// 
			this.GroupBoxRectAdjust.Controls.Add(this.label7);
			this.GroupBoxRectAdjust.Controls.Add(this.comboBoxAdjustAmount);
			this.GroupBoxRectAdjust.Controls.Add(this.RadioButtonAdjustDecr);
			this.GroupBoxRectAdjust.Controls.Add(this.RadioButtonAdjustIncri);
			this.GroupBoxRectAdjust.Controls.Add(this.GroupBoxRectLogomarkPosition);
			this.GroupBoxRectAdjust.Controls.Add(this.RadioButtonAdjustPosition);
			this.GroupBoxRectAdjust.Controls.Add(this.ButtonRectUp);
			this.GroupBoxRectAdjust.Controls.Add(this.ButtonDown);
			this.GroupBoxRectAdjust.Controls.Add(this.ButtonRight);
			this.GroupBoxRectAdjust.Controls.Add(this.ButtonLeft);
			this.GroupBoxRectAdjust.Location = new System.Drawing.Point(12, 12);
			this.GroupBoxRectAdjust.Name = "GroupBoxRectAdjust";
			this.GroupBoxRectAdjust.Size = new System.Drawing.Size(212, 286);
			this.GroupBoxRectAdjust.TabIndex = 0;
			this.GroupBoxRectAdjust.TabStop = false;
			this.GroupBoxRectAdjust.Text = "位置調整";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(128, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(43, 12);
			this.label7.TabIndex = 4;
			this.label7.Text = "調整量:";
			// 
			// comboBoxAdjustAmount
			// 
			this.comboBoxAdjustAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAdjustAmount.FormattingEnabled = true;
			this.comboBoxAdjustAmount.Location = new System.Drawing.Point(133, 40);
			this.comboBoxAdjustAmount.Name = "comboBoxAdjustAmount";
			this.comboBoxAdjustAmount.Size = new System.Drawing.Size(45, 20);
			this.comboBoxAdjustAmount.TabIndex = 5;
			// 
			// RadioButtonAdjustDecr
			// 
			this.RadioButtonAdjustDecr.AutoSize = true;
			this.RadioButtonAdjustDecr.Location = new System.Drawing.Point(137, 127);
			this.RadioButtonAdjustDecr.Name = "RadioButtonAdjustDecr";
			this.RadioButtonAdjustDecr.Size = new System.Drawing.Size(49, 16);
			this.RadioButtonAdjustDecr.TabIndex = 8;
			this.RadioButtonAdjustDecr.Text = "小さく";
			this.RadioButtonAdjustDecr.UseVisualStyleBackColor = true;
			this.RadioButtonAdjustDecr.Visible = false;
			// 
			// RadioButtonAdjustIncri
			// 
			this.RadioButtonAdjustIncri.AutoSize = true;
			this.RadioButtonAdjustIncri.Location = new System.Drawing.Point(137, 104);
			this.RadioButtonAdjustIncri.Name = "RadioButtonAdjustIncri";
			this.RadioButtonAdjustIncri.Size = new System.Drawing.Size(50, 16);
			this.RadioButtonAdjustIncri.TabIndex = 7;
			this.RadioButtonAdjustIncri.Text = "大きく";
			this.RadioButtonAdjustIncri.UseVisualStyleBackColor = true;
			this.RadioButtonAdjustIncri.Visible = false;
			// 
			// GroupBoxRectLogomarkPosition
			// 
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.textBoxRectH);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.textBoxRectW);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.label9);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.label8);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label4);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label5);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.TextBoxRectDispEY);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.TextBoxRectDispEX);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label6);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label3);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label2);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.TextBoxRectDispSY);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.TextBoxRectDispSX);
			this.GroupBoxRectLogomarkPosition.Controls.Add(this.Label1);
			this.GroupBoxRectLogomarkPosition.Location = new System.Drawing.Point(14, 164);
			this.GroupBoxRectLogomarkPosition.Name = "GroupBoxRectLogomarkPosition";
			this.GroupBoxRectLogomarkPosition.Size = new System.Drawing.Size(185, 106);
			this.GroupBoxRectLogomarkPosition.TabIndex = 9;
			this.GroupBoxRectLogomarkPosition.TabStop = false;
			this.GroupBoxRectLogomarkPosition.Text = "矩形座標";
			// 
			// textBoxRectH
			// 
			this.textBoxRectH.Location = new System.Drawing.Point(130, 77);
			this.textBoxRectH.Name = "textBoxRectH";
			this.textBoxRectH.ReadOnly = true;
			this.textBoxRectH.Size = new System.Drawing.Size(40, 19);
			this.textBoxRectH.TabIndex = 13;
			this.textBoxRectH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRectW
			// 
			this.textBoxRectW.Location = new System.Drawing.Point(53, 77);
			this.textBoxRectW.Name = "textBoxRectW";
			this.textBoxRectW.ReadOnly = true;
			this.textBoxRectW.Size = new System.Drawing.Size(40, 19);
			this.textBoxRectW.TabIndex = 11;
			this.textBoxRectW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(114, 80);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(15, 12);
			this.label9.TabIndex = 12;
			this.label9.Text = "H:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(31, 80);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(16, 12);
			this.label8.TabIndex = 10;
			this.label8.Text = "W:";
			// 
			// Label4
			// 
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(161, 47);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(9, 12);
			this.Label4.TabIndex = 9;
			this.Label4.Text = ")";
			// 
			// Label5
			// 
			this.Label5.AutoSize = true;
			this.Label5.Location = new System.Drawing.Point(108, 48);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(7, 12);
			this.Label5.TabIndex = 7;
			this.Label5.Text = ",";
			// 
			// TextBoxRectDispEY
			// 
			this.TextBoxRectDispEY.Location = new System.Drawing.Point(116, 44);
			this.TextBoxRectDispEY.Name = "TextBoxRectDispEY";
			this.TextBoxRectDispEY.ReadOnly = true;
			this.TextBoxRectDispEY.Size = new System.Drawing.Size(40, 19);
			this.TextBoxRectDispEY.TabIndex = 8;
			this.TextBoxRectDispEY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TextBoxRectDispEX
			// 
			this.TextBoxRectDispEX.Location = new System.Drawing.Point(66, 44);
			this.TextBoxRectDispEX.Name = "TextBoxRectDispEX";
			this.TextBoxRectDispEX.ReadOnly = true;
			this.TextBoxRectDispEX.Size = new System.Drawing.Size(40, 19);
			this.TextBoxRectDispEX.TabIndex = 6;
			this.TextBoxRectDispEX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// Label6
			// 
			this.Label6.AutoSize = true;
			this.Label6.Location = new System.Drawing.Point(7, 47);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(59, 12);
			this.Label6.TabIndex = 5;
			this.Label6.Text = "右下座標:(";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(160, 22);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(9, 12);
			this.Label3.TabIndex = 4;
			this.Label3.Text = ")";
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(107, 24);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(7, 12);
			this.Label2.TabIndex = 2;
			this.Label2.Text = ",";
			// 
			// TextBoxRectDispSY
			// 
			this.TextBoxRectDispSY.Location = new System.Drawing.Point(116, 19);
			this.TextBoxRectDispSY.Name = "TextBoxRectDispSY";
			this.TextBoxRectDispSY.ReadOnly = true;
			this.TextBoxRectDispSY.Size = new System.Drawing.Size(40, 19);
			this.TextBoxRectDispSY.TabIndex = 3;
			this.TextBoxRectDispSY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TextBoxRectDispSX
			// 
			this.TextBoxRectDispSX.Location = new System.Drawing.Point(65, 19);
			this.TextBoxRectDispSX.Name = "TextBoxRectDispSX";
			this.TextBoxRectDispSX.ReadOnly = true;
			this.TextBoxRectDispSX.Size = new System.Drawing.Size(40, 19);
			this.TextBoxRectDispSX.TabIndex = 1;
			this.TextBoxRectDispSX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(7, 22);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(59, 12);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "左上座標:(";
			// 
			// RadioButtonAdjustPosition
			// 
			this.RadioButtonAdjustPosition.AutoSize = true;
			this.RadioButtonAdjustPosition.Checked = true;
			this.RadioButtonAdjustPosition.Location = new System.Drawing.Point(137, 79);
			this.RadioButtonAdjustPosition.Name = "RadioButtonAdjustPosition";
			this.RadioButtonAdjustPosition.Size = new System.Drawing.Size(47, 16);
			this.RadioButtonAdjustPosition.TabIndex = 6;
			this.RadioButtonAdjustPosition.TabStop = true;
			this.RadioButtonAdjustPosition.Text = "移動";
			this.RadioButtonAdjustPosition.UseVisualStyleBackColor = true;
			this.RadioButtonAdjustPosition.Visible = false;
			// 
			// ButtonRectUp
			// 
			this.ButtonRectUp.Location = new System.Drawing.Point(50, 24);
			this.ButtonRectUp.Name = "ButtonRectUp";
			this.ButtonRectUp.Size = new System.Drawing.Size(36, 36);
			this.ButtonRectUp.TabIndex = 0;
			this.ButtonRectUp.Text = "↑";
			this.ButtonRectUp.UseVisualStyleBackColor = true;
			this.ButtonRectUp.Click += new System.EventHandler(this.ButtonRectUp_Click);
			// 
			// ButtonDown
			// 
			this.ButtonDown.Location = new System.Drawing.Point(50, 96);
			this.ButtonDown.Name = "ButtonDown";
			this.ButtonDown.Size = new System.Drawing.Size(36, 36);
			this.ButtonDown.TabIndex = 3;
			this.ButtonDown.Text = "↓";
			this.ButtonDown.UseVisualStyleBackColor = true;
			this.ButtonDown.Click += new System.EventHandler(this.ButtonDown_Click);
			// 
			// ButtonRight
			// 
			this.ButtonRight.Location = new System.Drawing.Point(85, 60);
			this.ButtonRight.Name = "ButtonRight";
			this.ButtonRight.Size = new System.Drawing.Size(36, 36);
			this.ButtonRight.TabIndex = 2;
			this.ButtonRight.Text = "→";
			this.ButtonRight.UseVisualStyleBackColor = true;
			this.ButtonRight.Click += new System.EventHandler(this.ButtonRight_Click);
			// 
			// ButtonLeft
			// 
			this.ButtonLeft.Location = new System.Drawing.Point(14, 60);
			this.ButtonLeft.Name = "ButtonLeft";
			this.ButtonLeft.Size = new System.Drawing.Size(36, 36);
			this.ButtonLeft.TabIndex = 1;
			this.ButtonLeft.Text = "←";
			this.ButtonLeft.UseVisualStyleBackColor = true;
			this.ButtonLeft.Click += new System.EventHandler(this.ButtonLeft_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(810, 542);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(100, 43);
			this.buttonOk.TabIndex = 4;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.Location = new System.Drawing.Point(922, 542);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(100, 43);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// textWorkEdgePos
			// 
			this.textWorkEdgePos.Location = new System.Drawing.Point(75, 26);
			this.textWorkEdgePos.Name = "textWorkEdgePos";
			this.textWorkEdgePos.ReadOnly = true;
			this.textWorkEdgePos.Size = new System.Drawing.Size(39, 19);
			this.textWorkEdgePos.TabIndex = 2;
			this.textWorkEdgePos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textWorkEdgePos.Visible = false;
			// 
			// labelWorkEdgePos
			// 
			this.labelWorkEdgePos.AutoSize = true;
			this.labelWorkEdgePos.Location = new System.Drawing.Point(17, 29);
			this.labelWorkEdgePos.Name = "labelWorkEdgePos";
			this.labelWorkEdgePos.Size = new System.Drawing.Size(53, 12);
			this.labelWorkEdgePos.TabIndex = 1;
			this.labelWorkEdgePos.Text = "基準位置";
			this.labelWorkEdgePos.Visible = false;
			// 
			// numAreaLeft
			// 
			this.numAreaLeft.Location = new System.Drawing.Point(45, 54);
			this.numAreaLeft.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
			this.numAreaLeft.Name = "numAreaLeft";
			this.numAreaLeft.Size = new System.Drawing.Size(54, 19);
			this.numAreaLeft.TabIndex = 7;
			this.numAreaLeft.Visible = false;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(22, 56);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(17, 12);
			this.label10.TabIndex = 8;
			this.label10.Text = "左";
			this.label10.Visible = false;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(118, 56);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 12);
			this.label11.TabIndex = 10;
			this.label11.Text = "右";
			this.label11.Visible = false;
			// 
			// numAreaRight
			// 
			this.numAreaRight.Location = new System.Drawing.Point(141, 54);
			this.numAreaRight.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
			this.numAreaRight.Name = "numAreaRight";
			this.numAreaRight.Size = new System.Drawing.Size(63, 19);
			this.numAreaRight.TabIndex = 9;
			this.numAreaRight.Visible = false;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(22, 81);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(17, 12);
			this.label12.TabIndex = 12;
			this.label12.Text = "上";
			this.label12.Visible = false;
			// 
			// numAreaTop
			// 
			this.numAreaTop.Location = new System.Drawing.Point(45, 79);
			this.numAreaTop.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numAreaTop.Name = "numAreaTop";
			this.numAreaTop.Size = new System.Drawing.Size(54, 19);
			this.numAreaTop.TabIndex = 11;
			this.numAreaTop.Visible = false;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(118, 81);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(17, 12);
			this.label13.TabIndex = 14;
			this.label13.Text = "下";
			this.label13.Visible = false;
			// 
			// numAreaBottom
			// 
			this.numAreaBottom.Location = new System.Drawing.Point(141, 79);
			this.numAreaBottom.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numAreaBottom.Name = "numAreaBottom";
			this.numAreaBottom.Size = new System.Drawing.Size(63, 19);
			this.numAreaBottom.TabIndex = 13;
			this.numAreaBottom.Visible = false;
			// 
			// groupEdge
			// 
			this.groupEdge.Controls.Add(this.labelError);
			this.groupEdge.Controls.Add(this.butAutoAdjustTry);
			this.groupEdge.Controls.Add(this.propertyEdgeParameter);
			this.groupEdge.Location = new System.Drawing.Point(12, 304);
			this.groupEdge.Name = "groupEdge";
			this.groupEdge.Size = new System.Drawing.Size(212, 274);
			this.groupEdge.TabIndex = 15;
			this.groupEdge.TabStop = false;
			this.groupEdge.Text = "自動調整";
			// 
			// labelError
			// 
			this.labelError.AutoSize = true;
			this.labelError.ForeColor = System.Drawing.Color.Red;
			this.labelError.Location = new System.Drawing.Point(26, 242);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(30, 12);
			this.labelError.TabIndex = 18;
			this.labelError.Text = "Error";
			this.labelError.Visible = false;
			// 
			// butAutoAdjustTry
			// 
			this.butAutoAdjustTry.Location = new System.Drawing.Point(112, 236);
			this.butAutoAdjustTry.Name = "butAutoAdjustTry";
			this.butAutoAdjustTry.Size = new System.Drawing.Size(86, 32);
			this.butAutoAdjustTry.TabIndex = 17;
			this.butAutoAdjustTry.Text = "試行";
			this.butAutoAdjustTry.UseVisualStyleBackColor = true;
			this.butAutoAdjustTry.Click += new System.EventHandler(this.butAutoAdjustTry_Click);
			// 
			// propertyEdgeParameter
			// 
			this.propertyEdgeParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyEdgeParameter.HelpVisible = false;
			this.propertyEdgeParameter.Location = new System.Drawing.Point(12, 18);
			this.propertyEdgeParameter.Name = "propertyEdgeParameter";
			this.propertyEdgeParameter.Size = new System.Drawing.Size(187, 208);
			this.propertyEdgeParameter.TabIndex = 16;
			this.propertyEdgeParameter.ToolbarVisible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelWorkEdgePos);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.checkBoxFitImage);
			this.groupBox1.Controls.Add(this.numAreaBottom);
			this.groupBox1.Controls.Add(this.textWorkEdgePos);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.numAreaLeft);
			this.groupBox1.Controls.Add(this.numAreaTop);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.numAreaRight);
			this.groupBox1.Location = new System.Drawing.Point(239, 464);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(224, 114);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			this.groupBox1.Visible = false;
			// 
			// FormGetArea
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1045, 597);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupEdge);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.GroupBoxRectAdjust);
			this.Controls.Add(this.labelMousePos);
			this.Controls.Add(this.panelDisplayImage);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormGetArea";
			this.ShowIcon = false;
			this.Text = "範囲設定";
			this.Load += new System.EventHandler(this.FormSnipRectangleUI_Load);
			this.panelDisplayImage.ResumeLayout(false);
			this.GroupBoxRectAdjust.ResumeLayout(false);
			this.GroupBoxRectAdjust.PerformLayout();
			this.GroupBoxRectLogomarkPosition.ResumeLayout(false);
			this.GroupBoxRectLogomarkPosition.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAreaLeft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaRight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaTop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAreaBottom)).EndInit();
			this.groupEdge.ResumeLayout(false);
			this.groupEdge.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMousePos;
        private System.Windows.Forms.CheckBox checkBoxFitImage;
        private System.Windows.Forms.Panel panelDisplayImage;
        internal System.Windows.Forms.GroupBox GroupBoxRectAdjust;
        internal System.Windows.Forms.RadioButton RadioButtonAdjustDecr;
        internal System.Windows.Forms.RadioButton RadioButtonAdjustIncri;
        internal System.Windows.Forms.GroupBox GroupBoxRectLogomarkPosition;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TextBoxRectDispEY;
        internal System.Windows.Forms.TextBox TextBoxRectDispEX;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox TextBoxRectDispSY;
        internal System.Windows.Forms.TextBox TextBoxRectDispSX;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.RadioButton RadioButtonAdjustPosition;
        internal System.Windows.Forms.Button ButtonRectUp;
        internal System.Windows.Forms.Button ButtonDown;
        internal System.Windows.Forms.Button ButtonRight;
        internal System.Windows.Forms.Button ButtonLeft;
        internal System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxAdjustAmount;
        internal System.Windows.Forms.TextBox textBoxRectH;
        internal System.Windows.Forms.TextBox textBoxRectW;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        internal System.Windows.Forms.TextBox textWorkEdgePos;
        internal System.Windows.Forms.Label labelWorkEdgePos;
        private HalconDotNet.HWindowControl hWindowControlDisplayImage;
        private System.Windows.Forms.NumericUpDown numAreaLeft;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numAreaRight;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numAreaTop;
        internal System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numAreaBottom;
		private System.Windows.Forms.GroupBox groupEdge;
		private System.Windows.Forms.PropertyGrid propertyEdgeParameter;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button butAutoAdjustTry;
		private System.Windows.Forms.Label labelError;
	}
}