using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vid
{
    public struct col
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
    public class Global
    {
        public static String text = ",";
        public static Boolean videoFromFile = false;
        public static OpenFileDialog name;
        public static col colors = new col();
    }
}
