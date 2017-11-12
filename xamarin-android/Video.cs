using Android.Content;
using Android.Graphics;

namespace xamarin_android
{
    abstract class Video : IVideo
    {
        abstract public void PlayVideo(Context context, SurfaceTexture surfaceTexture);
        abstract public void StopVideo();
    }
}