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
    class PlayersListAdapter : BaseAdapter
    {

        Activity context;
        List<TPlayer> list;

        public PlayersListAdapter(Activity context, List<TPlayer> list)
        {
            this.context = context;
            this.list = list;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return list[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_tournament, parent, false);

            view.FindViewById<TextView>(Resource.Id.t_player).Text = this.GetItemId(position).ToString();
            
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return 0;
            }
        }

    }

    class PlayersListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}