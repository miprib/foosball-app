using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Media;

namespace xamarin_android
{
    class VideoFile : Video
    {
        MediaPlayer mediaPlayer;
        public override void PlayVideo(Context context, SurfaceTexture surfaceTexture)
        {
            Surface s = new Surface(surfaceTexture);
            mediaPlayer = new MediaPlayer();
            Android.Net.Uri uri = Android.Net.Uri.Parse("android.resource://xamarin_android.xamarin_android//" + Resource.Raw.video);
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