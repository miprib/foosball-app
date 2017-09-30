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
        BindingList<string> sarasiukas = new BindingList<string>();


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
<<<<<<< HEAD
                if (capture != null)
                {
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(opf.FileName);
                    capture.ImageGrabbed += Capture_ImageGrabbed1;
                    capture.Start();
                }
=======
            }
            //check if video was selecte
            if (capture != null)
            {
                capture.ImageGrabbed += Capture_ImageGrabbed1;
                capture.Start();
>>>>>>> 2420f1ad0ecab09da2b8edceaca1e150e8ef1439
            }
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {  
                Mat m = new Mat();
                capture.Retrieve(m);
                Mat g = new Mat();
                Mat n = new Mat();
                Mat k = new Mat();
                Mat hsv = new Mat();
                CvInvoke.CvtColor(m, g, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

                CvInvoke.InRange(g, new ScalarArray(new MCvScalar(0,100,100)), new ScalarArray(new MCvScalar(10,255,255)), g);  // išskiriam raudona spalva per tas tris eilutes
                //CvInvoke.InRange(g, new ScalarArray(new MCvScalar(160, 100, 100)), new ScalarArray(new MCvScalar(179, 255, 255)), k);
                //CvInvoke.Add(n, k, g);

                CvInvoke.Blur(g, g, sz, po); // išryškinam paveikslėlį
                CvInvoke.Dilate(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);


                CircleF[] circles = CvInvoke.HoughCircles(g, Emgu.CV.CvEnum.HoughType.Gradient, 2, g.Rows/4, 60, 30, 15,40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                
                Image<Bgr, Byte> circleImage = m.ToImage<Bgr,byte>();
                foreach (CircleF circle in circles)
                {
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("ball position = x " + circle.Center.X.ToString().PadLeft(4) + ", y " + circle.Center.Y.ToString().PadLeft(4));
                    circleImage.Draw(circle, new Bgr(Color.Red), 4); // apibrėžiam apvalius
                }

               // listBox1.DataSource = null;        // sitos dvi turetu atnaujint sarasa
               // listBox1.DataSource = sarasiukas;

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox1.Image = g.ToImage<Bgr, byte>().Bitmap; 
                pictureBox2.Image = circleImage.Bitmap;
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
