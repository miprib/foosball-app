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
        public static string url = "http://192.168.0.103:5000/api/matchdetailitems"; //change to current IP

        static public GameList GetList()
        {
            // Initiate Rest Client

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            // Set Data format
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute<GameList>(request);

            // Get Data
            return response.Data;
        }

        static public void PostGame(Game i)
        {
            // Initiate Rest Client
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            // Set Data format
            request.RequestFormat = DataFormat.Json;

            // Set Data
            request.AddBody(i);

            // Execute
            client.Execute(request);
        }

        static public void PutGame(Game i)
        {
            // Initiate Rest Client
            var client = new RestClient(url);
            var request = new RestRequest(Method.PUT);

            // Set Data format
            request.RequestFormat = DataFormat.Json;

            // Set Data
            request.AddBody(i);

            // Execute
            client.Execute(request);
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