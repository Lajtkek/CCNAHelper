using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCNAHelper
{
    public partial class KeyDialog : Form
    {
        public Keys pressedKey;
        public KeyDialog()
        {
            InitializeComponent();
        }

        private void KeyDialog_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKey = e.KeyCode;
            DialogResult = DialogResult.OK;
        }
    }
}
