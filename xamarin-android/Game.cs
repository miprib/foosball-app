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
    public class Game : IEquatable<Game>
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public int team1Score { get; set; }
        public int team2Score { get; set; }

        public bool Equals(Game other)
        {
            return this.id == other.id;
        }
    }
}