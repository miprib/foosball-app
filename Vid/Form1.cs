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
        VideoCapture capture;
        Boolean n = true;

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
                Detaling det = new Detaling();
                ball = det.Ball_only(m);

                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                ff++;
                Coordinates coo;
                CooString str = new CooString();
                foreach (CircleF circle in circles)
                {
                    ff = 0;
                    coo = new Coordinates((int)circle.Center.X, (int)circle.Center.Y);
                    textBox1.Invoke(new Action(() => textBox1.AppendText(str.Coordinates_to_string(coo)))); // reikia kreiptis taip, nes is kito threado negalima toliau test 
                    xlatest = (int)circle.Center.X;
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
