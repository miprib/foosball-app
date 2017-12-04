using Emgu.CV;
using Emgu.CV.Structure;
using System;
using Emgu.CV.Features2D;
using Android.Graphics;

namespace xamarin_android.Recognition
{
    static class Processing
    {
        public static Mat ToHSV(Bitmap bmp, ColorRange colorRange)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(bmp);
            bmp.Dispose();

            Image<Hsv, byte> imageh = image.Convert<Hsv, byte>();
            image.Dispose();

            Mat frame = imageh.Mat;

            CvInvoke.InRange(frame, colorRange.LowBound(), colorRange.HighBound(), frame);
            CvInvoke.BitwiseNot(frame, frame);

            return frame;
        }

        public static Coordinates FindBall(Mat frame)
        {
            SimpleBlobDetector detector = new SimpleBlobDetector();
            MKeyPoint[] points = detector.Detect(frame);
            frame.Dispose();

            Coordinates coordinates = new Coordinates();

            foreach (MKeyPoint point in points)
            {
                coordinates = new Coordinates(point.Point);
            }
            return coordinates;
        }
    }
}