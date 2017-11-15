using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Threading.Tasks;
using System;

namespace xamarin_android
{
    class Recognition
    {
        public static Mat findColor(Mat frame)
        {

            CvInvoke.CvtColor(frame, frame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

            CvInvoke.InRange(frame, new ScalarArray(new MCvScalar(0, 205, 205)), new ScalarArray(new MCvScalar(50, 255, 255)), frame);  // išskiriam raudona spalva per tas tris eilutes

            return frame;
        }

        public static Mat Detailing(Mat frame)
        {
            Mat rect = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
            MCvScalar empt = new MCvScalar();
            System.Drawing.Point poin = new System.Drawing.Point();

            CvInvoke.MedianBlur(frame, frame, 7);

            CvInvoke.Dilate(frame, frame, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);
            CvInvoke.Erode(frame, frame, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);

            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(frame, frame, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(frame, frame, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            return frame;
        }

        public static CircleF[] FindCircle(Mat ball)
        {
            CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj 
            return circles;
        }
    }
}