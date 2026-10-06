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
    public partial class FormInspectHistory : Form
    {
        const int MAV_VIEW_NO = 5;
        private UserHistoryView[] _historyView = new UserHistoryView[MAV_VIEW_NO];
        public Point ShowPos;
        public InspectResultData InspectResult { get; set; } = new InspectResultData();


        public FormInspectHistory()
        {
            InitializeComponent();

            userHistoryViewTitle.ViewNo = -1;
            for (int i = 0; i < MAV_VIEW_NO; i++)
            {
                _historyView[i] = new UserHistoryView();

            }
            _historyView[0] = userHistoryView0;
            _historyView[1] = userHistoryView1;
            _historyView[2] = userHistoryView2;
            _historyView[3] = userHistoryView3;
            _historyView[4] = userHistoryView4;
            for (int i = 0; i < MAV_VIEW_NO; i++)
            {
                _historyView[i].ViewNo = i;
            }

            userHistoryViewTitle.OnSplitMoved += SplitMove;
            //ShowPos = new Point(0, 0);
        }

        private void FormInspectHistory_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(ShowPos.X, ShowPos.Y);
            FormResize();
        }

        private void FormInspectHistory_Resize(object sender, EventArgs e)
        {
            FormResize();
        }

        private void FormResize()
        {
            int winW = splitContainerTitle.Panel2.Width;
            int viewW = winW / MAV_VIEW_NO;

            splitContainerView0.SplitterDistance = viewW;
            splitContainerView1.SplitterDistance = viewW;
            splitContainerView2.SplitterDistance = viewW;
            splitContainerView3.SplitterDistance = viewW;

            int h = userHistoryViewTitle.GetSplitHeight();
            SplitMove(h);
        }

        private void SplitMove(int height)
        {
            for (int i = 0; i < MAV_VIEW_NO; i++)
            {
                _historyView[i].SetSplitHeight(height);
            }
        }

        private void FormInspectHistory_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            //WindowState = FormWindowState.Minimized;
        }

        public void FormMaximized()
        {
            this.Location = new Point(ShowPos.X, ShowPos.Y);
            //WindowState = FormWindowState.Maximized;
            Show();
        }

        public void UpdateInspectResult()
        {
            int dataCount = InspectResult.GetCount();
            int maxDataNo = dataCount - 1;
            for (int i = maxDataNo, viewno = MAV_VIEW_NO - 1; i >= 0 && viewno >= 0; i--)
            {
                ResultData result = InspectResult.GetData(i);
                if (result.No < 0)
                    continue;
                _historyView[viewno].SetResultData(result);
                _historyView[viewno].UpdateDataView();
                viewno--;
            }
        }
    }
}
