using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities;
using System.Threading;
using System.Windows.Forms;

namespace CCNAHelper
{
    public partial class Helper : Form
    {
        private List<Question> questions;
        private globalKeyboardHook gkh;
        private bool isCtrl;
        private Thread tr;

        public Helper(List<Question> questions)
        {
            InitializeComponent();
            this.questions = questions;
            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            TopMost = true;
            Width = Screen.PrimaryScreen.WorkingArea.Width / 3;
            gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.LControlKey);
            gkh.HookedKeys.Add(Keys.C);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.Up);
            gkh.HookedKeys.Add(Keys.Down);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            this.ShowInTaskbar = false;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
            if (e.KeyCode == Keys.Up)
            {
                label1.Show();
            }
            if (e.KeyCode == Keys.Down)
            {
                label1.Hide();
            }
            if (e.KeyCode == Keys.C && isCtrl)
            {
                timer1.Enabled = true;
            }
            if (e.KeyCode == Keys.LControlKey)
            {
                isCtrl = true;
            }
            else
            {
                isCtrl = false;
            }
        }

        private void Helper_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Size.Width, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

                getOdpoved(Clipboard.GetText());
            
            timer1.Enabled = false;
        }

        private void getOdpoved(string otazka)
        {
            label1.Text = "";
            if (otazka.Length >= 10) {
                foreach (Question a in questions)
                {
                    if (a.Body.Contains(otazka))
                    {
                        foreach (string b in a.Answers)
                        {
                            label1.Text += b + "\n";
                        }
                    }
                }
                if (label1.Text == "")
                {
                    label1.Text = "Otázka nenalezena.";
                }
            }
            else
            {
                label1.Text = "Označená otázka musí mít alespoň 10 znaků.";
            }
        }
    }
}
