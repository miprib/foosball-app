using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace xamarin_android
{
    class Recognition
    {
        public static Mat ToHSV(Mat frame)
        {

            CvInvoke.CvtColor(frame, frame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

            CvInvoke.InRange(frame, new ScalarArray(new MCvScalar(15, 220, 225)), new ScalarArray(new MCvScalar(35, 240, 255)), frame);  // išskiriam raudona spalva per tas tris eilutes

            return frame;
        }

        public static int i;

        public static Game BallRecognition(Mat frame, Game game)
        {
            frame = Detailing(frame);
            Point coordinates = FindCircle(frame);
            int wid = frame.Width;
            frame.Dispose();
            if(i == 5) return ResultCheck(game, coordinates, wid);
            return game;
        }

        public static Mat Detailing(Mat frame)
        {
            Mat rect = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
            MCvScalar empt = new MCvScalar();
            System.Drawing.Point poin = new System.Drawing.Point();

            CvInvoke.MedianBlur(frame, frame, 7);

            CvInvoke.Dilate(frame, frame, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);
            CvInvoke.Erode(frame, frame, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);

            rect.Dispose();

            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            return frame;
        }

        public static Point coordinates = new Point();

        public static Point FindCircle(Mat ball)
        {
            CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj 
            
            if (circles.Length <= 0) {
                i++;
            }
            else
            {
                i = 0;
                coordinates.X = (int)circles[0].Center.X;
                coordinates.Y = (int)circles[0].Center.Y;
            }

            return coordinates;
        }

        private static bool scored = false;

        public static Game ResultCheck(Game game, Point coordinates, int x)
        {
            if(x/4 < coordinates.X)
            {
                game.team1Score++;
                scored = true;
            }
            if(coordinates.Y > x * 3 / 4)
            {
                game.team2Score++;
                scored = true;
            }
            Console.WriteLine("Result: {0}-{1}", game.team1Score, game.team2Score);
            return game;
        }

        public static bool IsScored()
        {
            if (scored)
            {
                scored = !scored;
                return !scored;
            }
            else {
                return scored;
            }
        }
    }
}