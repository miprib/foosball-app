using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Threading.Tasks;

namespace xamarin_android
{
    class Detailing
    {
        public static Mat Detail(Mat frame)
        {
            Mat ball = new Mat();
            //Bandžiau padaryt, kad keitimas į hsv būtu atskiruose threduose, kad greičiau veiktų, bet nieko nepadėjo
            /*
            System.Drawing.Rectangle topLRect = new Rectangle(0, 0, frame.Width / 2, (frame.Height / 2));
            System.Drawing.Rectangle topRRect = new Rectangle(frame.Width / 2, 0, frame.Width / 2, (frame.Height / 2));
            System.Drawing.Rectangle bottomLRect = new Rectangle(0, frame.Height / 2, frame.Width/2, (frame.Height / 2));
            System.Drawing.Rectangle bottomRRect = new Rectangle((frame.Width / 2), frame.Height / 2, frame.Width/2, (frame.Height / 2));

            Mat topL = new Mat(frame, topLRect);
            Mat topR = new Mat(frame, topRRect);
            Mat bottomL = new Mat(frame, bottomLRect);
            Mat bottomR = new Mat(frame, bottomRRect);

            var toHSV1 = Task.Run(() => CvInvoke.CvtColor(topL, topL, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv));
            var toHSV2 = Task.Run(() => CvInvoke.CvtColor(topR, topR, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv));
            var toHSV3 = Task.Run(() => CvInvoke.CvtColor(bottomL, bottomL, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv));
            var toHSV4 = Task.Run(() => CvInvoke.CvtColor(bottomR, bottomR, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv));


            await toHSV1;
            await toHSV2;
            await toHSV3;
            await toHSV4;

            Mat left = new Mat();
            Mat right = new Mat();

            CvInvoke.VConcat(topL, bottomL, left);
            CvInvoke.VConcat(topR, bottomR, right);

            CvInvoke.HConcat(left, right, ball);
            */

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

            return ball;
        }
    }
}