using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vid
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global.text = textBox1.Text + "," + textBox2.Text;

            OpenFileDialog opf = new OpenFileDialog
            {
                Filter = "Video files | *.avi; *.mp4; *.mov"
            };
            if (opf.ShowDialog() == DialogResult.OK)
            {
                Global.name = opf;
            }
            Global.n = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.text = textBox1.Text + "," + textBox2.Text;

            Global.n = false;
            this.Close();
        }
    }
}
