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

            prefs.onlineMode = true;
            prefs.apiKey = "LightCap1337Eva32";
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

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            prefs.onlineMode = checkBox1.Checked;
        }
    }
}
