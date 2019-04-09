using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCNAHelper
{
    class LightCapKeyHook
    {
        private List<Keys>keys;
        private List<bool> isPressed;
        public event Action<Keys> Events;

        public Thread main;

        public LightCapKeyHook()
        {
            main = new Thread(new ThreadStart(Main));
            main.Start();
        }

        public void Main()
        {
            while (true)
            {

            }
        }
    }
}
