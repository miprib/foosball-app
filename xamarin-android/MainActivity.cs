using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using System;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;

namespace xamarin_android
{
    [Activity(Label = "xamarin_android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        VideoCapture capture;
        ImageView imageView;

        MCvScalar sk = new MCvScalar();
        Point po = new Point(-1, -1);
        Size sz = new Size(3, 3);
        Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
   
            imageView = FindViewById<ImageView> (Resource.Id.imageView);
            Button button1 = FindViewById<Button>(Resource.Id.button);
            button1.Click += (sender, e) => {
                // Perform action on click
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };
        }
       

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            {
            if (resultCode == Result.Ok)
                capture = new VideoCapture(data.DataString);
                imageView.SetImageURI(data.Data);
            }
            if (capture != null)
            {
                capture.ImageGrabbed += Capture_ImageGrabbed;
                capture.Start();
            }
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                Mat g = new Mat();
                Mat n = new Mat();
                Mat k = new Mat();
                Mat hsv = new Mat();
                CvInvoke.CvtColor(m, g, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

                CvInvoke.InRange(g, new ScalarArray(new MCvScalar(0, 100, 100)), new ScalarArray(new MCvScalar(10, 255, 255)), g);  // išskiriam raudona spalva per tas tris eilutes

                CvInvoke.Blur(g, g, sz, po); // išryškinam paveikslėlį
                CvInvoke.Dilate(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
                CvInvoke.Erode(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);


                CircleF[] circles = CvInvoke.HoughCircles(g, Emgu.CV.CvEnum.HoughType.Gradient, 2, g.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj


                Image<Bgr, Byte> circleImage = m.ToImage<Bgr, byte>();
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine; //dvi eilut4s tekstui
                    //textBox1.Invoke(new Action(() => textBox1.AppendText(text))); // reikia kreiptis taip, nes is kito threado negalima toliau test visko
                    circleImage.Draw(circle, new Bgr(Color.Red), 4); // apibrėžiam apvalius
                }

                //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos
                //pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                //pictureBox1.Image = g.ToImage<Bgr, byte>().Bitmap;
                imageView.SetImageBitmap(circleImage.Bitmap);
                imageView.Image = circleImage.Bitmap;
                Thread.Sleep((int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            }
            catch (Exception)
            {

            }
        }
    }
}

