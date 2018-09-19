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

namespace JustTesting
{
    [Activity(Label = "PictureActivity")]
    class PictureActivity : Activity
    {
        ViewPager vp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.show_picture);
            int picPosition = Intent.GetIntExtra("picPosition", -1);

            vp = FindViewById<ViewPager>(Resource.Id.viewPager1);
            ViewPagerAdapter vpAdaper = new ViewPagerAdapter(this);
            vp.Adapter = vpAdaper;
            vp.SetCurrentItem(picPosition, true);
            
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

    }
}