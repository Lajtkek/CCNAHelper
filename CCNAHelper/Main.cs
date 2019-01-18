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
    public static class Clipboard
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);
        [DllImport("user32.dll")]
        

        static extern bool IsClipboardFormatAvailable(uint format);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);

        const uint CF_UNICODETEXT = 13;

        public static string GetText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
                return null;
            if (!OpenClipboard(IntPtr.Zero))
                return null;
            
            string data = null;
            var hGlobal = GetClipboardData(CF_UNICODETEXT);
            if (hGlobal != IntPtr.Zero)
            {
                var lpwcstr = GlobalLock(hGlobal);
                if (lpwcstr != IntPtr.Zero)
                {
                    data = Marshal.PtrToStringUni(lpwcstr);
                    GlobalUnlock(lpwcstr);
                }
            }
 
            CloseClipboard();
        
            return data;
        }

    }
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
            //BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            Rectangle resolution = new Rectangle(0,0,1920,1080);

            checker = new Thread(new ThreadStart(CheckForQuestion));
            checker.Start();

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

        void CheckForQuestion()
        {
            MessageBox.Show(Clipboard.GetText());
            running = true;
            while (running) {
                if (Clipboard.GetText() != null)
                {
                    string text = Clipboard.GetText();
                    
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
                //FindAnswer();
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

        void FindAnswer(string question)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { FindAnswer(question); }));                
            }
            else
            {
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
                if (Settings.Instance.Prefs.onlineMode) FindOnlineAnswer(question);
                label1.Refresh();
            }
        }

        void FindOnlineAnswer(string question)
        {

            string html = string.Empty;
            string url = @"http://localhost/api/Answer.php?question="+question+"&key="+Settings.Instance.Prefs.apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string[] answers = new string[0];

            try{
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

                foreach (string a in answers)
                {
                    label1.Text += a + "\n";
                }
            }catch(Exception e)
            {
                MessageBox.Show("Nelje ze připojit k serveru");
            }
        }
    }
}
