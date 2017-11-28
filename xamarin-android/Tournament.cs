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
    class Tournament
    {
        public string PIN { get; set; }
        public List<TPlayer> players { get; set; }
        public bool ended { get; set; }
        
        public bool check (string p)
        {
            return this.PIN == p;
        }

        public List<string> GetNames()
        {
            List<string> a = new List<string>();
            foreach(TPlayer p in players)
            {
                a.Add(p.Name);
            }
            return a;
        }

    }
}