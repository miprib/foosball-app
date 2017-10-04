using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace Foosball
{
    [Activity(Label = "Foosball", MainLauncher = true, Icon = "@drawable/foosball_launcher")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get the UI controls from the loaded layout:
            Button startButton = FindViewById<Button>(Resource.Id.StartButton);

            startButton.Click += (object sender, EventArgs e) =>
            {
                // TO-DO: add code for "Start"
            };

        }
    }
}

