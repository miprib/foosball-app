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
    public class GameList : List<Game>
    {
        public int GetUniqueId()
        {
            int i = 0;
            foreach(Game game in this)
            {
                if(i != game.id)
                {
                    break;
                }
                i++;
            }
            return i;
        }
    }
}