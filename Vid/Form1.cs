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
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(opf.FileName);
                    capture.ImageGrabbed += Capture_ImageGrabbed1;
                    capture.Start();
                }
            }
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {  
                Mat m = new Mat();
                capture.Retrieve(m);
                Mat ball = new Mat();
                Mat hsv = new Mat();
                Mat gate = new Mat();
                CvInvoke.CvtColor(m, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

                CvInvoke.InRange(hsv, new ScalarArray(new MCvScalar(0,230,0)), new ScalarArray(new MCvScalar(50,255,100)), gate);  // išskiriam juodą

                CvInvoke.MedianBlur(gate, gate, 7);
                CvInvoke.Dilate(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                
                CvInvoke.InRange(hsv, new ScalarArray(new MCvScalar(0,200,200)), new ScalarArray(new MCvScalar(10,250,250)), ball);  // išskiriam geltona, nes hsv filtras uzdetas

                CvInvoke.MedianBlur(ball, ball, 5); // išryškinam paveikslėlį
                CvInvoke.Dilate(ball, ball, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(ball, ball, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);


                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows/4, 60, 30, 15,40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                
                Image<Bgr, Byte> circleImage = m.ToImage<Bgr,byte>();
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + Environment.NewLine; //dvi eilut4s tekstui
                    textBox1.Invoke(new Action(() => textBox1.AppendText(text))); // reikia kreiptis taip, nes is kito threado negalima toliau test visko
                    circleImage.Draw(circle, new Bgr(Color.Red), 4); // apibrėžiam apvalius
                }

                CvInvoke.Canny(gate, gate, 180, 120);

                CvInvoke.Threshold(gate, gate, 10, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                CvInvoke.Blur(gate, gate, new Size(30, 30), new Point(-1, -1));
                CvInvoke.Threshold(gate, gate, 10, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                CvInvoke.Canny(gate, gate, 180, 120);

                CvInvoke.Threshold(gate, gate, 10, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                CvInvoke.Blur(gate, gate, new Size(20, 20), new Point(-1, -1));
                CvInvoke.Threshold(gate, gate, 10, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                CvInvoke.Canny(gate, gate, 180, 120);

                LineSegment2D[] lines = CvInvoke.HoughLinesP(gate, 1, Math.PI / 45, 20, 30, 10);

                Image<Bgr, byte> gateimg = gate.ToImage<Bgr, byte>();

                foreach(var line in lines)
                {
                    gateimg.Draw(line, new Bgr(Color.Red), 4);
                }
                

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox1.Image = gateimg.Bitmap; 
                pictureBox2.Image = hsv.Bitmap;
                Thread.Sleep((int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
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



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
