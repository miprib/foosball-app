using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        //Mat tmp;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private Mat Sfcnt(Mat dif, Mat camera)
        {
            //Boolean found = false;
            Mat t = new Mat();
            dif.CopyTo(t);

            CvBlobs blobs = new CvBlobs();
            CvBlobDetector b = new CvBlobDetector();
            b.Detect(t.ToImage<Gray, byte>(), blobs);
            blobs.FilterByArea(100, int.MaxValue);
            CvTracks tr = new CvTracks();

            float scale = (camera.Height + camera.Width) / 2.0f;
            tr.Update(blobs, 0.01 * scale, 5, 5);

            foreach(var pair in tr)
            {
                CvTrack ak = pair.Value;
                Rectangle kv = new Rectangle();
                kv = ak.BoundingBox;
                kv.Width = (int)(ak.MaxX - ak.MinX);
                kv.Height = (int)(ak.MaxY - ak.MinY);
                CvInvoke.Rectangle(camera, ak.BoundingBox, new MCvScalar(255.0, 0.0, 0.0), 2);
            }

            return camera;


            /*Mat contours = new Mat();
            Mat hierarcy = new Mat();

            CvInvoke.FindContours(t, contours, hierarcy, Emgu.CV.CvEnum.RetrType.External,Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            
            if(contours.Size.IsEmpty)
            {
                found = false;
            }
            else
            {
                found = true;
            }
            if (found)
            {
               Mat largestContour = new Mat();
                optical
            }*/
        }


        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void StartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(capture == null)
            {
                OpenFileDialog opf = new OpenFileDialog
                {
                    Filter = "Video files | *.avi; *.mp4; *.mov"
                };
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    capture = new VideoCapture(opf.FileName);
                }
            }
            //check if video was selected
            if (capture != null)
            {
                capture.ImageGrabbed += Capture_ImageGrabbed1;
                capture.Start();
            }
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {  
                Mat m = new Mat();
                capture.Retrieve(m);
                UMat g = new UMat();
                Mat n = new Mat();
                Mat k = new Mat();
                CvInvoke.CvtColor(m, g, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

                CvInvoke.InRange(g, new ScalarArray(new MCvScalar(0,100,100)), new ScalarArray(new MCvScalar(10,255,255)), n);  // išskiriam raudona spalva per tas tris eilutes
                CvInvoke.InRange(g, new ScalarArray(new MCvScalar(160, 100, 100)), new ScalarArray(new MCvScalar(179, 255, 255)), k);
                CvInvoke.Add(n, k, g);

                CvInvoke.Blur(g, g, new Size(3, 3), new Point(-1, -1)); // išryškinam paveikslėlį
                CvInvoke.Dilate(g, g, CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1)), new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar());
                CvInvoke.Erode(g, g, CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1)), new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar());


                CircleF[] circles = CvInvoke.HoughCircles(g, Emgu.CV.CvEnum.HoughType.Gradient, 2, g.Rows/4, 60, 30, 5,100); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                
                Image<Bgr, Byte> circleImage = m.ToImage<Bgr,byte>();
                foreach (CircleF circle in circles)
                {
                    circleImage.Draw(circle, new Bgr(Color.Red), 4); // apibrėžiam apvalius
                }

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
    }
}
