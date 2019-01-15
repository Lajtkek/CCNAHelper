﻿using System;
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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            
            Settings.Instance.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            Hide();
            main.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Settings.Instance.ShowDialog();
            Show();
        }
    }
}