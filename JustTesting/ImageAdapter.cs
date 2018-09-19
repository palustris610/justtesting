using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Java.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JustTesting
{
    class ImageAdapter : BaseAdapter
    {

        Context context;

        public ImageAdapter(Context context)
        {
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return MainActivity.pictureList[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View vw;
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            if (convertView == null)
            {
                vw = new View(context);
                vw = inflater.Inflate(Resource.Layout.grid_layout, null);
                ImageView imgView = vw.FindViewById<ImageView>(Resource.Id.imageView_forgrid);
                imgView.LayoutParameters = new LinearLayout.LayoutParams(300, 300); //fine tune OR create proper dynamic fitting style!!!!
                imgView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imgView.SetPadding(0,0,0,0);
                imgView.SetImageURI(Android.Net.Uri.Parse(GetItem(position).ToString()));
            }
            else
            {
                vw = (View)convertView;
            }
            
            return vw;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return MainActivity.pictureList.Count;
            }
        }

    }

    class ImageAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}