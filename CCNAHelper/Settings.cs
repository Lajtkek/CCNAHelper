using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCNAHelper
{
    public partial class Settings : Form
    {
        private SerializedSettings prefs = new SerializedSettings();
        public SerializedSettings Prefs { get { return prefs; } }

        private List<Question> questions = new List<Question>();
        public Question[] Questions { get { return questions.ToArray(); } }

        public static Settings Instance = new Settings();

        private Settings()
        {
            InitializeComponent();
            LoadOfflinePackages();
            Initialize();
        }

        void Initialize()
        {
            prefs.onlineMode = false;
            prefs.apiKey = "LightCap1337Eva32";
            prefs.anchorA = new PointF(0, 0f);
            prefs.anchorB = new PointF(0.5F, 0.1f);
            prefs.showMode = false;

            prefs.showKey = Keys.Up;
            prefs.hideKey = Keys.Down;
            prefs.toggleShowKey = Keys.Up;

            RefreshBindings();
            ToggleButtons();
        }

        void LoadOfflinePackages()
        {
            questions = new List<Question>();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"answers";

            bool exists = Directory.Exists(filePath);

            if (!exists) Directory.CreateDirectory(filePath);

            label2.Text = "";
            foreach (var file in Directory.GetFiles(filePath, "*.json"))
            {
                string fileName = Path.GetFileName(file);
                if (fileName != "prefs.json")
                {
                    try
                    {
                        string content = File.ReadAllText(file);
                        List<Question> a = JsonConvert.DeserializeObject<List<Question>>(content);
                        foreach (Question q in a)
                        {
                            questions.Add(q);
                        }
                        label2.Text += fileName + "\n";
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Soubor " + fileName + " není validní json soubor. \n chyba:" + e.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "JSON (*.json)|*.json|JSON (*.json)|*.json";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile() as FileStream) != null)
                    {
                        using (myStream)
                        {
                            string extension = Path.GetExtension(myStream.Name);
                            if (extension == ".json")
                            {
                                string fileName = Path.GetFileName(myStream.Name);
                                File.Copy(myStream.Name, AppDomain.CurrentDomain.BaseDirectory + @"answers\" + fileName, true);
                                LoadOfflinePackages();
                            }
                            else
                            {
                                MessageBox.Show("Přípona souboru nerozeznána.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. \n Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PositionPicker p = new PositionPicker();
            if(p.ShowDialog() == DialogResult.OK)
            {
                prefs.anchorA = new PointF((int)Math.Min(p.percentA.X, p.percentB.X)/100f,(int) Math.Min(p.percentA.Y, p.percentB.Y)/100f);
                prefs.anchorB = new PointF((int)Math.Max(p.percentA.X, p.percentB.X)/100f, (int)Math.Max(p.percentA.Y, p.percentB.Y)/100f);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            prefs.onlineMode = checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prefs.showMode = false;
            ToggleButtons();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            prefs.showMode = true;
            ToggleButtons();
        }

        void ToggleButtons()
        {
            if (prefs.showMode)
            {
                button3.Enabled = true;
                button4.Enabled = false;
                button3.Focus();
                panel1.Visible = false;
                panel2.Visible = true;
            }
            else
            {
                button3.Enabled = false;
                button4.Enabled = true;
                button4.Focus();
                panel1.Visible = true;
                panel2.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KeyDialog kd = new KeyDialog();
            if(kd.ShowDialog() == DialogResult.OK)
            {
                prefs.showKey = kd.pressedKey;
                RefreshBindings();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            KeyDialog kd = new KeyDialog();
            if (kd.ShowDialog() == DialogResult.OK)
            {
                prefs.hideKey = kd.pressedKey;
                RefreshBindings();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            KeyDialog kd = new KeyDialog();
            if (kd.ShowDialog() == DialogResult.OK)
            {
                prefs.toggleShowKey = kd.pressedKey;
                RefreshBindings();
            }
        }

        void RefreshBindings()
        {
            button5.Text = prefs.showKey.ToString();
            button6.Text = prefs.hideKey.ToString();
            button7.Text = prefs.toggleShowKey.ToString();
        }
    }
}
