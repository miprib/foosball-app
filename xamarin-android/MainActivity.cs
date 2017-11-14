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
            SetContentView(Resource.Layout.a_main);

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
        Boolean n = true; // is it is first frame
        int xlatest = 0; //Latest ball cordinates in x axis
        int ff = 0; //frames counted since last recognition of ball
        int left = 0; //left team result
        int right = 0; //right team result
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            Console.WriteLine(i);
            i++;
            Bitmap frame = textureView.Bitmap;
            Mat m = new Mat(frame);

            if (n)
            {
                xlatest = m.Size.Width / 2;
                n = !n;
            }// if first frame it chooses middle of frame, in case it wont find ball

            Mat ball = Recognition.findColor(m);

            ball = Recognition.Detailing(ball);

            CircleF[] circles = Recognition.FindCircle(ball);
            /*
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
            */
            ff++; // it counts how many frames since last recognition
            foreach (CircleF circle in circles)
            {
                string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine;
                xlatest = (int)circle.Center.X; //if ball found it updates it's coordinates
                ff = 0;// ball found, so it will be last recognition
                /*
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
                */
            }
            if (ff == 15) // 15 frames gone since last recognition, so we count goal if ball was near gates
            {
                if (m.Size.Width - m.Size.Width / 4 < xlatest)
                {
                    right++;
                }

                if (m.Size.Width - m.Size.Width / 4 * 3 > xlatest)
                {
                    left++;
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

