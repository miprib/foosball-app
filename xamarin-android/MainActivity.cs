using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using System;

using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Android.Graphics;
using Android.Content.PM;
using Camera = Android.Hardware.Camera;
using Xamarin.Forms;

namespace xamarin_android
{
    [Activity(Label = "xamarin_android", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity , TextureView.ISurfaceTextureListener
    {
        VideoCapture capture;
        ImageView imageView;

        private Camera camera;
        TextureView textureView;

        MCvScalar sk = new MCvScalar();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            textureView = new TextureView(this);
            textureView.SurfaceTextureListener = this;
            SetContentView(textureView);

            /*imageView = FindViewById<ImageView> (Resource.Id.imageView);
            Button button1 = FindViewById<Button>(Resource.Id.button);
            button1.Click += (sender, e) => {
                // Perform action on click
                var imageIntent = new Intent();
                imageIntent.SetType("video/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };*/
        }
       

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            {
            if (resultCode == Result.Ok)
                capture = new VideoCapture(AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\" + "\\data\\video.mp4");
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

                Mat ball = Detailing.Detail(m);

                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj


                Image<Bgr, Byte> circleImage = ball.ToImage<Bgr, byte>();
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine;
                }
                
                imageView.SetImageBitmap(circleImage.Bitmap);
            }
            catch (Exception)
            {

            }
        }
        /** method that opens camera when the activity is launched
         */
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            camera = Camera.Open();

            try
            {
                camera.SetPreviewTexture(surface);
                camera.StartPreview();
            }
            catch (Exception t)
            {}

            var metrics = Resources.DisplayMetrics;

            Camera.Parameters parameters = camera.GetParameters();
            parameters.SetPreviewSize((int)metrics.WidthPixels, (int)metrics.HeightPixels);
            parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (camera != null)
            {
                camera.StopPreview();
                camera.Release();
                camera = null;
            }
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        /** method that gets called with every frame. retrieve frame with 
         *  Bitmap frameBitmap = textureView.Bitmap;
         */
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            try
            {
                Bitmap frame = textureView.Bitmap;
                Image<Bgr, byte> fr = new Image<Bgr, byte>(frame);
                Mat m = fr.Mat;

                Mat ball = Detailing.Detail(m);

                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine;
                }

                imageView.SetImageBitmap(ball.Bitmap);
            }
            catch (Exception)
            {

            }
        }
    }
}

