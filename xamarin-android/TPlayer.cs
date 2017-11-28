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
    class TPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int place { get; set; }

        public Boolean Equals(string otter)
        {
            return this.Name.Equals(otter);
        }
    }
}