using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadedProgressForms
{
    public partial class ProgressForm : Form
    {
        public delegate void ProgressHandler(int i);

        public ProgressHandler Progess;

        public ProgressForm()
        {
            InitializeComponent();
            Progess += handleProgress;
        }

        private void handleProgress(int i)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                ProgressHandler call = new ProgressHandler(handleProgress);
                this.BeginInvoke(call, new object[] { i });
            }
            else
            {
                if (0 <= i && i <= 100)
                {
                    progressBar1.Value = i;
                    progressBar1.Invalidate();
                }
                else this.Close();
                 
            }
        }
    }
}
