using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;

namespace JustTesting
{
    class ViewPagerAdapter : PagerAdapter
    {

        Context context;
        LayoutInflater layoutInf;
        public ViewPagerAdapter(Context context)
        {
            this.context = context;
            
        }

        public Object GetItem(int position)
        {
            return MainActivity.pictureList[position];
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return MainActivity.pictureList.Count;
            }
        }

        public override Object InstantiateItem(ViewGroup container, int position)
        {
            layoutInf = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View vw = layoutInf.Inflate(Resource.Layout.picture_content, null);
            ImageView imgView = (ImageView)vw.FindViewById(Resource.Id.imageView1);
            imgView.SetImageURI(Android.Net.Uri.Parse(GetItem(position).ToString()));

            ViewPager vp = (ViewPager)container;
            vp.AddView(vw, 0);
            return vw;
        }

        public override void DestroyItem(ViewGroup container, int position, Object obj)
        {
            ViewPager vp = (ViewPager)container;
            View vw = (View)obj;
            vp.RemoveView(vw);
        }

    }

    class ViewPagerAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}