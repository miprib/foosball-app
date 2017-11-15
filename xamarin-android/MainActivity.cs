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
using System.ComponentModel;
using RestSharp;

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
        Game game;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Work on background
            worker = new BackgroundWorker();
            worker.DoWork += bgwrk;
            worker.RunWorkerCompleted += bgwrkcmp;

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
            
            game = new Game
            {
                // todo get unique id
                id = MyProperties.getInstance().gameList.GetUniqueId(),
                date = DateTime.Now,
                team1 = Intent.GetStringExtra("teamName1"),
                team2 = Intent.GetStringExtra("teamName2"),
                team1Score = 0,
                team2Score = 0
            };
            ServerConnection.PostGame(game);
            // todo update game in server whenever goal happens
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
        private BackgroundWorker worker;

        private void bgwrk(object sender, DoWorkEventArgs e)
        {
            e.Result = recognize();
        }

        private void bgwrkcmp(object sender, RunWorkerCompletedEventArgs e)
        {
            ball = (Mat)e.Result;

        }

        private Mat recognize()
        {
            Bitmap frame = textureView.Bitmap;
            Mat m = new Mat(frame);

            m = Recognition.findColor(m);

            m = Recognition.Detailing(m);

            return m;
        }
        Mat ball = new Mat();
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            try
            {
                Console.WriteLine(i);
                i++;

                worker.RunWorkerAsync();

                if (n)
                {
                    xlatest = ball.Size.Width / 2;
                    n = !n;
                }// if first frame it chooses middle of frame, in case it wont find ball

                CircleF[] circles = Recognition.FindCircle(ball);
                ff++; // it counts how many frames since last recognition
                foreach (CircleF circle in circles)
                {
                    string text = "ball position = x " + circle.Center.X.ToString() + ", y " + circle.Center.Y.ToString() + System.Environment.NewLine;
                    xlatest = (int)circle.Center.X; //if ball found it updates it's coordinates
                    ff = 0;// ball found, so it will be last recognition
                }
                if (ff == 5) // 15 frames gone since last recognition, so we count goal if ball was near gates
                {
                    if (ball.Size.Width - ball.Size.Width / 4 < xlatest)
                    {
                        right++;
                    }

                    if (ball.Size.Width - ball.Size.Width / 4 * 3 > xlatest)
                    {
                        left++;
                    }
                }
            }
            catch{
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

