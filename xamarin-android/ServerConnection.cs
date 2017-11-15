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
using System.Net;

namespace xamarin_android
{
    static public class ServerConnection
    {
        public static string url = "http://192.168.0.101:5000/api/matchdetailitems"; //change to current IP

        static public GameList GetList()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute<GameList>(request);

            return response.Data;
        }

        static public void PostGame(Game i)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(i);
            client.Execute(request);
        }

        static public void PutGame()
        {
            // todo
        }

        public static bool IsAddressAvailable()
        {
            try
            {
                System.Net.WebClient client = new WebClient();
                client.DownloadData(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}