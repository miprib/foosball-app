using Emgu.CV;
using Emgu.CV.Structure;
using System;
using Emgu.CV.Cvb;
using Android.Graphics;
using Emgu.CV.Features2D;

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
            //CvInvoke.BitwiseNot(mat, mat);

            frame = mat.Bitmap;
            mat.Dispose();

            return frame;
        }

        public static Coordinates FindBall(Bitmap frame)
        {
            Image<Gray,byte> image = new Image<Gray, byte>(frame);
            frame.Dispose();

           // CvBlobDetector blobDetector = new CvBlobDetector();
            SimpleBlobDetector detector = new SimpleBlobDetector();
            MKeyPoint[] points =  detector.Detect(image);
           // CvBlobs blobs = new CvBlobs();
            //uint a = blobDetector.Detect(image, blobs);
            image.Dispose();

            Coordinates coordinates = new Coordinates();

            /*foreach(var blob in blobs)
            {
                coordinates = new Coordinates(blob.Value.Centroid);
                Console.WriteLine(coordinates.ToString());
            }*/

            foreach(MKeyPoint point in points)
            {
                coordinates = new Coordinates(point.Point);
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