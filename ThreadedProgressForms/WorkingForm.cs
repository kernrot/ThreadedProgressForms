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

        private void button1_Click(object sender, EventArgs e)
        {
            ShowProgressView();

            int i = 0;
            while (i < 100)
            {
                i++;
                if (progressViewerWindow != null) progressViewerWindow.Progess.Invoke(i);
                Thread.Sleep(100);
            }
            button2.Text += " Done!";

            EndProgressView();
        }

        private Thread workerThread;

        private void button2_Click(object sender, EventArgs e)
        {
            Work worker = new Work();
            worker.ProgessChanged += handleProgess;

            workerThread = new Thread(new ThreadStart(() =>
                    {
                        worker.Run();
                    }));

            workerThread.Start();
        }

        public void handleProgess(int p)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                ProgressHandler call = new ProgressHandler(handleProgess);
                this.BeginInvoke(call, new object[] { p });
            }
            else
            {
                if (0 <= p && p <= 100)
                {
                    this.progressBar1.Value = p;
                }
                else
                    button2.Text += " Done!";
            }
        }
    }
}
