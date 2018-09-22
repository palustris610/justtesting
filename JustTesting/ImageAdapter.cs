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
using Android.Provider;
using Android.Database;
using Android.Graphics;
using Thumbnails = Android.Provider.MediaStore.Images.Thumbnails;
using ANUri = Android.Net.Uri;

namespace JustTesting
{
    class ImageAdapter : BaseAdapter
    {

        Context context;
        ContentResolver ContentResolver;

        public ImageAdapter(Context context, ContentResolver resolver)
        {
            this.context = context;
            ContentResolver = resolver;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return getThumbnailPath(MediaStore.Images.Media.ExternalContentUri, position);
            return getThumbnailBitmap(ANUri.FromFile(MainActivity.picDir), position);
            //return MainActivity.pictureList[position];
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
                //imgView.SetImageURI(Android.Net.Uri.Parse(GetItem(position).ToString()));
                imgView.SetImageBitmap((Bitmap)GetItem(position));
            }
            else
            {
                vw = (View)convertView;
            }
            
            return vw;
        }

        //public String getThumbnailPath(Android.Net.Uri uri, int position)
        //{

        //    string[] projection = { MediaStore.Images.Media.InterfaceConsts.Id, MediaStore.Images.Media.InterfaceConsts.Data };
        //    string result = null;
        //    ICursor cursor = ContentResolver.Query(uri, projection, null, null, null, null);
        //    int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Id);

        //    //cursor.MoveToFirst();
        //    cursor.MoveToPosition(position);
        //    long imageId = cursor.GetLong(column_index);
        //    cursor.Close();

        //    cursor = MediaStore.Images.Thumbnails.QueryMiniThumbnail(
        //            ContentResolver, imageId,
        //            ThumbnailKind.MiniKind,
        //            null);
        //    if (cursor != null && cursor.Count > 0)
        //    {
        //        //cursor.MoveToFirst();
        //        cursor.MoveToPosition(position);

        //        result = cursor.GetString(cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data));
        //        cursor.Close();
        //    }
        //    return result;
        //}

        public Bitmap getThumbnailBitmap(Android.Net.Uri uri, int position)
        {
            
            string[] projection = { MediaStore.Images.Media.InterfaceConsts.Id, MediaStore.Images.Media.InterfaceConsts.Data };
            Bitmap result = null;
            ICursor cursor2 = ContentResolver.Query(uri, projection, null, null, null, null);
            ICursor cursor = ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, projection, null, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Id);

            //cursor.MoveToFirst();
            cursor.MoveToPosition(position);
            long imageId = cursor.GetLong(column_index);
            cursor.Close();
            //TEST android.media.thumbnailutils!
            result = Thumbnails.GetThumbnail(ContentResolver, imageId, ThumbnailKind.MiniKind, null);
            return result;
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