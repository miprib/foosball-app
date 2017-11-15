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
    public class HistoryListAdapter : BaseAdapter<Game>
    {
        Activity context;
        GameList list;

        public HistoryListAdapter(Activity _context, GameList _list)
            : base()
        {
            this.context = _context;
            this.list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Game this[int index]
        {
            get { return list[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.list_row, parent, false);

            Game game = this[position];
            view.FindViewById<TextView>(Resource.Id.teamName1).Text = game.team1;
            view.FindViewById<TextView>(Resource.Id.teamName2).Text = game.team2;
            view.FindViewById<TextView>(Resource.Id.teamScore1).Text = game.team1Score.ToString();
            view.FindViewById<TextView>(Resource.Id.teamScore2).Text = game.team2Score.ToString();
            
            return view;
        }
    }
}