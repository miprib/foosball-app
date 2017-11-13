using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using System;

using Emgu.CV;
using Emgu.CV.Structure;
using Android.Graphics;
using Android.Content.PM;
using Android.Runtime;

namespace xamarin_android
{
    [Activity(Label = "xamarin_android", ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener, ISurfaceHolderCallback
    {
        ISurfaceHolder surfaceHolder;
        SurfaceView surfaceView;
        TextureView textureView;
        Video video;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

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

        /** method that opens camera when the activity is launched
         */
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            if (Intent.GetStringExtra("videoType") == "file")
            {
                video = new VideoFile(Intent.GetStringExtra("path"));
            }
            else
            {
                video = new VideoCamera();
            }
            video.PlayVideo(BaseContext, surface);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            video.StopVideo();
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        /** method that gets called with every frame. retrieve frame with 
         *  Bitmap frameBitmap = textureView.Bitmap;
         */
        int i = 0;
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            Console.WriteLine(i);
            i++;
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
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }
    }
}

