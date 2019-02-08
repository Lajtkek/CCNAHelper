using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace CCNAHelper
{
    public partial class Main : Form
    {
        private bool isContolDown;
        private bool running;
        private Thread checker;

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

            Rectangle resolution = new Rectangle(0, 0, 1920, 1080);

            checker = new Thread(new ThreadStart(CheckForQuestion));
            checker.SetApartmentState(ApartmentState.STA);
            checker.Start();

            Location = new Point((int)(resolution.Width * Settings.Instance.Prefs.anchorA.X), (int)(resolution.Height * Settings.Instance.Prefs.anchorA.Y));
            Size = new Size((int)(resolution.Width * Settings.Instance.Prefs.anchorB.X), (int)(resolution.Height * Settings.Instance.Prefs.anchorB.Y));

            Clipboard.Clear();
        }

        void InitializeHooks()
        {
            globalKeyboardHook gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Settings.Instance.Prefs.toggleShowKey);
            gkh.HookedKeys.Add(Settings.Instance.Prefs.showKey);
            gkh.HookedKeys.Add(Settings.Instance.Prefs.hideKey);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
        }

        [STAThread]
        void CheckForQuestion()
        {
            running = true;
            while (running)
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    Clipboard.Clear();
                    FindAnswer(text);
                }
                Application.DoEvents();
                Thread.Sleep(50);
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                running = false;
                Application.Exit();
            }
            if (Settings.Instance.Prefs.showMode)
            {
                if (e.KeyCode == Settings.Instance.Prefs.toggleShowKey)
                {
                    Show();
                }
            }
            else
            {
                if (e.KeyCode == Settings.Instance.Prefs.showKey)
                {
                    Show();
                }
                if (e.KeyCode == Settings.Instance.Prefs.hideKey)
                {
                    Hide();
                }
            }
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (Settings.Instance.Prefs.showMode)
            {
                if (e.KeyCode == Settings.Instance.Prefs.toggleShowKey)
                {
                    Hide();
                }
            }
        }

        void FindAnswer(string question)
        {
            SetLabelText(label1,"");
            foreach (Question q in Settings.Instance.Questions)
            {
                if (q.Body.Contains(question))
                {
                    foreach (string s in q.Answers)
                    {
                        AddLabelText(label1, s + "\n"); 
                    }
                }
            }
            if (GetText(label1) == "")
            {
                SetLabelText(label1,"Coudnt find answer offline\n");
            }

            if (Settings.Instance.Prefs.onlineMode)
            {
                SetLabelText(label2, "Serching for answer on server ...");
                Thread t = new Thread(new ThreadStart(() => FindOnlineAnswer(question)));
                t.Start();
            };


        }

        void FindOnlineAnswer(string question)
        {
            string html = string.Empty;
            string url = @"http://localhost/api/Answer.php?question=" + question + "&key=" + Settings.Instance.Prefs.apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string[] answers = new string[0];

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    if (!html.Contains("Error"))
                    {
                        answers = JsonConvert.DeserializeObject<string[]>(html);
                    }
                    else
                    {
                        SetLabelText(label2, html);
                        return;
                    }

                }
                SetLabelText(label2, "");
                foreach (string a in answers)
                {
                    AddLabelText(label2, a + "\n");
                }
            }
            catch (Exception e)
            {
                SetLabelText(label2, "Cant connect to Server (" + e.Message + ")");
            }
        }

        void SetLabelText(Label l, string s)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { SetLabelText(l, s); }));
            }
            else
            {
                l.Text = s;
            }
        }

        void AddLabelText(Label l, string s)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { AddLabelText(l, s); }));
            }
            else
            {
                l.Text += s;
            }
        }

        string GetText(Label l)
        {
           return l.Text;
        }
    }
}
