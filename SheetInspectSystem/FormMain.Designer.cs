
namespace HFVisionInspect
{
    partial class FormMain
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			HFVisionInspect.InspectParameter inspectParameter1 = new HFVisionInspect.InspectParameter();
			HFVisionInspect.InspectParameter inspectParameter2 = new HFVisionInspect.InspectParameter();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.menuMainStrip = new System.Windows.Forms.MenuStrip();
			this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSystemChangeMode = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuSystemProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.menuLightLifeReset = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuSystemExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuInspect = new System.Windows.Forms.ToolStripMenuItem();
			this.menuInspectRecipeLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.menuInspectRecipeSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuInspectResultClear = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInfomation = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDioCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.statusMainStrip = new System.Windows.Forms.StatusStrip();
			this.panelOperation = new System.Windows.Forms.Panel();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.labelExposureLeft = new System.Windows.Forms.Label();
			this.labelExposureSetRight = new System.Windows.Forms.Label();
			this.labelExposureSetLeft = new System.Windows.Forms.Label();
			this.labelExposureRight = new System.Windows.Forms.Label();
			this.checkAutoExposure = new System.Windows.Forms.CheckBox();
			this.lblGrayMeanLeft = new System.Windows.Forms.Label();
			this.labelBrightRight = new System.Windows.Forms.Label();
			this.labelBrightLeft = new System.Windows.Forms.Label();
			this.lblGrayMeanRight = new System.Windows.Forms.Label();
			this.labelInspectState = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btLoadInspectResult = new System.Windows.Forms.Button();
			this.butResultSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btInspectSetRight = new System.Windows.Forms.Button();
			this.textInspectID = new System.Windows.Forms.TextBox();
			this.labelLifeHour = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btInspectSetLeft = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkLightPower = new System.Windows.Forms.CheckBox();
			this.panelInspect = new System.Windows.Forms.Panel();
			this.butInspectStop = new System.Windows.Forms.Button();
			this.butInspectStart = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelDisplayJudge = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.hWinDefectImage = new HalconDotNet.HSmartWindowControl();
			this.hWinDefectProcImage = new HalconDotNet.HSmartWindowControl();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.tabImageView = new System.Windows.Forms.TabControl();
			this.tabDefect = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.hWinInspectRight = new HalconDotNet.HSmartWindowControl();
			this.hWinInspectLeft = new HalconDotNet.HSmartWindowControl();
			this.tabCamera0 = new System.Windows.Forms.TabPage();
			this.userCamViewFront = new HFVisionInspect.UserCameraViewFront();
			this.tabCamera1 = new System.Windows.Forms.TabPage();
			this.userCamViewRear = new HFVisionInspect.UserCameraViewRear();
			this.labelMessage = new System.Windows.Forms.Label();
			this.listInspectResult = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timerLightLife = new System.Windows.Forms.Timer(this.components);
			this.timerCutter = new System.Windows.Forms.Timer(this.components);
			this.labelDatetime = new System.Windows.Forms.Label();
			this.timerCheckBright = new System.Windows.Forms.Timer(this.components);
			this.timerDate = new System.Windows.Forms.Timer(this.components);
			this.butNGRelease = new System.Windows.Forms.Button();
			this.butResultReset = new System.Windows.Forms.Button();
			this.menuMainStrip.SuspendLayout();
			this.panelOperation.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panelInspect.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.tabImageView.SuspendLayout();
			this.tabDefect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabCamera0.SuspendLayout();
			this.tabCamera1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuMainStrip
			// 
			this.menuMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.menuInspect,
            this.MenuInfomation});
			this.menuMainStrip.Location = new System.Drawing.Point(0, 0);
			this.menuMainStrip.Name = "menuMainStrip";
			this.menuMainStrip.Size = new System.Drawing.Size(1769, 24);
			this.menuMainStrip.TabIndex = 0;
			this.menuMainStrip.Text = "menuStrip1";
			// 
			// MenuFile
			// 
			this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSystemChangeMode,
            this.toolStripSeparator1,
            this.menuSystemProperty,
            this.menuLightLifeReset,
            this.toolStripSeparator2,
            this.menuSystemExit});
			this.MenuFile.Name = "MenuFile";
			this.MenuFile.Size = new System.Drawing.Size(70, 20);
			this.MenuFile.Text = "システム(&S)";
			// 
			// menuSystemChangeMode
			// 
			this.menuSystemChangeMode.Name = "menuSystemChangeMode";
			this.menuSystemChangeMode.Size = new System.Drawing.Size(195, 22);
			this.menuSystemChangeMode.Text = "管理者モード(&M)";
			this.menuSystemChangeMode.Click += new System.EventHandler(this.manuSystemChangeMode_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
			// 
			// menuSystemProperty
			// 
			this.menuSystemProperty.Name = "menuSystemProperty";
			this.menuSystemProperty.Size = new System.Drawing.Size(195, 22);
			this.menuSystemProperty.Text = "環境設定(&P)";
			this.menuSystemProperty.Click += new System.EventHandler(this.manuSystemProperty_Click);
			// 
			// menuLightLifeReset
			// 
			this.menuLightLifeReset.Name = "menuLightLifeReset";
			this.menuLightLifeReset.Size = new System.Drawing.Size(195, 22);
			this.menuLightLifeReset.Text = "照明点灯時間リセット(&R)";
			this.menuLightLifeReset.Click += new System.EventHandler(this.menuLightLifeReset_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
			// 
			// menuSystemExit
			// 
			this.menuSystemExit.Name = "menuSystemExit";
			this.menuSystemExit.Size = new System.Drawing.Size(195, 22);
			this.menuSystemExit.Text = "終了(&X)";
			// 
			// menuInspect
			// 
			this.menuInspect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInspectRecipeLoad,
            this.menuInspectRecipeSave,
            this.toolStripSeparator3,
            this.menuInspectResultClear});
			this.menuInspect.Name = "menuInspect";
			this.menuInspect.Size = new System.Drawing.Size(54, 20);
			this.menuInspect.Text = "検査(&I)";
			// 
			// menuInspectRecipeLoad
			// 
			this.menuInspectRecipeLoad.Name = "menuInspectRecipeLoad";
			this.menuInspectRecipeLoad.Size = new System.Drawing.Size(164, 22);
			this.menuInspectRecipeLoad.Text = "設定読込(&L)";
			this.menuInspectRecipeLoad.Click += new System.EventHandler(this.menuInspectRecipeLoad_Click);
			// 
			// menuInspectRecipeSave
			// 
			this.menuInspectRecipeSave.Name = "menuInspectRecipeSave";
			this.menuInspectRecipeSave.Size = new System.Drawing.Size(164, 22);
			this.menuInspectRecipeSave.Text = "設定保存(&S)";
			this.menuInspectRecipeSave.Click += new System.EventHandler(this.menuInspectRecipeSave_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
			// 
			// menuInspectResultClear
			// 
			this.menuInspectResultClear.Name = "menuInspectResultClear";
			this.menuInspectResultClear.Size = new System.Drawing.Size(164, 22);
			this.menuInspectResultClear.Text = "結果リストクリア(&C)";
			this.menuInspectResultClear.Click += new System.EventHandler(this.menuInspectResultClear_Click);
			// 
			// MenuInfomation
			// 
			this.MenuInfomation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDioCheck});
			this.MenuInfomation.Name = "MenuInfomation";
			this.MenuInfomation.Size = new System.Drawing.Size(60, 20);
			this.MenuInfomation.Text = "ツール(&T)";
			// 
			// menuDioCheck
			// 
			this.menuDioCheck.Name = "menuDioCheck";
			this.menuDioCheck.Size = new System.Drawing.Size(134, 22);
			this.menuDioCheck.Text = "DIO確認(&D)";
			this.menuDioCheck.Click += new System.EventHandler(this.menuDioCheck_Click);
			// 
			// statusMainStrip
			// 
			this.statusMainStrip.Location = new System.Drawing.Point(0, 1039);
			this.statusMainStrip.Name = "statusMainStrip";
			this.statusMainStrip.Size = new System.Drawing.Size(1769, 22);
			this.statusMainStrip.TabIndex = 1;
			this.statusMainStrip.Text = "statusStrip1";
			// 
			// panelOperation
			// 
			this.panelOperation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelOperation.BackColor = System.Drawing.SystemColors.Control;
			this.panelOperation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelOperation.Controls.Add(this.groupBox5);
			this.panelOperation.Controls.Add(this.labelInspectState);
			this.panelOperation.Controls.Add(this.groupBox4);
			this.panelOperation.Controls.Add(this.groupBox1);
			this.panelOperation.Controls.Add(this.groupBox3);
			this.panelOperation.Controls.Add(this.groupBox2);
			this.panelOperation.Location = new System.Drawing.Point(1502, 27);
			this.panelOperation.Name = "panelOperation";
			this.panelOperation.Size = new System.Drawing.Size(266, 1012);
			this.panelOperation.TabIndex = 2;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.label7);
			this.groupBox5.Controls.Add(this.labelExposureLeft);
			this.groupBox5.Controls.Add(this.labelExposureSetRight);
			this.groupBox5.Controls.Add(this.labelExposureSetLeft);
			this.groupBox5.Controls.Add(this.labelExposureRight);
			this.groupBox5.Controls.Add(this.checkAutoExposure);
			this.groupBox5.Controls.Add(this.lblGrayMeanLeft);
			this.groupBox5.Controls.Add(this.labelBrightRight);
			this.groupBox5.Controls.Add(this.labelBrightLeft);
			this.groupBox5.Controls.Add(this.lblGrayMeanRight);
			this.groupBox5.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox5.Location = new System.Drawing.Point(14, 549);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(239, 277);
			this.groupBox5.TabIndex = 11;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "輝度平均";
			this.groupBox5.Visible = false;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label7.Location = new System.Drawing.Point(125, 177);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(67, 15);
			this.label7.TabIndex = 16;
			this.label7.Text = "露光時間";
			// 
			// labelExposureLeft
			// 
			this.labelExposureLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelExposureLeft.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelExposureLeft.Location = new System.Drawing.Point(116, 200);
			this.labelExposureLeft.Name = "labelExposureLeft";
			this.labelExposureLeft.Size = new System.Drawing.Size(91, 22);
			this.labelExposureLeft.TabIndex = 12;
			this.labelExposureLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelExposureSetRight
			// 
			this.labelExposureSetRight.AutoSize = true;
			this.labelExposureSetRight.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelExposureSetRight.Location = new System.Drawing.Point(42, 234);
			this.labelExposureSetRight.Name = "labelExposureSetRight";
			this.labelExposureSetRight.Size = new System.Drawing.Size(67, 15);
			this.labelExposureSetRight.TabIndex = 15;
			this.labelExposureSetRight.Text = "反操作側";
			// 
			// labelExposureSetLeft
			// 
			this.labelExposureSetLeft.AutoSize = true;
			this.labelExposureSetLeft.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelExposureSetLeft.Location = new System.Drawing.Point(42, 202);
			this.labelExposureSetLeft.Name = "labelExposureSetLeft";
			this.labelExposureSetLeft.Size = new System.Drawing.Size(52, 15);
			this.labelExposureSetLeft.TabIndex = 13;
			this.labelExposureSetLeft.Text = "操作側";
			// 
			// labelExposureRight
			// 
			this.labelExposureRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelExposureRight.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelExposureRight.Location = new System.Drawing.Point(116, 232);
			this.labelExposureRight.Name = "labelExposureRight";
			this.labelExposureRight.Size = new System.Drawing.Size(91, 22);
			this.labelExposureRight.TabIndex = 14;
			this.labelExposureRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// checkAutoExposure
			// 
			this.checkAutoExposure.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkAutoExposure.BackColor = System.Drawing.Color.DarkGreen;
			this.checkAutoExposure.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.checkAutoExposure.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkAutoExposure.Location = new System.Drawing.Point(18, 100);
			this.checkAutoExposure.Name = "checkAutoExposure";
			this.checkAutoExposure.Size = new System.Drawing.Size(205, 59);
			this.checkAutoExposure.TabIndex = 11;
			this.checkAutoExposure.Text = "自動露光 OFF";
			this.checkAutoExposure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkAutoExposure.UseVisualStyleBackColor = false;
			this.checkAutoExposure.CheckedChanged += new System.EventHandler(this.checkAutoExposure_CheckedChanged);
			// 
			// lblGrayMeanLeft
			// 
			this.lblGrayMeanLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblGrayMeanLeft.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblGrayMeanLeft.Location = new System.Drawing.Point(116, 28);
			this.lblGrayMeanLeft.Name = "lblGrayMeanLeft";
			this.lblGrayMeanLeft.Size = new System.Drawing.Size(91, 22);
			this.lblGrayMeanLeft.TabIndex = 7;
			this.lblGrayMeanLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelBrightRight
			// 
			this.labelBrightRight.AutoSize = true;
			this.labelBrightRight.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelBrightRight.Location = new System.Drawing.Point(42, 62);
			this.labelBrightRight.Name = "labelBrightRight";
			this.labelBrightRight.Size = new System.Drawing.Size(67, 15);
			this.labelBrightRight.TabIndex = 10;
			this.labelBrightRight.Text = "反操作側";
			// 
			// labelBrightLeft
			// 
			this.labelBrightLeft.AutoSize = true;
			this.labelBrightLeft.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelBrightLeft.Location = new System.Drawing.Point(42, 30);
			this.labelBrightLeft.Name = "labelBrightLeft";
			this.labelBrightLeft.Size = new System.Drawing.Size(52, 15);
			this.labelBrightLeft.TabIndex = 8;
			this.labelBrightLeft.Text = "操作側";
			// 
			// lblGrayMeanRight
			// 
			this.lblGrayMeanRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblGrayMeanRight.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblGrayMeanRight.Location = new System.Drawing.Point(116, 60);
			this.lblGrayMeanRight.Name = "lblGrayMeanRight";
			this.lblGrayMeanRight.Size = new System.Drawing.Size(91, 22);
			this.lblGrayMeanRight.TabIndex = 9;
			this.lblGrayMeanRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelInspectState
			// 
			this.labelInspectState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInspectState.BackColor = System.Drawing.Color.Lime;
			this.labelInspectState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelInspectState.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelInspectState.Location = new System.Drawing.Point(30, 919);
			this.labelInspectState.Name = "labelInspectState";
			this.labelInspectState.Size = new System.Drawing.Size(206, 68);
			this.labelInspectState.TabIndex = 6;
			this.labelInspectState.Text = "検査中";
			this.labelInspectState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btLoadInspectResult);
			this.groupBox4.Controls.Add(this.butResultSave);
			this.groupBox4.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox4.Location = new System.Drawing.Point(14, 396);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(239, 142);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "検出履歴";
			// 
			// btLoadInspectResult
			// 
			this.btLoadInspectResult.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btLoadInspectResult.Location = new System.Drawing.Point(18, 80);
			this.btLoadInspectResult.Name = "btLoadInspectResult";
			this.btLoadInspectResult.Size = new System.Drawing.Size(205, 45);
			this.btLoadInspectResult.TabIndex = 1;
			this.btLoadInspectResult.Text = "履歴読込";
			this.btLoadInspectResult.UseVisualStyleBackColor = true;
			this.btLoadInspectResult.Click += new System.EventHandler(this.btLoadInspectResult_Click);
			// 
			// butResultSave
			// 
			this.butResultSave.BackColor = System.Drawing.SystemColors.Control;
			this.butResultSave.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.butResultSave.Location = new System.Drawing.Point(18, 28);
			this.butResultSave.Name = "butResultSave";
			this.butResultSave.Size = new System.Drawing.Size(205, 45);
			this.butResultSave.TabIndex = 4;
			this.butResultSave.Text = "履歴保存";
			this.butResultSave.UseVisualStyleBackColor = true;
			this.butResultSave.Click += new System.EventHandler(this.butResultSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btInspectSetRight);
			this.groupBox1.Controls.Add(this.textInspectID);
			this.groupBox1.Controls.Add(this.labelLifeHour);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox1.Location = new System.Drawing.Point(14, 832);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(239, 68);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "検査ID";
			this.groupBox1.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(181, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 15);
			this.label3.TabIndex = 5;
			this.label3.Text = "hour";
			this.label3.Visible = false;
			// 
			// btInspectSetRight
			// 
			this.btInspectSetRight.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btInspectSetRight.Location = new System.Drawing.Point(16, 34);
			this.btInspectSetRight.Name = "btInspectSetRight";
			this.btInspectSetRight.Size = new System.Drawing.Size(205, 45);
			this.btInspectSetRight.TabIndex = 1;
			this.btInspectSetRight.Text = "反操作側  設定";
			this.btInspectSetRight.UseVisualStyleBackColor = true;
			this.btInspectSetRight.Visible = false;
			this.btInspectSetRight.Click += new System.EventHandler(this.btInspectSetRight_Click);
			// 
			// textInspectID
			// 
			this.textInspectID.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textInspectID.Location = new System.Drawing.Point(18, 23);
			this.textInspectID.Name = "textInspectID";
			this.textInspectID.Size = new System.Drawing.Size(209, 26);
			this.textInspectID.TabIndex = 0;
			// 
			// labelLifeHour
			// 
			this.labelLifeHour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelLifeHour.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelLifeHour.Location = new System.Drawing.Point(11, 46);
			this.labelLifeHour.Name = "labelLifeHour";
			this.labelLifeHour.Size = new System.Drawing.Size(164, 23);
			this.labelLifeHour.TabIndex = 4;
			this.labelLifeHour.Text = "0000000";
			this.labelLifeHour.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelLifeHour.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "点灯時間";
			this.label1.Visible = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btInspectSetLeft);
			this.groupBox3.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox3.Location = new System.Drawing.Point(14, 134);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(239, 111);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "検査レベル";
			// 
			// btInspectSetLeft
			// 
			this.btInspectSetLeft.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btInspectSetLeft.Location = new System.Drawing.Point(22, 26);
			this.btInspectSetLeft.Name = "btInspectSetLeft";
			this.btInspectSetLeft.Size = new System.Drawing.Size(205, 54);
			this.btInspectSetLeft.TabIndex = 2;
			this.btInspectSetLeft.Text = "設　定";
			this.btInspectSetLeft.UseVisualStyleBackColor = true;
			this.btInspectSetLeft.Click += new System.EventHandler(this.btInspectSetLeft_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkLightPower);
			this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox2.Location = new System.Drawing.Point(14, 17);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(239, 98);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "照明";
			// 
			// chkLightPower
			// 
			this.chkLightPower.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkLightPower.BackColor = System.Drawing.Color.Yellow;
			this.chkLightPower.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.chkLightPower.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.chkLightPower.Location = new System.Drawing.Point(18, 21);
			this.chkLightPower.Name = "chkLightPower";
			this.chkLightPower.Size = new System.Drawing.Size(209, 59);
			this.chkLightPower.TabIndex = 2;
			this.chkLightPower.Text = "照明 消灯中";
			this.chkLightPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkLightPower.UseVisualStyleBackColor = false;
			this.chkLightPower.CheckedChanged += new System.EventHandler(this.chkLightPower_CheckedChanged);
			// 
			// panelInspect
			// 
			this.panelInspect.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panelInspect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelInspect.Controls.Add(this.butNGRelease);
			this.panelInspect.Controls.Add(this.butResultReset);
			this.panelInspect.Controls.Add(this.butInspectStop);
			this.panelInspect.Controls.Add(this.butInspectStart);
			this.panelInspect.Controls.Add(this.panel1);
			this.panelInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelInspect.Location = new System.Drawing.Point(0, 374);
			this.panelInspect.Name = "panelInspect";
			this.panelInspect.Size = new System.Drawing.Size(990, 195);
			this.panelInspect.TabIndex = 0;
			// 
			// butInspectStop
			// 
			this.butInspectStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butInspectStop.BackColor = System.Drawing.Color.DarkRed;
			this.butInspectStop.Enabled = false;
			this.butInspectStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butInspectStop.Font = new System.Drawing.Font("MS UI Gothic", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.butInspectStop.Location = new System.Drawing.Point(805, 103);
			this.butInspectStop.Name = "butInspectStop";
			this.butInspectStop.Size = new System.Drawing.Size(161, 68);
			this.butInspectStop.TabIndex = 2;
			this.butInspectStop.Text = "ストップ";
			this.butInspectStop.UseVisualStyleBackColor = false;
			this.butInspectStop.Click += new System.EventHandler(this.butInspectStop_Click);
			// 
			// butInspectStart
			// 
			this.butInspectStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butInspectStart.BackColor = System.Drawing.Color.LightGreen;
			this.butInspectStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butInspectStart.Font = new System.Drawing.Font("MS UI Gothic", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.butInspectStart.Location = new System.Drawing.Point(805, 20);
			this.butInspectStart.Name = "butInspectStart";
			this.butInspectStart.Size = new System.Drawing.Size(161, 68);
			this.butInspectStart.TabIndex = 1;
			this.butInspectStart.Text = "スタート";
			this.butInspectStart.UseVisualStyleBackColor = false;
			this.butInspectStart.Click += new System.EventHandler(this.butInspectStart_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.labelDisplayJudge);
			this.panel1.Location = new System.Drawing.Point(20, 18);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(333, 156);
			this.panel1.TabIndex = 0;
			// 
			// labelDisplayJudge
			// 
			this.labelDisplayJudge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDisplayJudge.BackColor = System.Drawing.Color.Red;
			this.labelDisplayJudge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelDisplayJudge.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelDisplayJudge.Location = new System.Drawing.Point(22, 25);
			this.labelDisplayJudge.Name = "labelDisplayJudge";
			this.labelDisplayJudge.Size = new System.Drawing.Size(282, 101);
			this.labelDisplayJudge.TabIndex = 0;
			this.labelDisplayJudge.Text = "N G";
			this.labelDisplayJudge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Location = new System.Drawing.Point(0, 27);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer1.Size = new System.Drawing.Size(1496, 1012);
			this.splitContainer1.SplitterDistance = 496;
			this.splitContainer1.TabIndex = 3;
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.hWinDefectImage);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.hWinDefectProcImage);
			this.splitContainer2.Size = new System.Drawing.Size(492, 1008);
			this.splitContainer2.SplitterDistance = 481;
			this.splitContainer2.TabIndex = 0;
			// 
			// hWinDefectImage
			// 
			this.hWinDefectImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hWinDefectImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hWinDefectImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hWinDefectImage.HDoubleClickToFitContent = true;
			this.hWinDefectImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hWinDefectImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWinDefectImage.HKeepAspectRatio = true;
			this.hWinDefectImage.HMoveContent = true;
			this.hWinDefectImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hWinDefectImage.Location = new System.Drawing.Point(0, 0);
			this.hWinDefectImage.Margin = new System.Windows.Forms.Padding(0);
			this.hWinDefectImage.Name = "hWinDefectImage";
			this.hWinDefectImage.Size = new System.Drawing.Size(490, 479);
			this.hWinDefectImage.TabIndex = 0;
			this.hWinDefectImage.WindowSize = new System.Drawing.Size(490, 479);
			// 
			// hWinDefectProcImage
			// 
			this.hWinDefectProcImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hWinDefectProcImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hWinDefectProcImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hWinDefectProcImage.HDoubleClickToFitContent = true;
			this.hWinDefectProcImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hWinDefectProcImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWinDefectProcImage.HKeepAspectRatio = true;
			this.hWinDefectProcImage.HMoveContent = true;
			this.hWinDefectProcImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hWinDefectProcImage.Location = new System.Drawing.Point(0, 0);
			this.hWinDefectProcImage.Margin = new System.Windows.Forms.Padding(0);
			this.hWinDefectProcImage.Name = "hWinDefectProcImage";
			this.hWinDefectProcImage.Size = new System.Drawing.Size(490, 521);
			this.hWinDefectProcImage.TabIndex = 0;
			this.hWinDefectProcImage.WindowSize = new System.Drawing.Size(490, 521);
			// 
			// splitContainer3
			// 
			this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.tabImageView);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.labelMessage);
			this.splitContainer3.Panel2.Controls.Add(this.listInspectResult);
			this.splitContainer3.Panel2.Controls.Add(this.panelInspect);
			this.splitContainer3.Size = new System.Drawing.Size(992, 1008);
			this.splitContainer3.SplitterDistance = 433;
			this.splitContainer3.TabIndex = 0;
			this.splitContainer3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer3_SplitterMoved);
			// 
			// tabImageView
			// 
			this.tabImageView.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabImageView.Controls.Add(this.tabDefect);
			this.tabImageView.Controls.Add(this.tabCamera0);
			this.tabImageView.Controls.Add(this.tabCamera1);
			this.tabImageView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabImageView.Location = new System.Drawing.Point(0, 0);
			this.tabImageView.Name = "tabImageView";
			this.tabImageView.SelectedIndex = 0;
			this.tabImageView.Size = new System.Drawing.Size(990, 431);
			this.tabImageView.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabImageView.TabIndex = 0;
			this.tabImageView.SelectedIndexChanged += new System.EventHandler(this.tabImageView_SelectedIndexChanged);
			this.tabImageView.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabImageView_Selected);
			// 
			// tabDefect
			// 
			this.tabDefect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tabDefect.Controls.Add(this.tableLayoutPanel1);
			this.tabDefect.Location = new System.Drawing.Point(4, 25);
			this.tabDefect.Name = "tabDefect";
			this.tabDefect.Padding = new System.Windows.Forms.Padding(3);
			this.tabDefect.Size = new System.Drawing.Size(982, 402);
			this.tabDefect.TabIndex = 0;
			this.tabDefect.Text = "検出画像";
			this.tabDefect.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.hWinInspectRight, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.hWinInspectLeft, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(974, 394);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// hWinInspectRight
			// 
			this.hWinInspectRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hWinInspectRight.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hWinInspectRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hWinInspectRight.HDoubleClickToFitContent = true;
			this.hWinInspectRight.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hWinInspectRight.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWinInspectRight.HKeepAspectRatio = true;
			this.hWinInspectRight.HMoveContent = true;
			this.hWinInspectRight.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hWinInspectRight.Location = new System.Drawing.Point(2, 198);
			this.hWinInspectRight.Margin = new System.Windows.Forms.Padding(0);
			this.hWinInspectRight.Name = "hWinInspectRight";
			this.hWinInspectRight.Size = new System.Drawing.Size(970, 194);
			this.hWinInspectRight.TabIndex = 1;
			this.hWinInspectRight.WindowSize = new System.Drawing.Size(970, 194);
			// 
			// hWinInspectLeft
			// 
			this.hWinInspectLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hWinInspectLeft.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.hWinInspectLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hWinInspectLeft.HDoubleClickToFitContent = true;
			this.hWinInspectLeft.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
			this.hWinInspectLeft.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWinInspectLeft.HKeepAspectRatio = true;
			this.hWinInspectLeft.HMoveContent = true;
			this.hWinInspectLeft.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
			this.hWinInspectLeft.Location = new System.Drawing.Point(2, 2);
			this.hWinInspectLeft.Margin = new System.Windows.Forms.Padding(0);
			this.hWinInspectLeft.Name = "hWinInspectLeft";
			this.hWinInspectLeft.Size = new System.Drawing.Size(970, 194);
			this.hWinInspectLeft.TabIndex = 0;
			this.hWinInspectLeft.WindowSize = new System.Drawing.Size(970, 194);
			// 
			// tabCamera0
			// 
			this.tabCamera0.Controls.Add(this.userCamViewFront);
			this.tabCamera0.Location = new System.Drawing.Point(4, 25);
			this.tabCamera0.Name = "tabCamera0";
			this.tabCamera0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tabCamera0.Size = new System.Drawing.Size(982, 402);
			this.tabCamera0.TabIndex = 1;
			this.tabCamera0.Text = "表面カメラ";
			this.tabCamera0.UseVisualStyleBackColor = true;
			// 
			// userCamViewFront
			// 
			this.userCamViewFront.CameraStatus = HFVisionInspect.ProcState.NONE;
			this.userCamViewFront.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userCamViewFront.InspectParam = inspectParameter1;
			this.userCamViewFront.IsInspect = false;
			this.userCamViewFront.Location = new System.Drawing.Point(0, 0);
			this.userCamViewFront.Name = "userCamViewFront";
			this.userCamViewFront.ResultData = null;
			this.userCamViewFront.Size = new System.Drawing.Size(982, 402);
			this.userCamViewFront.TabIndex = 1;
			// 
			// tabCamera1
			// 
			this.tabCamera1.Controls.Add(this.userCamViewRear);
			this.tabCamera1.Location = new System.Drawing.Point(4, 25);
			this.tabCamera1.Name = "tabCamera1";
			this.tabCamera1.Size = new System.Drawing.Size(982, 402);
			this.tabCamera1.TabIndex = 2;
			this.tabCamera1.Text = "裏面カメラ";
			this.tabCamera1.UseVisualStyleBackColor = true;
			// 
			// userCamViewRear
			// 
			this.userCamViewRear.CameraStatus = HFVisionInspect.ProcState.NONE;
			this.userCamViewRear.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userCamViewRear.InspectParam = inspectParameter2;
			this.userCamViewRear.IsInspect = false;
			this.userCamViewRear.Location = new System.Drawing.Point(0, 0);
			this.userCamViewRear.Name = "userCamViewRear";
			this.userCamViewRear.ResultData = null;
			this.userCamViewRear.Size = new System.Drawing.Size(982, 402);
			this.userCamViewRear.TabIndex = 1;
			// 
			// labelMessage
			// 
			this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.labelMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelMessage.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMessage.ForeColor = System.Drawing.Color.Red;
			this.labelMessage.Location = new System.Drawing.Point(45, 228);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(900, 101);
			this.labelMessage.TabIndex = 2;
			this.labelMessage.Text = "N G";
			this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelMessage.Visible = false;
			// 
			// listInspectResult
			// 
			this.listInspectResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listInspectResult.BackColor = System.Drawing.Color.Gainsboro;
			this.listInspectResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
			this.listInspectResult.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listInspectResult.FullRowSelect = true;
			this.listInspectResult.GridLines = true;
			this.listInspectResult.HideSelection = false;
			this.listInspectResult.Location = new System.Drawing.Point(26, 19);
			this.listInspectResult.MultiSelect = false;
			this.listInspectResult.Name = "listInspectResult";
			this.listInspectResult.Size = new System.Drawing.Size(941, 329);
			this.listInspectResult.TabIndex = 1;
			this.listInspectResult.UseCompatibleStateImageBehavior = false;
			this.listInspectResult.View = System.Windows.Forms.View.Details;
			this.listInspectResult.SelectedIndexChanged += new System.EventHandler(this.listInspectResult_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "No";
			this.columnHeader1.Width = 50;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "日付";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "時刻";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 100;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "面積 [ｍ㎡]";
			this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader4.Width = 100;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "幅 [mm]";
			this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader5.Width = 100;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "長さ [mm]";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader6.Width = 100;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "位置 (左から)";
			this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader7.Width = 140;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "判定";
			this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader8.Width = 100;
			// 
			// timerLightLife
			// 
			this.timerLightLife.Interval = 60000;
			this.timerLightLife.Tick += new System.EventHandler(this.timerLightLife_Tick);
			// 
			// timerCutter
			// 
			this.timerCutter.Interval = 1000;
			this.timerCutter.Tick += new System.EventHandler(this.timerCutter_Tick);
			// 
			// labelDatetime
			// 
			this.labelDatetime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDatetime.Location = new System.Drawing.Point(1568, 1042);
			this.labelDatetime.Name = "labelDatetime";
			this.labelDatetime.Size = new System.Drawing.Size(172, 19);
			this.labelDatetime.TabIndex = 7;
			this.labelDatetime.Text = "( 00:00:00 )";
			this.labelDatetime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// timerCheckBright
			// 
			this.timerCheckBright.Enabled = true;
			this.timerCheckBright.Interval = 1000;
			this.timerCheckBright.Tick += new System.EventHandler(this.timerCheckBright_Tick);
			// 
			// timerDate
			// 
			this.timerDate.Enabled = true;
			this.timerDate.Interval = 60000;
			this.timerDate.Tick += new System.EventHandler(this.timerDate_Tick);
			// 
			// butNGRelease
			// 
			this.butNGRelease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butNGRelease.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.butNGRelease.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butNGRelease.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.butNGRelease.Location = new System.Drawing.Point(617, 106);
			this.butNGRelease.Name = "butNGRelease";
			this.butNGRelease.Size = new System.Drawing.Size(161, 68);
			this.butNGRelease.TabIndex = 7;
			this.butNGRelease.Text = "NG解除";
			this.butNGRelease.UseVisualStyleBackColor = false;
			this.butNGRelease.Click += new System.EventHandler(this.butNGRelease_Click);
			// 
			// butResultReset
			// 
			this.butResultReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butResultReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.butResultReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butResultReset.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.butResultReset.Location = new System.Drawing.Point(617, 19);
			this.butResultReset.Name = "butResultReset";
			this.butResultReset.Size = new System.Drawing.Size(161, 68);
			this.butResultReset.TabIndex = 6;
			this.butResultReset.Text = "リセット";
			this.butResultReset.UseVisualStyleBackColor = false;
			this.butResultReset.Click += new System.EventHandler(this.butResultReset_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1769, 1061);
			this.Controls.Add(this.labelDatetime);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.panelOperation);
			this.Controls.Add(this.statusMainStrip);
			this.Controls.Add(this.menuMainStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuMainStrip;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "中空糸外観検査装置";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.Shown += new System.EventHandler(this.FormMain_Shown);
			this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
			this.menuMainStrip.ResumeLayout(false);
			this.menuMainStrip.PerformLayout();
			this.panelOperation.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panelInspect.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.tabImageView.ResumeLayout(false);
			this.tabDefect.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tabCamera0.ResumeLayout(false);
			this.tabCamera1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMainStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuInfomation;
        private System.Windows.Forms.StatusStrip statusMainStrip;
        private System.Windows.Forms.Panel panelOperation;
        private System.Windows.Forms.Panel panelInspect;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private HalconDotNet.HSmartWindowControl hWinDefectImage;
        private HalconDotNet.HSmartWindowControl hWinDefectProcImage;
        private System.Windows.Forms.ListView listInspectResult;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ToolStripMenuItem menuSystemProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSystemExit;
        private System.Windows.Forms.ToolStripMenuItem menuDioCheck;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLifeHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLightPower;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textInspectID;
        private System.Windows.Forms.Button butResultSave;
        private System.Windows.Forms.Button butInspectStop;
        private System.Windows.Forms.Button butInspectStart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelDisplayJudge;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btLoadInspectResult;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btInspectSetRight;
        private System.Windows.Forms.TabControl tabImageView;
        private System.Windows.Forms.TabPage tabDefect;
        private HalconDotNet.HSmartWindowControl hWinInspectLeft;
        private System.Windows.Forms.Timer timerLightLife;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Label labelInspectState;
        private System.Windows.Forms.ToolStripMenuItem menuSystemChangeMode;
        private System.Windows.Forms.ToolStripMenuItem menuLightLifeReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuInspect;
        private System.Windows.Forms.ToolStripMenuItem menuInspectRecipeLoad;
        private System.Windows.Forms.ToolStripMenuItem menuInspectRecipeSave;
        private System.Windows.Forms.Timer timerCutter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuInspectResultClear;
        private System.Windows.Forms.Label labelDatetime;
        private System.Windows.Forms.Timer timerCheckBright;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblGrayMeanLeft;
        private System.Windows.Forms.Label labelBrightRight;
        private System.Windows.Forms.Label labelBrightLeft;
        private System.Windows.Forms.Label lblGrayMeanRight;
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Button btInspectSetLeft;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private HalconDotNet.HSmartWindowControl hWinInspectRight;
		private System.Windows.Forms.TabPage tabCamera0;
		private System.Windows.Forms.CheckBox checkAutoExposure;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label labelExposureLeft;
		private System.Windows.Forms.Label labelExposureSetRight;
		private System.Windows.Forms.Label labelExposureSetLeft;
		private System.Windows.Forms.Label labelExposureRight;
		private System.Windows.Forms.Timer timerDate;
		private System.Windows.Forms.TabPage tabCamera1;
		private UserCameraViewFront userCamViewFront;
		private UserCameraViewRear userCamViewRear;
		private System.Windows.Forms.Button butNGRelease;
		private System.Windows.Forms.Button butResultReset;
	}
}

