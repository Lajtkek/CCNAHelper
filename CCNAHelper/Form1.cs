using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CCNAHelper
{
    public partial class Form1 : Form
    {
        private Helper h;
        private List<Question> qL = new List<Question>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
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
                                File.Copy(myStream.Name, AppDomain.CurrentDomain.BaseDirectory + @"files\"+ fileName, true);
                                loadAnswers();
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

        private void loadAnswers()
        {
            qL = new List<Question>();
            string filePath = AppDomain.CurrentDomain.BaseDirectory+@"files";
            string prefPath = filePath + @"\prefs.json";

            bool exists = Directory.Exists(filePath);

            if (!exists) Directory.CreateDirectory(filePath);

            FileStream fs = new FileStream(prefPath, FileMode.OpenOrCreate);
            fs.Dispose();

            listBox1.Items.Clear();

            foreach (var file in Directory.GetFiles(filePath,"*.json"))
            {
                string fileName = Path.GetFileName(file);
                if(fileName != "prefs.json")
                {
                    try
                    {
                        string content = File.ReadAllText(file);
                        List<Question> a = JsonConvert.DeserializeObject<List<Question>>(content);

                        foreach (Question q in a)
                        {
                            qL.Add(q);
                        }
                        listBox1.Items.Add(fileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Soubor " + fileName + " není validní json soubor. \n chyba:" + e.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            h = new Helper(qL);
            h.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadAnswers();
        }
    }
}
