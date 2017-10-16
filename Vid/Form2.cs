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
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox3.Text = "0";
            pictureBox1.BackColor = Color.Black;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global.text = textBox1.Text + "," + textBox2.Text;
            Global.blu = trackBar1.Value;
            Global.grn = trackBar2.Value;
            Global.red = trackBar3.Value;

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
            Global.text = ",";
            Application.Exit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar1.Value.ToString();
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = trackBar2.Value.ToString();
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = trackBar3.Value.ToString();
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            trackBar1.Value = Int32.Parse(textBox3.Text);
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            trackBar2.Value = Int32.Parse(textBox4.Text);
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            trackBar3.Value = Int32.Parse(textBox5.Text);
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Global.text = textBox1.Text + "," + textBox2.Text;

            Global.blu = trackBar1.Value;
            Global.grn = trackBar2.Value;
            Global.red = trackBar3.Value;

            Global.n = false;
            this.Close();
        }
    }
}
