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
    [Activity(Label = "New_Tournament")]
    public class NewTournamentActivity : Activity
    {
        List<TPlayer> players;
        List<string> names;
        Button badd;
        Button bgo;
        EditText name;
        ListView list;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.a_new_tournament);

            list = (ListView)FindViewById(Resource.Id.t_list);

            names = new List<string>();

            list.Adapter = new ArrayAdapter(this, Resource.Layout.list_tournament, names);

            badd = (Button)FindViewById(Resource.Id.add);
            bgo = (Button)FindViewById(Resource.Id.tesk);
            name = (EditText)FindViewById(Resource.Id.name);

            players = new List<TPlayer>();
            TPlayer player = new TPlayer();
            int i = 0;

            badd.Click += delegate
            {
                i++;
                player.Id = i;
                player.Name = name.Text;
                name.Text = "";
                player.place = 0;
                names.Add(player.Name);
                players.Add(player);
                list.Adapter.Dispose();
                list.Adapter = new ArrayAdapter(this, Resource.Layout.list_tournament, names);
            };
            
            bgo.Click += delegate
            {
                //to do service connection
                //new window to display tournament
                string pin = RandomString(8);
                Tournament tournament = new Tournament()
                {
                    PIN = pin,
                    players = players,
                    ended = false
                };
            };

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}