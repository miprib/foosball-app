﻿using System;
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
    class VideoFile : Video
    {
        String path;
        MediaPlayer mediaPlayer;

        public VideoFile(String path)
        {
            this.path = path;
        }

        public override void PlayVideo(Context context, SurfaceTexture surfaceTexture)
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
        }

        public override void StopVideo()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
            }
        }
    }
}