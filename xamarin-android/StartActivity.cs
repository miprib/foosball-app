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
        Button bHistory;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.a_start);

            MyProperties properties = MyProperties.getInstance();

            bPlayCamera = (Button)FindViewById(Resource.Id.bPlayCamera);
            bPlayFile = (Button)FindViewById(Resource.Id.bPlayFile);
            bHistory = (Button)FindViewById(Resource.Id.bHistory);
                  
            bPlayCamera.Click += delegate {
                InputTeamNames("live");
            };

            bPlayFile.Click += delegate {
                var videoIntent = new Intent();
                videoIntent.SetType("video/*");
                videoIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(videoIntent, "Select video"), 0);
            };

            bHistory.Click += delegate {
                if (ServerConnection.IsAddressAvailable())
                {
                    var intent = new Intent(this, typeof(HighscoreActivity));
                    StartActivity(intent);
                }
                else { Toast.MakeText(ApplicationContext, "No connection with service", ToastLength.Long).Show(); }
            };

            properties.gameList = ServerConnection.GetList();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                InputTeamNames("file", data.DataString);
            }
        }

        public void InputTeamNames(string type, string path = "")
        {
            //set alert for executing the task
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Input team names");
            var inputView = LayoutInflater.Inflate(Resource.Layout.d_team_names, null);
            var etTeamName1 = (EditText)inputView.FindViewById(Resource.Id.etTeamName1);
            var etTeamName2 = (EditText)inputView.FindViewById(Resource.Id.etTeamName2);
            alert.SetView(inputView);
            alert.SetPositiveButton("Submit", (senderAlert, args) => {
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("videoType", type);
                intent.PutExtra("path", path);
                intent.PutExtra("teamName1", etTeamName1.Text);
                intent.PutExtra("teamName2", etTeamName2.Text);
                StartActivity(intent);
                Finish();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                alert.Dispose();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}