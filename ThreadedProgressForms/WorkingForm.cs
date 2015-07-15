using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadedProgressForms
{
    public partial class WorkingForm : Form
    {
        public WorkingForm()
        {
            InitializeComponent();
        }

        public new void Dispose()
        {
            this.EndProgressView();
            base.Dispose();
        }

        private Thread progressViewerThread;
        private ProgressForm progressViewerWindow;
        public void ShowProgressView()
        {
            progressViewerThread = new Thread(new ThreadStart(() => 
                    {
                        progressViewerWindow = new ProgressForm();
                        System.Windows.Forms.Application.Run(progressViewerWindow);
                    }
            ));
            progressViewerThread.SetApartmentState(ApartmentState.STA);
            progressViewerThread.Start();

        }

        public void EndProgressView()
        {
            if (this.progressViewerThread.IsAlive) this.progressViewerThread.Abort();
        }

        private void SendProgress(int p)
        {
            if (progressViewerWindow != null) progressViewerWindow.Progess.Invoke(p);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowProgressView();

            int i = 0;
            while (i < 100)
            {
                i++;
                SendProgress(i);
                Thread.Sleep(100);
            }

            EndProgressView();
        }
    }
}
