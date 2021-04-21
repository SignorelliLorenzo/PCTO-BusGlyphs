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

namespace GlyphsBus
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = false, Label = "Menu Cam")]
    public class CamActivity : AppCompatActivity
    {

        //Variabili per Menu
        private static bool menuopen;
        FloatingActionButton MBus;
        FloatingActionButton MHome;
        FloatingActionButton MMaps;
        FloatingActionButton MCamera;
        FloatingActionButton MPlus;
        View MenuContentCam;

        //Variabili per Camera
        Button CaptureButton;
        ImageView IViewHelp;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.CamActivity);

            //FindByID Camera
            CaptureButton = FindViewById<Button>(Resource.Id.BtnCapture);
            IViewHelp = FindViewById<ImageView>(Resource.Id.IViewCam);

            //Camera
            CaptureButton.Click += BtnCapture_Click;
            RequestPermissions(permissionGroup, 0);

            //FindByIdD Menu
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

            MCamera.Click += (o, e) =>
            {
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
                Intent nextActivity = new Intent(this, typeof(BusActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MenuContentCam.Click += (o, e) => { CloseFabMenu(); };

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

        private void BtnCapture_Click(object sender, System.EventArgs e)
        {
            TakePhoto();
        }

        public async void TakePhoto()
        {

            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "myimage.jpg",
                Directory = "testCamera"

            });

            if (file == null)
            {
                return;
            }

            //Convert file to byte array and set the resulting bitmap to imageview
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            IViewHelp.SetImageBitmap(bitmap);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}