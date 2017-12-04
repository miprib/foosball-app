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

       public ColorRange(int B, int G, int R)
       {
            Bgr bgr = new Bgr(B, G, R);
            Image<Bgr, byte> image = new Image<Bgr, byte>(1, 1, bgr);
            CvInvoke.CvtColor(image, image, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
            B = (int)image[0, 0].Blue;
            G = (int)image[0, 0].Green;
            R = (int)image[0, 0].Red;
            if(R > 230)
            {
                R1 = 205;
                R2 = 255;
            }
            else
            {
                if (R < 25)
                {
                    R1 = 0;
                    R2 = 50;
                }
                else
                {
                    R1 = R - 25;
                    R2 = R + 25;
                }

            }
            if (G > 230)
            {
                G1 = 205;
                G2 = 255;
            }
            else
            {
                if (G < 25)
                {
                    G1 = 0;
                    G2 = 50;
                }
                else
                {
                    G1 = G - 25;
                    G2 = G + 25;
                }

            }
            if (B > 230)
            {
                B1 = 205;
                B2 = 255;
            }
            else
            {
                if (B < 25)
                {
                    B1 = 0;
                    B2 = 50;
                }
                else
                {
                    B1 = B - 25;
                    B2 = B + 25;
                }
            }
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