using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadedProgressForms
{
    public class Work
    {
        public event ProgressHandler ProgessChanged = (i) => { };

        public void Run()
        {
            int i = 0;
            while (i < 100)
            {
                i++;
                ProgessChanged.Invoke(i);
                Thread.Sleep(100);
            }
        }
    }
}
