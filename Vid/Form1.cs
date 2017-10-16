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
using Newtonsoft.Json;

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

        private struct col
        {
            public double B1, B2, G1, G2, R1, R2;

            public col(int a, int b, int c)
            {
                if (a - 25 < 0)
                {
                    B1 = 0;
                    B2 = 50;
                }
                else
                {
                    if (a + 25 > 255)
                    {
                        B1 = 205;
                        B2 = 255;
                    }
                    else
                    {
                        B1 = a - 25;
                        B2 = a + 25;
                    }
                }
                if (b - 25 < 0)
                {
                    G1 = 0;
                    G2 = 50;
                }
                else
                {
                    if (b + 25 > 255)
                    {
                        G1 = 205;
                        G2 = 255;
                    }
                    else
                    {
                        G1 = b - 25;
                        G2 = b + 25;
                    }
                }
                if (c - 25 < 0)
                {
                    R1 = 0;
                    R2 = 50;
                }
                else
                {
                    if (c + 25 > 255)
                    {
                        R1 = 205;
                        R2 = 255;
                    }
                    else
                    {
                        R1 = c - 25;
                        R2 = c + 25;
                    }
                }
            }
        }

        enum Colors
        {
            B1 = 0, G1 = 200, R1 = 200,
            B2 = 10, G2 = 250, R2 = 250
        }

        VideoCapture capture;
        MCvScalar sk = new MCvScalar();
        Point po = new Point(-1, -1);
        Size sz = new Size(3, 3);
        Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
        Boolean n = true;
        col colors = new col();

        GameList gameList;
        Game game;

        int ff = 0;
        int xlatest;
        int left = 0, right = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameList = GameList.NewInstance();

        }

        private void StartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                Vid.Form2 a = new Form2();
                a.ShowDialog();
                String[] names = Global.text.Split(',');

                label1.Text = names[0];
                label2.Text = names[1];


                Image<Bgr, byte> lll = new Image<Bgr, byte>(50, 50, new Bgr(Global.blu, Global.grn, Global.red));

                Mat ll = lll.Mat;
                CvInvoke.CvtColor(ll, ll, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

                Bgr color = (ll.ToImage<Bgr, byte>())[20, 20];

                Global.blu = (int)color.Blue;
                Global.grn = (int)color.Green;
                Global.red = (int)color.Red;

                colors = new col(Global.blu, Global.grn, Global.red);

                if (Global.videoFromFile)
                {
                    if (Global.name != null)
                    {
                        capture = new VideoCapture(Global.name.FileName);
                        if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                        textBox1.AppendText(Global.name.FileName + Environment.NewLine);
                    }
                }
                else
                {
                    capture = new VideoCapture(0);
                    if (textBox1.Text != "") textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("Video filmuojamas kamera" + Environment.NewLine);
                }

                if (capture != null)
                {
                    //save new game info
                    game = new Game();
                    game.id = gameList.NextId();
                    game.Team1 = names[0];
                    game.Team2 = names[1];
                    game.date = DateTime.Now;
                    gameList.Add(game);
                    gameList.SaveList();

                    capture.ImageGrabbed += Capture_ImageGrabbed1;
                }
            }
            if (capture != null)
                capture.Start();
        }


        private Mat ball_only(Mat a)
        {
            Mat gate = new Mat();
            CvInvoke.InRange(a, new ScalarArray(new MCvScalar((double)colors.B1, (double)colors.G1, (double)colors.R1)), new ScalarArray(new MCvScalar((double)colors.B2, (double)colors.G2, (double)colors.R2)), gate);

            CvInvoke.MedianBlur(gate, gate, 7);

            CvInvoke.Dilate(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
            CvInvoke.Erode(gate, gate, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);

            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(gate, gate, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(gate, gate, new Size(10, 10), po, Emgu.CV.CvEnum.BorderType.Default);
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
                capture.Retrieve(m);
                pictureBox1.Image = m.Bitmap;
                if (n)
                {
                    xlatest = m.Size.Width / 2;
                    n = !n;
                }

                Mat ball = new Mat(m.Size, Emgu.CV.CvEnum.DepthType.Cv8U, 3);

                coordinates s = new coordinates();

                CvInvoke.CvtColor(m, ball, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv del spalvu paletes

                ball = ball_only(ball);

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
                    if (m.Size.Width - m.Size.Width / 4 < xlatest)
                    {
                        right++;
                        label3.Invoke(new Action(() => label3.Text = right.ToString()));
                        //save score for the last game started
                        gameList.ElementAt(game.id).Team1Score = right;
                        gameList.SaveList();
                    }

                    if (m.Size.Width - m.Size.Width / 4 * 3 > xlatest)
                    {
                        left++;
                        label4.Invoke(new Action(() => label4.Text = left.ToString()));
                        //save score for the last game started
                        gameList.ElementAt(game.id).Team2Score = left;
                        gameList.SaveList();
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
                
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create new form for game history
            Vid.HistoryForm history = new HistoryForm();
            history.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
