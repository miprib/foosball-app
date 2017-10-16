using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vid
{
    class Detaling
    {
        private static MCvScalar sk = new MCvScalar();
        Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

        public Mat ball_only(Mat a)
        {
            Mat bwmat = new Mat();
            CvInvoke.InRange(a, new ScalarArray(new MCvScalar((double)Global.colors.B1, (double)Global.colors.G1, (double)Global.colors.R1)), new ScalarArray(new MCvScalar((double)Global.colors.B2, (double)Global.colors.G2, (double)Global.colors.R2)), bwmat);

            CvInvoke.MedianBlur(bwmat, bwmat, 7);

            CvInvoke.Dilate(bwmat, bwmat, s, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, sk);
            CvInvoke.Erode(bwmat, bwmat, s, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, sk);

            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1,-1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            CvInvoke.Blur(bwmat, bwmat, new Size(10, 10), new Point(-1, -1), Emgu.CV.CvEnum.BorderType.Default);
            CvInvoke.Threshold(bwmat, bwmat, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            return bwmat;
        }
    }
}
