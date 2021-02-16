using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Lexical_analyzer
{
    public partial class Form1 : Form
    {
        string File_Name = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void runAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager lexeme_Manager = new  Manager();
            dataGridView1.DataSource = lexeme_Manager.getLexemesTable(textBox1.Text);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "Text|*.txt";
            if (dialog.ShowDialog() != DialogResult.OK) return;
            File_Name = dialog.FileName;


            String s0 = Environment.NewLine;
            String[] ss = new String[100];

            StreamReader sr = new StreamReader(File_Name);
            ss = File.ReadAllLines(File_Name);
            int n = ss.Count();
            for (int i = 0; i < n; i++)
                textBox1.Text += (ss[i] + s0);

        }
    }
}
