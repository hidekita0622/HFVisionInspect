
namespace HFVisionInspect
{
    partial class UserSetDefectLevel
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
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.numMinArea = new System.Windows.Forms.NumericUpDown();
            this.numMaxArea = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.numMaxGray = new System.Windows.Forms.NumericUpDown();
            this.numMinGray = new System.Windows.Forms.NumericUpDown();
            this.checkJudgeEnable = new System.Windows.Forms.CheckBox();
            this.checkJudgeNG = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelMinAreaMM = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMaxAreaMM = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numMinArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxGray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinGray)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 107;
            this.label1.Text = "検出輝度";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 15);
            this.label10.TabIndex = 103;
            this.label10.Text = "検出面積 [Pixel]";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(211, 89);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(13, 14);
            this.label23.TabIndex = 109;
            this.label23.Text = "～";
            // 
            // numMinArea
            // 
            this.numMinArea.Location = new System.Drawing.Point(140, 37);
            this.numMinArea.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numMinArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinArea.Name = "numMinArea";
            this.numMinArea.Size = new System.Drawing.Size(65, 19);
            this.numMinArea.TabIndex = 104;
            this.numMinArea.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMinArea.ValueChanged += new System.EventHandler(this.numMinArea_ValueChanged);
            // 
            // numMaxArea
            // 
            this.numMaxArea.Location = new System.Drawing.Point(230, 37);
            this.numMaxArea.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numMaxArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxArea.Name = "numMaxArea";
            this.numMaxArea.Size = new System.Drawing.Size(65, 19);
            this.numMaxArea.TabIndex = 105;
            this.numMaxArea.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMaxArea.ValueChanged += new System.EventHandler(this.numMaxArea_ValueChanged);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(211, 41);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(13, 14);
            this.label18.TabIndex = 106;
            this.label18.Text = "～";
            // 
            // numMaxGray
            // 
            this.numMaxGray.Location = new System.Drawing.Point(230, 85);
            this.numMaxGray.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMaxGray.Name = "numMaxGray";
            this.numMaxGray.Size = new System.Drawing.Size(65, 19);
            this.numMaxGray.TabIndex = 110;
            this.numMaxGray.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numMinGray
            // 
            this.numMinGray.Location = new System.Drawing.Point(140, 85);
            this.numMinGray.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMinGray.Name = "numMinGray";
            this.numMinGray.Size = new System.Drawing.Size(65, 19);
            this.numMinGray.TabIndex = 108;
            this.numMinGray.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkJudgeEnable
            // 
            this.checkJudgeEnable.AutoSize = true;
            this.checkJudgeEnable.Location = new System.Drawing.Point(15, 13);
            this.checkJudgeEnable.Name = "checkJudgeEnable";
            this.checkJudgeEnable.Size = new System.Drawing.Size(72, 16);
            this.checkJudgeEnable.TabIndex = 111;
            this.checkJudgeEnable.Text = "条件有効";
            this.checkJudgeEnable.UseVisualStyleBackColor = true;
            // 
            // checkJudgeNG
            // 
            this.checkJudgeNG.AutoSize = true;
            this.checkJudgeNG.Location = new System.Drawing.Point(34, 110);
            this.checkJudgeNG.Name = "checkJudgeNG";
            this.checkJudgeNG.Size = new System.Drawing.Size(64, 16);
            this.checkJudgeNG.TabIndex = 112;
            this.checkJudgeNG.Text = "NG判定";
            this.checkJudgeNG.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 113;
            this.label2.Text = " [m㎡]";
            // 
            // labelMinAreaMM
            // 
            this.labelMinAreaMM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMinAreaMM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMinAreaMM.Location = new System.Drawing.Point(140, 61);
            this.labelMinAreaMM.Name = "labelMinAreaMM";
            this.labelMinAreaMM.Size = new System.Drawing.Size(65, 19);
            this.labelMinAreaMM.TabIndex = 114;
            this.labelMinAreaMM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(210, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 14);
            this.label3.TabIndex = 115;
            this.label3.Text = "～";
            // 
            // labelMaxAreaMM
            // 
            this.labelMaxAreaMM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMaxAreaMM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMaxAreaMM.Location = new System.Drawing.Point(229, 61);
            this.labelMaxAreaMM.Name = "labelMaxAreaMM";
            this.labelMaxAreaMM.Size = new System.Drawing.Size(65, 19);
            this.labelMaxAreaMM.TabIndex = 116;
            this.labelMaxAreaMM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserSetDefectLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelMaxAreaMM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelMinAreaMM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkJudgeNG);
            this.Controls.Add(this.checkJudgeEnable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.numMinArea);
            this.Controls.Add(this.numMaxArea);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.numMaxGray);
            this.Controls.Add(this.numMinGray);
            this.Name = "UserSetDefectLevel";
            this.Size = new System.Drawing.Size(347, 133);
            ((System.ComponentModel.ISupportInitialize)(this.numMinArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxGray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinGray)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown numMinArea;
        private System.Windows.Forms.NumericUpDown numMaxArea;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown numMaxGray;
        private System.Windows.Forms.NumericUpDown numMinGray;
        private System.Windows.Forms.CheckBox checkJudgeEnable;
        private System.Windows.Forms.CheckBox checkJudgeNG;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelMinAreaMM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMaxAreaMM;
    }
}
