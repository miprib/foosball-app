﻿using System;
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
        private struct coordinates
        {
            public int x;
            public int y;

            public coordinates(int tx, int ty)
            {
                x = tx;
                y = ty;
            }
        }

        VideoCapture capture;
        MCvScalar sk = new MCvScalar();
        Point po = new Point(-1, -1);
        Size sz = new Size(3, 3);
        Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1,-1));

        int ff = 0;
        int xlatest;
        int left = 0, right = 0;


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
                Vid.Form2 a = new Form2();
                a.ShowDialog();
                String[] names = Global.text.Split(',');

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
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(opf.FileName);



                 


                    label1.Text = names[0];
                    label2.Text = names[1];

                    capture.ImageGrabbed += Capture_ImageGrabbed1;
                    capture.Start();
                }
            }
        }


        private Mat ball_only(Mat a) {
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

        private void print_coordinates(coordinates n)
        {

            string text = "ball position: x " + n.x + ", y " + n.y + Environment.NewLine; //dvi eilut4s tekstui
            textBox1.Invoke(new Action(() => textBox1.AppendText(text))); // reikia kreiptis taip, nes is kito threado negalima toliau test visko
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos

                Mat m = new Mat();
                Mat ball = new Mat();
                Mat hsv = new Mat();

                coordinates s = new coordinates();

                capture.Retrieve(m);
                pictureBox1.Image = m.Bitmap;

                CvInvoke.CvtColor(m, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv del spalvu paletes

                ball = ball_only(hsv);

                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                ff++;
                foreach (CircleF circle in circles)
                {
                    ff = 0;
                    s = new coordinates((int)circle.Center.X, (int)circle.Center.Y);
                    xlatest = (int)circle.Center.X;
                    print_coordinates(s);
                }

                if (ff == 15)
                {
                    if (m.Size.Width - m.Size.Width / 10 * 2 < xlatest)
                    {
                        right++;
                        label3.Invoke(new Action(() => label3.Text = right.ToString()));
                    }

                    if (m.Size.Width - m.Size.Width / 10 * 8 > xlatest)
                    {
                        left++;
                        label4.Invoke(new Action(() => label4.Text = left.ToString()));
                    }
                }
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

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
