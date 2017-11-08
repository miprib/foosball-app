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
using System.ServiceModel;
using FoosballWcfHost;

// For reference
// https://developer.xamarin.com/guides/cross-platform/application_fundamentals/web_services/walkthrough_working_with_WCF/

namespace Foosball
{
     [Activity(Label = "Highscores")]
     public class HighscoreActivity : Activity
     {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://192.168.0.103:9608/FoosballService.svc");

        private FoosballServiceClient _client;
        private Button _getHelloWorldDataButton;
        private TextView _getHelloWorldDataTextView;
        private Button _sayHelloWorldButton;
        private TextView _sayHelloWorldTextView;

        // Initializes the instance variables for our class and wires up some event handlers
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Highscores);

            InitializeFoosballServiceClient();

            // This button will invoke the GetHelloWorldData - the method that takes a C# object as a parameter.
            _getHelloWorldDataButton = FindViewById<Button>(Resource.Id.getHelloWorldDataButton);
            _getHelloWorldDataButton.Click += GetHelloWorldDataButtonOnClick;
            _getHelloWorldDataTextView = FindViewById<TextView>(Resource.Id.getHelloWorldDataTextView);

            // This button will invoke SayHelloWorld - this method takes a simple string as a parameter.
            _sayHelloWorldButton = FindViewById<Button>(Resource.Id.sayHelloWorldButton);
            _sayHelloWorldButton.Click += SayHelloWorldButtonOnClick;
            _sayHelloWorldTextView = FindViewById<TextView>(Resource.Id.sayHelloWorldTextView);
        }

        // Instantiates and initializes a FoosballService object
        private void InitializeFoosballServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            _client = new FoosballServiceClient(binding, EndPoint);
            _client.SayHelloToCompleted += ClientOnSayHelloToCompleted;
            _client.GetHelloDataCompleted += ClientOnGetHelloDataCompleted;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        // Event handlers for the two buttons in our activity
        private void GetHelloWorldDataButtonOnClick(object sender, EventArgs eventArgs)
        {
            FoosballData data = new FoosballData { Name = "<(^O^)>", SayHello = true };
            _getHelloWorldDataTextView.Text = "Waiting for WCF...";
            _client.GetHelloDataAsync(data);
        }

        private void SayHelloWorldButtonOnClick(object sender, EventArgs eventArgs)
        {
            _sayHelloWorldTextView.Text = "Waiting for WCF...";
            _client.SayHelloToAsync("(>^_^)>");
        }

        // Event handlers for the xxxCompleted events of the FoosballService proxy client
        private void ClientOnGetHelloDataCompleted(object sender, GetHelloDataCompletedEventArgs getHelloDataCompletedEventArgs)
        {
            string msg = null;

            if (getHelloDataCompletedEventArgs.Error != null)
            {
                msg = getHelloDataCompletedEventArgs.Error.Message;
            }
            else if (getHelloDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = getHelloDataCompletedEventArgs.Result.Name;
            }
            RunOnUiThread(() => _getHelloWorldDataTextView.Text = msg);
        }

        private void ClientOnSayHelloToCompleted(object sender, SayHelloToCompletedEventArgs sayHelloToCompletedEventArgs)
        {
            string msg = null;

            if (sayHelloToCompletedEventArgs.Error != null)
            {
                msg = sayHelloToCompletedEventArgs.Error.Message;
            }
            else if (sayHelloToCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = sayHelloToCompletedEventArgs.Result;
            }
            RunOnUiThread(() => _sayHelloWorldTextView.Text = msg);
        }


    }
     
}
    