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
using RestSharp;
using Android.Text.Method;
using System.Net;

namespace xamarin_android
{
    [Activity(Label = "Highscores")]
    public class HighscoreActivity : Activity
    {
        ListView listView;
        public class Item
        {
            public int id { get; set; }
            public DateTime date { get; set; }
            public string team1 { get; set; }
            public string team2 { get; set; }
            public int team1Score { get; set; }
            public int team2Score { get; set; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.a_highscore);

            //Initializing listview
            listView = (ListView) FindViewById(Resource.Id.ListView);

            // Get data from our Web Service
            GameList gameList = ServerConnection.GetList();
            MyProperties.getInstance().gameList = gameList;
            listView.Adapter = new HistoryListAdapter(this, gameList);      
        }
    }
}