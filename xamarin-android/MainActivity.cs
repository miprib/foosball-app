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
using Android.Media;
using Android.Runtime;

namespace xamarin_android
{
    [Activity(Label = "xamarin_android", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity , TextureView.ISurfaceTextureListener, ISurfaceHolderCallback
    {
        VideoCapture capture;
        ImageView imageView;

        private Camera camera;
        ISurfaceHolder surfaceHolder;
        SurfaceView surfaceView;
        TextureView textureView;
        MediaPlayer mediaPlayer;

        MCvScalar sk = new MCvScalar();
        //Point po = new Point(-1, -1);
        //Size sz = new Size(3, 3);
       // Mat s = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            textureView = (TextureView)FindViewById(Resource.Id.textureView1);
            textureView.SurfaceTextureListener = this;

            surfaceView = (SurfaceView)FindViewById(Resource.Id.surfaceView);
            //set to top layer
            surfaceView.SetZOrderOnTop(true);
            //set the background to transparent
            surfaceView.Holder.SetFormat(Format.Transparent);
            surfaceHolder = surfaceView.Holder;
            surfaceHolder.AddCallback(this);

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
                Mat g = new Mat();
                Mat n = new Mat();
                Mat k = new Mat();
                Mat hsv = new Mat();
                CvInvoke.CvtColor(m, g, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv); //Pakeičiu į hsv, nes "geresnė spalvų paletė jo"... Nu arba dar nemoku su spalvu jidaus žaist normaliai

                CvInvoke.InRange(g, new ScalarArray(new MCvScalar(0, 100, 100)), new ScalarArray(new MCvScalar(10, 255, 255)), g);  // išskiriam raudona spalva per tas tris eilutes

                //CvInvoke.Blur(g, g, sz, po); // išryškinam paveikslėlį
                //CvInvoke.Dilate(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);
               // CvInvoke.Erode(g, g, s, po, 1, Emgu.CV.CvEnum.BorderType.Default, sk);


                CircleF[] circles = CvInvoke.HoughCircles(g, Emgu.CV.CvEnum.HoughType.Gradient, 2, g.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj


                Image<Bgr, Byte> circleImage = m.ToImage<Bgr, byte>();
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine; //dvi eilut4s tekstui
                    //textBox1.Invoke(new Action(() => textBox1.AppendText(text))); // reikia kreiptis taip, nes is kito threado negalima toliau test visko
                    //circleImage.Draw(circle, new Bgr(Color.Red), 4); // apibrėžiam apvalius
                }

                //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // šitie du del dydžio lango dydžio, kad viskas matytuos
                //pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                //pictureBox1.Image = g.ToImage<Bgr, byte>().Bitmap;
                imageView.SetImageBitmap(circleImage.Bitmap);
                //imageView.Image = circleImage.Bitmap;
                Thread.Sleep((int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            }
            catch (Exception)
            {

            }
        }


        /** method that opens camera when the activity is launched
         */
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            // VIDEO FROM PROJECT
            Surface s = new Surface(surface);
            mediaPlayer = new MediaPlayer();
            Android.Net.Uri uri = Android.Net.Uri.Parse("android.resource://xamarin_android.xamarin_android//" + Resource.Raw.video);
            if(uri != null)
            {
                mediaPlayer.SetDataSource(BaseContext, uri);
                mediaPlayer.SetSurface(s);
                mediaPlayer.Prepare();
                mediaPlayer.Start();
            }


            // LIVE CAMERA FEED
            /*camera = Camera.Open();

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
            parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;*/
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
            
            
                Bitmap frame = textureView.Bitmap;
                Image<Bgr, byte> fr = new Image<Bgr, byte>(frame);
                Mat m = fr.Mat;

                Mat ball = Detailing.Detail(m);

                CircleF[] circles = CvInvoke.HoughCircles(ball, Emgu.CV.CvEnum.HoughType.Gradient, 2, ball.Rows / 4, 60, 30, 15, 40); // ieškom apvalių(kažkodėl ir ne tik) objektų jau toj išskirtoj raudonoj spalvoj

                Paint paint = new Paint();
                paint.Color = Android.Graphics.Color.Red;
                paint.SetStyle(Paint.Style.Stroke);
                paint.StrokeWidth = 5f;

                if (surfaceCreated)
                {
                    Canvas canvas = surfaceHolder.LockCanvas();
                    //clear the paint of last time
                    canvas.DrawColor(Android.Graphics.Color.Transparent, PorterDuff.Mode.Clear);
                    //draw a new one, set your ball's position to the rect here
                
                    surfaceHolder.UnlockCanvasAndPost(canvas);
                }
        
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine;
                    Console.Write(text);

                    //draw

                    Console.WriteLine(surfaceCreated);
                    if (surfaceCreated)
                    {
                        Canvas canvas = surfaceHolder.LockCanvas();
                        //clear the paint of last time
                     
                        //draw a new one, set your ball's position to the rect here
                        canvas.DrawCircle(circle.Center.X, circle.Center.Y, circle.Radius, paint);
                        surfaceHolder.UnlockCanvasAndPost(canvas);
                    }
                }
            
            
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
        }
        bool surfaceCreated = false;
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            surfaceCreated = true;
            Console.WriteLine("called");
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }
    }
}

