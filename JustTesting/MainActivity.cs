using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using SFile = System.IO.File;
using Thumbnails = Android.Provider.MediaStore.Images.Thumbnails;
using ANUri = Android.Net.Uri;

/* USEFUL STUFF:
 * https://www.youtube.com/watch?v=gOQnzTBR7wA - gridview + open single pic
 * 
 * TODO: Improve loading time, needs faster file-reading OR load and display file 1 by 1 async
 *      -some kind of async method of loading in an image, then passing it throu, then go to next image!
 * slidein-autohide toolbar thing for viewpager layout (back, options)
 * autohide toolbar in main view? or make it smaller!
 * play gifs in viewpager! what is needed?
 * video support?
 * sorting options: folder, date, size, etc(tags); ascending, descending
 * -refine thumbnails! save them to file? create a serialized collection for faster read, then analyze in background?
 * 
 * ERRORS:
 *      -position is 0 at the last item during rendering gridview!!!
 *      -out of memory really fast! -> create thumbs for gridview! -> out of memory later
 */


namespace JustTesting
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        RelativeLayout mainView;
        LinearLayout galleryView;
        GridView gallerygrid_1;

        public static List<ANUri> pictureList;
        public static File picDir;


        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            mainView = FindViewById<RelativeLayout>(Resource.Id.mainView);
            galleryView = FindViewById<LinearLayout>(Resource.Id.galleryView);

            SwitchView("gallery_1");

            //File picDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            //pictureList = new List<File>(GetPictures(picDir));
            //gallerygrid_1 = FindViewById<GridView>(Resource.Id.gridView1);
            //gallerygrid_1.SetColumnWidth(Resources.DisplayMetrics.WidthPixels / 3);
            //gallerygrid_1.Adapter = new ImageAdapter(this);
            //gallerygrid_1.ItemClick += Gallerygrid_1_ItemClick;

            
        }

        private List<ANUri> GetPictures(File storage)
        {
            List<ANUri> pictureList = new List<ANUri>();

            string writeperm = Manifest.Permission.WriteExternalStorage;
            if (CheckSelfPermission(writeperm) != Android.Content.PM.Permission.Granted)
            {
                string[] permissions =
                {
                    //Manifest.Permission.AccessCoarseLocation, //Location (GPS)
                    //Manifest.Permission.AccessFineLocation, //Location (GPS)
                    Manifest.Permission.WriteExternalStorage
                };
                RequestPermissions(permissions, 0);
                while (CheckSelfPermission(writeperm) != Android.Content.PM.Permission.Granted)
                {
                    System.Threading.Tasks.Task.Delay(250).Wait();
                }
            }
            
            File[] files = storage.ListFiles();
            
            if (files == null)
            {
                return null;
            }
            foreach (File file in files)
            {
                if (file.IsFile)
                {
                    if (IsFilePicture(file.Name)) //filter!
                    {
                        pictureList.Add(ANUri.FromFile(file));
                    }
                }
                else if (file.IsDirectory)
                {
                    //recursive!
                    pictureList.AddRange(GetPictures(file));
                }
            }
            return pictureList;
        }

        private bool IsFilePicture(string fileName)
        {
            if (fileName.EndsWith(".bmp"))
            {
                return true;
            }
            else if (fileName.EndsWith(".png"))
            {
                return true;
            }
            else if (fileName.EndsWith(".jpg"))
            {
                return true;
            }
            else if (fileName.EndsWith(".gif"))
            {
                return true;
            }
            else if (fileName.EndsWith(".jpeg"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Gallerygrid_1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(PictureActivity));
            nextActivity.PutExtra("picPosition", e.Position);
            StartActivity(nextActivity);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        private void SwitchView(string switchTo)
        {
            
            //make switchTo visible, all others invisible
            mainView.Visibility = ViewStates.Invisible;
            galleryView.Visibility = ViewStates.Invisible;
            switch (switchTo)
            {
                case "gallery_1":
                    galleryView.Visibility = ViewStates.Visible;
                    break;
                case "mainView":
                    mainView.Visibility = ViewStates.Visible;
                    break;
                default:
                    break;
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            
            if (id == Resource.Id.nav_camera)
            {
                // Handle the action of selecting a navigation option
                // Idea: have multiple layouts included in the main view, change visibility of these layouts
            }
            else if (id == Resource.Id.nav_gallery)
            {
                SwitchView("gallery_1");
            }
            else if (id == Resource.Id.nav_slideshow)
            {
                SwitchView("mainView");
            }
            else if (id == Resource.Id.nav_manage)
            {
                //TextView txtView = FindViewById<TextView>(Resource.Id.textView1);
                //string text = "";
                //text = pictureList.Count.ToString();

                //txtView.Text = text;
            }
            else if (id == Resource.Id.nav_share)
            {
                picDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                pictureList = new List<ANUri>(GetPictures(picDir));
                gallerygrid_1 = FindViewById<GridView>(Resource.Id.gridView1);
                gallerygrid_1.SetColumnWidth(Resources.DisplayMetrics.WidthPixels / 3);
                gallerygrid_1.Adapter = new ImageAdapter(this, ContentResolver);
                gallerygrid_1.ItemClick += Gallerygrid_1_ItemClick;
            }
            else if (id == Resource.Id.nav_send)
            {
                File picDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                List<ANUri> templist = new List<ANUri>(GetPictures(picDir));

                string uri = getThumbnailPath(Android.Provider.MediaStore.Images.Media.ExternalContentUri);
                //string uri = getThumbnailPath(Android.Net.Uri.Parse(templist[0].ToString()));
                ImageView imgview = FindViewById<ImageView>(Resource.Id.imageView_DEBUG);
                imgview.SetImageURI(Android.Net.Uri.Parse(uri));
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public String getThumbnailPath(Android.Net.Uri uri)
        {
            
            String[] projection = { MediaStore.Images.Media.InterfaceConsts.Id };
            String result = null;
            ICursor cursor = ContentResolver.Query(uri, projection, null, null, null, null);
            if (cursor == null)
            {

            }
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Id);

            cursor.MoveToFirst();
            long imageId = cursor.GetLong(column_index);
            cursor.Close();

            cursor = MediaStore.Images.Thumbnails.QueryMiniThumbnail(
                    ContentResolver, imageId,
                    ThumbnailKind.MiniKind,
                    null);
            if (cursor != null && cursor.Count > 0)
            {
                cursor.MoveToFirst();
                result = cursor.GetString(cursor.GetColumnIndexOrThrow(MediaStore.Images.Thumbnails.Data));
                cursor.Close();
            }
            return result;
        }
        

    }
}

