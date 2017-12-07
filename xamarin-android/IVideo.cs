using Android.Content;
using Android.Graphics;

namespace xamarin_android
{
    interface IVideo
    {
        void PlayVideo(Context context, SurfaceTexture surfaceTexture);
        void StopVideo();
        void ResumeVideo();
        void PauseVideo();
    }
}