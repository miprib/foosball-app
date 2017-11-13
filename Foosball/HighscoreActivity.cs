using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using RestSharp.Deserializers;
using System.Collections.ObjectModel;
using Android.Text.Method;

namespace Foosball
{
     [Activity(Label = "Highscores")]
     public class HighscoreActivity : Activity
     {
        public static string url = "http://192.168.0.101:5000/api/matchdetailitems"; //change to current IP
        public static String path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "All_games.txt");

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
            SetContentView(Resource.Layout.History);

            // Get data from our Web Service
            GET(url);

            Button doSomethingButton = FindViewById<Button>(Resource.Id.DoSomethingButton);

            // Data for POST
            Item i = new Item
            {
                id = 666,
                date = DateTime.Now,
                team1 = "PSI",
                team2 = "Gyvenimas",
                team1Score = 666,
                team2Score = -100
            };

            doSomethingButton.Click += async (sender, e) =>
            {
                POST(url, i);
            };
        }

        public void GET(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute<List<Item>>(request);

            TextView history = FindViewById<TextView>(Resource.Id.HistoryText);
            history.MovementMethod = new ScrollingMovementMethod();

            foreach (Item i in response.Data)
            {
                history.Append("\tGame: " + i.id + "\n" + 
                               "\tDate: " + i.date + "\n" +
                               "\t" + i.team1 + "(" + i.team1Score + ") VS " + i.team2 + " (" + i.team2Score + ")\n\n");
            }
        }

        public void POST(string url, Item i)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(i);
            client.Execute(request);
        }
    }
}
     
    