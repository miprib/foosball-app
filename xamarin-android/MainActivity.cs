using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using System;

using Emgu.CV;
using Android.Graphics;
using Android.Content.PM;
using Android.Runtime;
using System.ComponentModel;
using RestSharp;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;

using xamarin_android.Recognition;
using Emgu.CV.Structure;

namespace xamarin_android
{
    [Activity(Label = "xamarin_android", ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener, ISurfaceHolderCallback
    {
        ISurfaceHolder surfaceHolder;
        SurfaceView surfaceView;
        TextureView textureView;
        IVideo video;
        Game game;

        TextView team1;
        TextView team2;
        TextView score;

        ColorRange color;

        bool IsConnection = false;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            color = new ColorRange(43, 84, 240);

            AssignBackgroundWorker();

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

            team1 = (TextView)FindViewById(Resource.Id.team1);
            team2 = (TextView)FindViewById(Resource.Id.team2);
            score = (TextView)FindViewById(Resource.Id.score);
            try
            {
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
                IsConnection = true;
                // todo update game in server whenever goal happens
            }
            catch
            {
                game = new Game
                {
                    // todo get unique id
                    id = 0,
                    date = DateTime.Now,
                    team1 = Intent.GetStringExtra("teamName1"),
                    team2 = Intent.GetStringExtra("teamName2"),
                    team1Score = 0,
                    team2Score = 0
                };
                IsConnection = false;
                Toast.MakeText(ApplicationContext, "No connection with service", ToastLength.Long).Show();
            }
            team1.Text = game.team1;
            team2.Text = game.team2;
            updateScore(0, 0);
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
        int xlatest = 350;
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            /*await Task.Run(() => {
                Bitmap f = textureView.GetBitmap(640, 360);

                Mat frame = Processing.ToHSV(f, color);
                f.Dispose();

                Coordinates coordinates = Processing.FindBall(frame);

                if (coordinates.Found())
                {
                    i = 0;
                    xlatest = coordinates.x;
                }
                else
                {
                    i++;
                    if (i == 6)
                    {
                        if (440 < xlatest)
                        {
                            game.team1Score++;
                            updateScore(game.team1Score, game.team2Score);
                        }

                        if (200 > xlatest)
                        {
                            game.team2Score++;
                            updateScore(game.team1Score, game.team2Score);
                        }
                    }
                }
            });*/
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
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

        private void updateScore(int team1, int team2)
        {
            score.Text = team1.ToString() + " - " + team2.ToString();
            if (IsConnection)
            {
                ServerConnection.PostGame(game);
            }
        }

        private BackgroundWorker worker;

        private void AssignBackgroundWorker()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap f = textureView.GetBitmap(640, 360);

            Mat frame = Processing.ToHSV(f, color);

            Coordinates coordinates = Processing.FindBall(frame);

            if (coordinates.Found())
            {
                i = 0;
                xlatest = coordinates.x;
            }
            else
            {
                i++;
                if (i == 6)
                {
                    if (440 < xlatest)
                    {
                        game.team1Score++;
                        updateScore(game.team1Score, game.team2Score);
                    }

                    if (200 > xlatest)
                    {
                        game.team2Score++;
                        updateScore(game.team1Score, game.team2Score);
                    }
                }
            }
        }
    }
}

