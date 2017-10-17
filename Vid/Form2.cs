using Emgu.CV;
using Emgu.CV.Structure;
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
            textBox3.Text = "50";
            textBox4.Text = "70";
            textBox5.Text = "230";
            pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("First player name is empty", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Second player name is empty", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            Global.text = textBox1.Text + "," + textBox2.Text;

            Image<Bgr, byte> lll = new Image<Bgr, byte>(50, 50, new Bgr(trackBar1.Value, trackBar2.Value, trackBar3.Value));

            Mat ll = lll.Mat;
            CvInvoke.CvtColor(ll, ll, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            Bgr color = (ll.ToImage<Bgr, byte>())[20, 20];

            Global.colors = new Col(color.Blue, color.Green, color.Red);

            OpenFileDialog opf = new OpenFileDialog
            {
                Filter = "Video files | *.avi; *.mp4; *.mov"
            };
            if (opf.ShowDialog() == DialogResult.OK)
            {
                Global.name = opf;
            }
            Global.videoFromFile = true;
            Global.cancel = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Global.cancel = true;
            this.Close();
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
            try
            {
                trackBar1.Value = Int32.Parse(textBox3.Text);
            }
            catch
            {
                trackBar1.Value = 0;
            }
            finally
            {
                pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value = Int32.Parse(textBox4.Text);
            }
            catch
            {
                trackBar2.Value = 0;
            }
            finally
            {
                pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar3.Value = Int32.Parse(textBox5.Text);
            }
            catch
            {
                trackBar3.Value = 0;
            }
            finally
            {
                pictureBox1.BackColor = Color.FromArgb(trackBar3.Value, trackBar2.Value, trackBar1.Value);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Global.text = textBox1.Text + "," + textBox2.Text;

            Image<Bgr, byte> lll = new Image<Bgr, byte>(50, 50, new Bgr(trackBar1.Value, trackBar2.Value, trackBar3.Value));

            Mat ll = lll.Mat;
            CvInvoke.CvtColor(ll, ll, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            Bgr color = (ll.ToImage<Bgr, byte>())[20, 20];

            Global.colors = new Col(color.Blue, color.Green, color.Red);

            Global.videoFromFile = false;
            Global.cancel = false;
            this.Close();
        }
    }
}
