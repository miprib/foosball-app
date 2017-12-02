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
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;

using xamarin_android.Recognition;

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

        ColorRange color;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            color = new ColorRange(240, 84, 43);

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
                // todo update game in server whenever goal happens
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "No connection with service", ToastLength.Long).Show();
            }
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

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            Bitmap f = textureView.GetBitmap(640, 360);

            f = Processing.ToHSV(f);

            f = Processing.Color(f, color);

            Coordinates a = Processing.FindBall(f);

            f.Dispose();

            /*            
            if (Recognition.IsScored()) { ServerConnection.PostGame(game); }*/
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

