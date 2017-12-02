using Emgu.CV;
using Emgu.CV.Structure;

namespace xamarin_android.Recognition
{
    class ColorRange
    {
        public double R1;
        public double G1;
        public double B1;

        public double R2;
        public double G2;
        public double B2;

       public ColorRange(int R, int G, int B)
       {
            Bgr bgr = new Bgr(B, G, R);
            Image<Bgr, byte> image = new Image<Bgr, byte>(1, 1, bgr);
            CvInvoke.CvtColor(image, image, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
            B = (int)image[0, 0].Blue;
            G = (int)image[0, 0].Green;
            R = (int)image[0, 0].Red;
            R1 = R - 10;
            R2 = R + 10;
            G1 = G - 10;
            G2 = G + 10;
            B1 = B - 10;
            B2 = B + 10;
       }

        public ScalarArray LowBound()
        {
            return new ScalarArray(new MCvScalar(B1, G1, R1));
        }

        public ScalarArray HighBound()
        {
            return new ScalarArray(new MCvScalar(B2, G2, R2));
        }
    }
}