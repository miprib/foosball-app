using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;

namespace xamarin_android
{
    [Activity(Label = "StartActivity", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class StartActivity : Activity
    {
        Button bPlayFile;
        Button bPlayCamera;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.a_start);

            bPlayCamera = (Button)FindViewById(Resource.Id.bPlayCamera);
            bPlayFile = (Button)FindViewById(Resource.Id.bPlayFile);

            bPlayCamera.Click += delegate {
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("videoType", "live");
                StartActivity(intent);
                Finish();
            };
            bPlayFile.Click += delegate {
                var videoIntent = new Intent();
                videoIntent.SetType("video/*");
                videoIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(videoIntent, "Select video"), 0);
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("videoType", "file");
                intent.PutExtra("path", data.DataString);
                StartActivity(intent);
                Finish();
            }
        }
    }
}