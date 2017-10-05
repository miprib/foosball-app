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

using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;
using Emgu.CV.Cvb;
using Emgu.CV.Util;

namespace Vid
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        MCvScalar sk = new MCvScalar();
        Point po = new Point(-1, -1);
        Size sz = new Size(3, 3);
        Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1,-1));
        int ff = 0;
        int xlatest;
        int bluet = 0, redt = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                OpenFileDialog opf = new OpenFileDialog
                {
                    Filter = "Video files | *.avi; *.mp4; *.mov"
                };
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    capture = new VideoCapture(opf.FileName);
                }
                if (capture != null)
                {
                    richTextBox1.Text = redt.ToString();
                    richTextBox2.Text = bluet.ToString();
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(opf.FileName);
                    capture.ImageGrabbed += Capture_ImageGrabbed1;
                    capture.Start();
                }
            }
        }


        private Mat colorcang(Mat a) {
            Mat gate = new Mat();
            CvInvoke.InRange(a, new ScalarArray(new MCvScalar(0, 200, 200)), new ScalarArray(new MCvScalar(10, 250, 250)), gate);

            CvInvoke.MedianBlur(gate, gate, 7);

            CvInvoke.Dilate(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
            CvInvoke.Erode(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);

            CvInvoke.Blur(gate, gate, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            return gate;
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {  
                Mat m = new Mat();
                capture.Retrieve(m);
                Mat ball = new Mat();
                //Mat gate = new Mat();
                Mat hsv = new Mat();

                CvInvoke.CvtColor(m, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai


                ball = colorcang(hsv);
                /*CvInvoke.InRange(hsv, new ScalarArray(new MCvScalar(0,200,200)), new ScalarArray(new MCvScalar(10,250,250)), ball);  // išskiriam geltona, nes hsv filtras uzdetas
/*
                CvInvoke.InRange(hsv, new ScalarArray(new MCvScalar(0, 230, 0)), new ScalarArray(new MCvScalar(20, 255, 20)), gate);

                CvInvoke.MedianBlur(gate, gate, 7);
                CvInvoke.Dilate(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);

                CvInvoke.MedianBlur(ball, ball, 5); // išryškinam paveikslėlį
                CvInvoke.Dilate(ball, ball, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(ball, ball, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);*/


                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows/4, 60, 30, 15,40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                ff++;
                foreach (CircleF circle in circles)
                {
                    ff = 0;
                    string text = "ball position: x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + Environment.NewLine; //dvi eilut4s tekstui
                    textBox1.Invoke(new Action(() => textBox1.AppendText(text))); // reikia kreiptis taip, nes is kito threado negalima toliau test visko
                    xlatest = (int)circle.Center.X;

                }

                if (ff == 40)
                {
                    if(m.Size.Width - m.Size.Width / 10 * 2 < xlatest)
                    {
                        bluet++;
                        richTextBox1.Invoke(new Action(() => richTextBox1.Text = bluet.ToString()));
                    }

                    if (m.Size.Width - m.Size.Width /10 *8 > xlatest)
                    {
                        redt++;
                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = redt.ToString()));
                    }
                }


                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox1.Image = ball.Bitmap; 
                pictureBox2.Image = hsv.Bitmap;
                GC.Collect();
                //Thread.Sleep((int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            }
            catch (Exception)
            {

            }
        }

        private void StopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture = null;
                pictureBox1.Image = null;
            }
        }

        private void PauseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture.Stop();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VideoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
