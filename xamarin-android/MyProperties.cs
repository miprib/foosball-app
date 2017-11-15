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
    public class MyProperties
    {
        private static MyProperties mInstance = null;
        
        protected MyProperties() { }

        public static MyProperties getInstance()
        {
            if (null == mInstance)
            {
                mInstance = new MyProperties();
            }
            return mInstance;
        }

        public GameList gameList { get; set; }
    }
}