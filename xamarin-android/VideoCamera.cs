using System;
using Android.Content;
using Android.Graphics;
using Camera = Android.Hardware.Camera;

namespace xamarin_android
{
    class VideoCamera : IVideo
    {
        Camera camera;

        public void PlayVideo(Context context, SurfaceTexture surfaceTexture)
        {
            camera = Camera.Open();

            try
            {
                camera.SetPreviewTexture(surfaceTexture);
                camera.StartPreview();
            }
            catch (Exception)
            { }

            var metrics = context.Resources.DisplayMetrics;

            Camera.Parameters parameters = camera.GetParameters();
            parameters.SetPreviewSize((int)metrics.WidthPixels, (int)metrics.HeightPixels);
            parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
        }

        public void StopVideo()
        {
            if (camera != null)
            {
                camera.StopPreview();
                camera.Release();
                camera = null;
            }
        }

        public void PauseVideo()
        {
            if (camera != null)
            {
                camera.StopPreview();
            }
        }

        public void ResumeVideo()
        {
            if (camera != null)
            {
                camera.StartPreview();
            }
        }
    }
}