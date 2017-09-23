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

namespace Vid
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat tmp;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private Mat sfcnt(Mat dif, Mat camera)
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


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(capture == null)
            {
                OpenFileDialog opf = new OpenFileDialog();
                //opf.Filter = "Video files | *.avi; *.mp4; *.mov";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    capture = new VideoCapture(opf.FileName);
                }
            }
            capture.ImageGrabbed += Capture_ImageGrabbed1;
            capture.Start();
        }

        private void Capture_ImageGrabbed1(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                Mat g1 = new Mat();
                Mat g2 = new Mat();
                Mat dif = new Mat();
                if (tmp == null)
                {
                    tmp = m.Clone();
                }
                else
                {
                    CvInvoke.CvtColor(m, g1, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);
                    CvInvoke.CvtColor(tmp, g2, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);
                    CvInvoke.AbsDiff(g1, g2, dif);
                    CvInvoke.Threshold(dif, dif, 20, 255, 0);
                    CvInvoke.Blur(dif, dif, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
                    CvInvoke.Threshold(dif, dif, 20, 255, 0);
                    CvInvoke.Blur(dif, dif, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
                    CvInvoke.Threshold(dif, dif, 20, 255, 0);
                    CvInvoke.Blur(dif, dif, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
                    CvInvoke.Threshold(dif, dif, 20, 255, 0);
                    CvInvoke.Blur(dif, dif, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
                    CvInvoke.Threshold(dif, dif, 20, 255, 0);

                    tmp = m.Clone();
                    m = sfcnt(dif, m);
                }
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = m.ToImage<Bgr, byte>().Bitmap;
                pictureBox2.Image = dif.ToImage<Bgr, byte>().Bitmap;
                Thread.Sleep((int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            }
            catch (Exception)
            {

            }
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture = null;
                pictureBox1.Image = null;
            }
        }

        private void pauseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture.Stop();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
