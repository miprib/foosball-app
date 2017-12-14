using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace xamarin_android
{
    class VideoFile : IVideo
    {
        String path;
        MediaPlayer mediaPlayer;
        MainActivity mainActivity;

        public VideoFile(MainActivity mainActivity, String path)
        {
            this.mainActivity = mainActivity;
            this.path = path;
        }

        public bool IsPlaying()
        {
            return mediaPlayer.IsPlaying;
        }

        public void PlayVideo(Context context, SurfaceTexture surfaceTexture)
        {
            Surface s = new Surface(surfaceTexture);
            mediaPlayer = new MediaPlayer();
            //Android.Net.Uri uri = Android.Net.Uri.Parse("android.resource://xamarin_android.xamarin_android//" + Resource.Raw.video);
            Android.Net.Uri uri = Android.Net.Uri.Parse(path);
            if (uri != null)
            {
                mediaPlayer.SetDataSource(context, uri);
                mediaPlayer.SetSurface(s);
                mediaPlayer.Prepare();
                mediaPlayer.Start();
            }
            mediaPlayer.Completion += mainActivity.VideoEnd;
        }

        public void StopVideo()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
            }
        }

        public void PauseVideo()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Pause();
            }
        }

        public void ResumeVideo()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Start();
            }
        }
    }
}