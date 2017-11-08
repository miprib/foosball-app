using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace xamarin_android
{
    class Detailing
    {
        public static Mat Detail(Mat frame)
        {
            Mat ball = new Mat();

            Mat rect = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
            MCvScalar empt = new MCvScalar();
            System.Drawing.Point poin = new System.Drawing.Point();

            CvInvoke.CvtColor(frame, ball, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai
            CvInvoke.InRange(ball, new ScalarArray(new MCvScalar(0, 205, 205)), new ScalarArray(new MCvScalar(50, 255, 255)), ball);  // išskiriam raudona spalva per tas tris eilutes

            CvInvoke.MedianBlur(ball, ball, 7);

            CvInvoke.Dilate(ball, ball, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);
            CvInvoke.Erode(ball, ball, rect, poin, 1, Emgu.CV.CvEnum.BorderType.Default, empt);

            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(ball, ball, new Size(10, 10), poin, Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(ball, ball, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            return ball;
        }
    }
}