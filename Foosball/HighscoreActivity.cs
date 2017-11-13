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

namespace Foosball
{
     [Activity(Label = "Highscores")]
     public class HighscoreActivity : Activity
     {
        public class Item
        {
            public string id { get; set; }
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

            Button getDataButton = FindViewById<Button>(Resource.Id.GetDataButton);

            getDataButton.Click += async (sender, e) =>
            {
                //string url = "http://192.168.0.102:5000/api/matchdetailitems"; change to local IP
                // http://10.3.7.168:5000/api/matchdetailitems - MIF

                var client = new RestClient("http://10.3.7.168:5000/api/matchdetailitems");
                var request = new RestRequest( Method.GET);

                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                var response = client.Execute<List<Item>>(request);

                //ListView historyList = FindViewById<ListView>(Resource.Id.HistoryList);
                TextView history = FindViewById<TextView>(Resource.Id.HistoryText);

                StringBuilder MyStringBuilder = new StringBuilder("");

                foreach (Item i in response.Data)
                {
                    //history.Text = i.id + "\n" + i.date + "\n" + i.team1 + " " + i.team1Score + " VS " + i.team2 + " " + i.team2Score;
                    MyStringBuilder.Append(i.id + "\n" + i.date + "\n" + i.team1 + " " + i.team1Score + " VS " + i.team2 + " " + i.team2Score + "\n\n");
                }

                history.Text = MyStringBuilder.ToString();

            };

        }

        /*public void GET(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute<List<Item>>(request);
        }*/
    }
}
     
    