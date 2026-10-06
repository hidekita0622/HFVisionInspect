
namespace HFVisionInspect
{
    partial class UserHistoryView
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelHistoryNo = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.splitContainerImage = new System.Windows.Forms.SplitContainer();
            this.hWinGrabImage = new HalconDotNet.HSmartWindowControl();
            this.labelGrabImage = new System.Windows.Forms.Label();
            this.hWinProcImage = new HalconDotNet.HSmartWindowControl();
            this.labelProcImage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImage)).BeginInit();
            this.splitContainerImage.Panel1.SuspendLayout();
            this.splitContainerImage.Panel2.SuspendLayout();
            this.splitContainerImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelHistoryNo);
            this.panel1.Controls.Add(this.labelTime);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 152);
            this.panel1.TabIndex = 0;
            // 
            // labelHistoryNo
            // 
            this.labelHistoryNo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelHistoryNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHistoryNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHistoryNo.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelHistoryNo.Location = new System.Drawing.Point(0, 76);
            this.labelHistoryNo.Name = "labelHistoryNo";
            this.labelHistoryNo.Size = new System.Drawing.Size(250, 76);
            this.labelHistoryNo.TabIndex = 1;
            this.labelHistoryNo.Text = "履　歴　N　o";
            this.labelHistoryNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTime
            // 
            this.labelTime.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.labelTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTime.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelTime.Location = new System.Drawing.Point(0, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(250, 76);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "時  刻";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainerImage
            // 
            this.splitContainerImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImage.IsSplitterFixed = true;
            this.splitContainerImage.Location = new System.Drawing.Point(0, 152);
            this.splitContainerImage.Name = "splitContainerImage";
            this.splitContainerImage.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerImage.Panel1
            // 
            this.splitContainerImage.Panel1.Controls.Add(this.hWinGrabImage);
            this.splitContainerImage.Panel1.Controls.Add(this.labelGrabImage);
            // 
            // splitContainerImage.Panel2
            // 
            this.splitContainerImage.Panel2.Controls.Add(this.hWinProcImage);
            this.splitContainerImage.Panel2.Controls.Add(this.labelProcImage);
            this.splitContainerImage.Size = new System.Drawing.Size(250, 551);
            this.splitContainerImage.SplitterDistance = 251;
            this.splitContainerImage.TabIndex = 1;
            this.splitContainerImage.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerImage_SplitterMoved);
            // 
            // hWinGrabImage
            // 
            this.hWinGrabImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWinGrabImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWinGrabImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWinGrabImage.HDoubleClickToFitContent = true;
            this.hWinGrabImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWinGrabImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWinGrabImage.HKeepAspectRatio = true;
            this.hWinGrabImage.HMoveContent = true;
            this.hWinGrabImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.hWinGrabImage.Location = new System.Drawing.Point(0, 0);
            this.hWinGrabImage.Margin = new System.Windows.Forms.Padding(0);
            this.hWinGrabImage.Name = "hWinGrabImage";
            this.hWinGrabImage.Size = new System.Drawing.Size(246, 247);
            this.hWinGrabImage.TabIndex = 2;
            this.hWinGrabImage.WindowSize = new System.Drawing.Size(246, 247);
            // 
            // labelGrabImage
            // 
            this.labelGrabImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelGrabImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelGrabImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGrabImage.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelGrabImage.Location = new System.Drawing.Point(0, 0);
            this.labelGrabImage.Name = "labelGrabImage";
            this.labelGrabImage.Size = new System.Drawing.Size(246, 247);
            this.labelGrabImage.TabIndex = 1;
            this.labelGrabImage.Text = "画  像";
            this.labelGrabImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hWinProcImage
            // 
            this.hWinProcImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWinProcImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWinProcImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWinProcImage.HDoubleClickToFitContent = true;
            this.hWinProcImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWinProcImage.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWinProcImage.HKeepAspectRatio = true;
            this.hWinProcImage.HMoveContent = true;
            this.hWinProcImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.hWinProcImage.Location = new System.Drawing.Point(0, 0);
            this.hWinProcImage.Margin = new System.Windows.Forms.Padding(0);
            this.hWinProcImage.Name = "hWinProcImage";
            this.hWinProcImage.Size = new System.Drawing.Size(246, 292);
            this.hWinProcImage.TabIndex = 3;
            this.hWinProcImage.WindowSize = new System.Drawing.Size(246, 292);
            // 
            // labelProcImage
            // 
            this.labelProcImage.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelProcImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelProcImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcImage.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelProcImage.Location = new System.Drawing.Point(0, 0);
            this.labelProcImage.Name = "labelProcImage";
            this.labelProcImage.Size = new System.Drawing.Size(246, 292);
            this.labelProcImage.TabIndex = 2;
            this.labelProcImage.Text = "強 調 画 像";
            this.labelProcImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserHistoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerImage);
            this.Controls.Add(this.panel1);
            this.Name = "UserHistoryView";
            this.Size = new System.Drawing.Size(250, 703);
            this.Load += new System.EventHandler(this.UserHistoryView_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainerImage.Panel1.ResumeLayout(false);
            this.splitContainerImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImage)).EndInit();
            this.splitContainerImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelHistoryNo;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.SplitContainer splitContainerImage;
        private HalconDotNet.HSmartWindowControl hWinGrabImage;
        private System.Windows.Forms.Label labelGrabImage;
        private HalconDotNet.HSmartWindowControl hWinProcImage;
        private System.Windows.Forms.Label labelProcImage;
    }
}
