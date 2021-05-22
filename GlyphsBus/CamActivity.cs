using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Animation;
using Android.Support.Design.Widget;
using Android.Views;
using System;
using Android;
using Plugin.Media;
using Android.Graphics;
using Android.Content;
using System.IO;
using Android.Support.V4.Content;
using Android.Content.PM;
using System.Threading;
using System.Timers;
using ClientLibrary;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
namespace GlyphsBus
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = false, Label = "Menu Cam")]
    public class CamActivity : Activity, TextureView.ISurfaceTextureListener
    {

        //Variabili per Menu
        private static bool menuopen;
        FloatingActionButton MBus;
        FloatingActionButton MHome;
        FloatingActionButton MMaps;
        FloatingActionButton MCamera;
        FloatingActionButton MPlus;
        View MenuContentCam;

        //RelativeLayout layoutCam;
        //INDIRIZZO
        static string indirizzo = ":8080";
        //----------
        //Globals
        Android.Hardware.Camera _camera;
        TextureView _textureView;
        private System.Timers.Timer _timer1;
        
        byte[] imagebyte = default;
        Bitmap Frame = default;
        public static Client_Glifo_1 Client1 = new Client_Glifo_1(indirizzo);

        public static Dictionary<int, string> Nomifermate = new Dictionary<int, string> {
            { 3, "Milano"},
            {2,"Telgate"},
            {6,"Bonate"},
            { 7,"Como"},
            { 5,"Roma"},
            {0,"Bergamo"}
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.CamActivity);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //layoutCam = FindViewById<RelativeLayout>(Resource.Id.layoutcam);

            //Code Camera
            _textureView = FindViewById<TextureView>(Resource.Id.textureView2);
            _textureView.SurfaceTextureListener = this;
            _timer1 = new System.Timers.Timer();
            _timer1.Elapsed += OnTimedEvent1;
            _timer1.Interval = 4000;
            _timer1.Enabled = true;

            // Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //layoutCam.AddView(_textureView);

            //FindByID Menu
            MBus = FindViewById<FloatingActionButton>(Resource.Id.fab_bus);
            MHome = FindViewById<FloatingActionButton>(Resource.Id.fab_home);
            MMaps = FindViewById<FloatingActionButton>(Resource.Id.fab_maps);
            MCamera = FindViewById<FloatingActionButton>(Resource.Id.fab_camera);
            MPlus = FindViewById<FloatingActionButton>(Resource.Id.fab_main);
            MenuContentCam = FindViewById<View>(Resource.Id.MenuCam);

            //Menu
            MPlus.Click += (o, e) =>
            {
                if (!menuopen)
                    ShowFabMenu();
                else
                    CloseFabMenu();
            };

            MCamera.Click += (o, e) => {
                CloseFabMenu(); //FATTO
            };

            MMaps.Click += (o, e) =>
            {
                Intent nextActivity = new Intent(this, typeof(MapActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MHome.Click += (o, e) => {
                Intent nextActivity = new Intent(this, typeof(MainActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MBus.Click += (o, e) =>
            {
                Intent nextactivity = new Intent(this, typeof(BusActivity));
                StartActivity(nextactivity);
                CloseFabMenu(); //FATTO
            };

            MenuContentCam.Click += (o, e) => { CloseFabMenu(); };

        }

        //-------------------------------------------CLIENT-----------------------------------------------//
        private void OnTimedEvent1(object sender, ElapsedEventArgs e)
        {
            if (!Client1.response)
            {
                imagebyte = SaveBitmap(Frame);
                Client1.immage = imagebyte;
            }
            else 
            {
               

                mex risposta = JsonConvert.DeserializeObject<mex>(Client1.json);
                Dictionary<int, string> FermateDisponibili = new Dictionary<int, string>();
                foreach (var item in risposta.Percorsi)
                {
                    foreach (var fermata in item.elefermateandata)
                    {
                        FermateDisponibili[fermata] = Nomifermate[fermata];
                    }
                    foreach (var fermata in item.elefermateritorno)
                    {
                        FermateDisponibili[fermata] = Nomifermate[fermata];
                    }
                }
                Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);
                var Comboitems = FermateDisponibili.Values.ToList().ToArray();
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Comboitems);
                //
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;

                _timer1.Stop();
            }
        }
        
        
        
        //-------------------------------------------------------------------------------------------------//

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            //----//LOCAL SAVE ON PHONE//----//
            //_camera = Camera.Open();
            //var previewSize = _camera.GetParameters().PreviewSize;
            //_textureView.LayoutParameters = new FrameLayout.LayoutParams(w, h);
            //try
            //{
            //    _camera.SetPreviewTexture(surface);
            //    _camera.StartPreview();

            //}
            //catch (Java.IO.IOException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            _camera = Android.Hardware.Camera.Open();
            //var previewSize = _camera.GetParameters().PreviewSize;
            //_textureView.LayoutParameters =
            //    new FrameLayout.LayoutParams(previewSize.Width,
            //        previewSize.Height, GravityFlags.Center);

            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // this is the sort of thing TextureView enables
            _textureView.Rotation = 90.0f;
            _textureView.Alpha = 1.0f;

        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            _camera.StopPreview();
            _camera.Release();

            return true;
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
        }


        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {
            Frame = _textureView.Bitmap;
        }
        public byte[] SaveBitmap(Bitmap bitmap)
        {
            //var sdCardPath = "/storage/emulated/0/Android/data/com.companyname.appxam/files/";


            //var filePath = System.IO.Path.Combine(sdCardPath, "yourImageName.png");
            //Console.WriteLine(filePath);
            //var stream = new FileStream(filePath, FileMode.Create);
            //bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            //stream.Close();
            byte[] temp;

            using (var bytestream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, bytestream);
                temp = bytestream.ToArray();
            }
            return temp;

        }
        private void CloseFabMenu()
        {
            menuopen = false;

            MPlus.Animate().Rotation(0f);
            MenuContentCam.Animate().Alpha(0f);

            MHome.Animate().TranslationY(0f).Rotation(90f);
            MBus.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContentCam, MBus, MMaps, MCamera, MHome));
            MMaps.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContentCam, MBus, MMaps, MCamera, MHome));
            MCamera.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContentCam, MBus, MMaps, MCamera, MHome));
        }

        private void ShowFabMenu()
        {
            menuopen = true;
            MHome.Visibility = ViewStates.Visible;
            MBus.Visibility = Android.Views.ViewStates.Visible;
            MMaps.Visibility = Android.Views.ViewStates.Visible;
            MCamera.Visibility = Android.Views.ViewStates.Visible;
            MenuContentCam.Visibility = Android.Views.ViewStates.Visible;


            MPlus.Animate().Rotation(135f);
            MenuContentCam.Animate().Alpha(1f);

            MHome.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_190)).Rotation(0f);
            MBus.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_145)).Rotation(0f);
            MMaps.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_100)).Rotation(0f);
            MCamera.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_55)).Rotation(0f);


        }

        private class FabListener : Java.Lang.Object, Animator.IAnimatorListener
        {

            View[] viewsToHide;

            public FabListener(params View[] viewsToHide)
            {
                this.viewsToHide = viewsToHide;
            }

            public void OnAnimationCancel(Animator animation)
            {
            }

            public void OnAnimationEnd(Animator animation)
            {
                if (!menuopen)
                    foreach (var view in viewsToHide)
                        view.Visibility = ViewStates.Gone;
            }

            public void OnAnimationRepeat(Animator animation)
            {
            }

            public void OnAnimationStart(Animator animation)
            {
            }
        }

    }
}
