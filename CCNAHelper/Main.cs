﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace CCNAHelper
{
    public partial class Main : Form
    {
        public bool isContolDown;

        public Main()
        {
            InitializeComponent();
            Initialize();
            InitializeHooks();           
        }

        void Initialize()
        {
            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
        }

        void InitializeHooks()
        {
            globalKeyboardHook gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.LControlKey);
            gkh.HookedKeys.Add(Keys.C);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.Up);
            gkh.HookedKeys.Add(Keys.Down);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
            if (e.KeyCode == Keys.Up)
            {
                Show();
            }
            if (e.KeyCode == Keys.Down)
            {
                Hide();
            }
            if (e.KeyCode == Keys.C && isContolDown)
            {
                FindAnswer();
            }
            if (e.KeyCode == Keys.LControlKey)
            {
                isContolDown = true;
            }
            else
            {
                isContolDown = false;
            }
        }

        void FindAnswer()
        {
            string question = "";

            foreach(Question q in Settings.Instance.Questions)
            {
                if (q.Body.Contains(question))
                {
                    label1.Text = "";
                    foreach(string s in q.Answers)
                    {
                        label1.Text += s + "\n";
                    }
                }
            }
        }
    }
}