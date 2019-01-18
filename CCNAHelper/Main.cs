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
            //BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            Rectangle resolution = new Rectangle(0,0,1920,1080);

            //MessageBox.Show(resolution.ToString());
            //MessageBox.Show(Settings.Instance.Prefs.anchorA.ToString());
            Location = new Point((int)(resolution.Width * Settings.Instance.Prefs.anchorA.X), (int)(resolution.Height * Settings.Instance.Prefs.anchorA.Y));
            Size = new Size((int)(resolution.Width * Settings.Instance.Prefs.anchorB.X),(int)(resolution.Height * Settings.Instance.Prefs.anchorB.Y));

           
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
            //TODO: Upgrade

            Thread.Sleep(500);

            string question = Clipboard.GetText();

            label1.Text = "Coudnt find answer offline\n1";

            foreach (Question q in Settings.Instance.Questions)
            {
                if (q.Body.Contains(question))
                {
                    label1.Text = "";
                    foreach (string s in q.Answers)
                    {
                        label1.Text += s + "\n";
                    }
                }
            }
            if(Settings.Instance.Prefs.onlineMode)FindOnlineAnswer();
            label1.Refresh();
        }

        void FindOnlineAnswer()
        {
            string question = Clipboard.GetText();
            string html = string.Empty;
            string url = @"http://localhost/api/Answer.php?question="+question+"&key="+Settings.Instance.Prefs.apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string[] answers = new string[0];

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
                    label1.Text += html;
                }
                
            }

            foreach(string a in answers)
            {
                label1.Text += a + "\n";
            }
        }
    }
}
