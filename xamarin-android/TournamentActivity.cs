using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace xamarin_android
{
    [Activity(Label = "TournamentActivity")]
    public class TournamentActivity : Activity
    {
        Button bExisting;
        Button bNew;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.a_tournament);

            bExisting = (Button)FindViewById(Resource.Id.bExisting);
            bNew = (Button)FindViewById(Resource.Id.bNew);

            bExisting.Click += delegate
            {
                InputCode();
            };

            bNew.Click += delegate
            {
                Intent create = new Intent(this, typeof(TournamentActivity));
            };
        }

        private void InputCode()
        {
            AlertDialog.Builder Code = new AlertDialog.Builder(this);
            Code.SetTitle(Resource.String.code);
            View input = LayoutInflater.Inflate(Resource.Layout.d_PIN, null);
            EditText pin = (EditText)input.FindViewById(Resource.Id.PIN);
            Code.SetView(input);
            Code.SetPositiveButton("Enter", (senderAlert, args) =>
            {
                var intent = new Intent(this, typeof(TournamentActivity));
                intent.PutExtra("pin", pin.Text);
                StartActivity(intent);
                Finish();
            });

            Code.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                Code.Dispose();
            });

            Dialog dialog = Code.Create();
            dialog.Show();
        }
    }
}