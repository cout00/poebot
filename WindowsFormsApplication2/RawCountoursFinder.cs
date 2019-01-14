using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public class RawCountoursFinder
    {
        Timer timer = new Timer();
        public RawCountoursFinder()
        {
            timer.Interval = 1000;
            timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            
        }
    }
}
