using Emgu.CV;
using Emgu.CV.Structure;
using System;
using Emgu.CV.Features2D;
using Android.Graphics;

namespace xamarin_android.Recognition
{
    static class Processing
    {
        public static Bitmap ToHSV(Bitmap frame)
        {
            Mat mat = new Mat(frame);
            frame.Dispose();

            CvInvoke.CvtColor(mat, mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            frame = mat.Bitmap;
            mat.Dispose();

            return frame;
        }

        public static Bitmap Color(Bitmap frame, ColorRange colorRange)
        {
            Mat mat = new Mat(frame);
            frame.Dispose();

            CvInvoke.InRange(mat, colorRange.LowBound(), colorRange.HighBound(), mat);
            CvInvoke.BitwiseNot(mat, mat);

            frame = mat.Bitmap;
            mat.Dispose();

            return frame;
        }

        public static Coordinates FindBall(Bitmap frame)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(frame);
            frame.Dispose();
            Mat mat = image.Mat;
            image.Dispose();

            SimpleBlobDetector blobDetector = new SimpleBlobDetector();
            MKeyPoint[] blobs = blobDetector.Detect(mat);
            mat.Dispose();

            Coordinates coordinates = new Coordinates();

            foreach(MKeyPoint blob in blobs)
            {
                coordinates = new Coordinates(blob.Point);
                Console.WriteLine(coordinates.ToString());
            }
            return coordinates;
        }
/*


        public static void FindCircle(Mat ball)
        {
            SimpleBlobDetector  blobDetector = new SimpleBlobDetector();

            Coordinates coo;

            MKeyPoint[] blobs = blobDetector.Detect(ball);
            i++;
            foreach (MKeyPoint blob in blobs)
            {
                coo = new Coordinates(blob.Point);
                j = i - j;
                string d = "Frames lost " + (j-1) + " " + coo.ToString() + Environment.NewLine;
                j = i;
                Console.WriteLine(d);
            }
        }

        private static bool scored = false;
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
        }*/
    }
}