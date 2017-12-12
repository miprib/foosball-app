using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.IO;

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
            Button highscoreButton = FindViewById<Button>(Resource.Id.HighscoreButton);
            Button tournamentButton = FindViewById<Button>(Resource.Id.TournamentButton);

            startButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(StartButtonActivity));
                StartActivity(intent);
            };

            highscoreButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(HighscoreActivity));
                StartActivity(intent);
            };

            tournamentButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(TournamentActivity));
                StartActivity(intent);
            };

        }
    }
}

