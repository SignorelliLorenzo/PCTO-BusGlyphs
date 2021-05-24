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
using static Android.Hardware.Camera;
using static Android.Views.ViewGroup;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace GlyphsBus
{
    [Activity(Theme = "@style/AppTheme", Icon ="@drawable/icon", MainLauncher = true, Label = "GlyphsBus")]
    public class MainActivity : AppCompatActivity
    {
        //Variabili per Menu
        private static bool menuopen;
        FloatingActionButton MBus;
        FloatingActionButton MHome;
        FloatingActionButton MMaps;
        FloatingActionButton MCamera;
        FloatingActionButton MPlus;
        View MenuContent;

        ImageView IViewMainHome;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //FindByID Home
            IViewMainHome = FindViewById<ImageView>(Resource.Id.IViewMainHome);
            IViewMainHome.SetImageResource(Resource.Drawable.logo);
            //FindByID Menu
            MBus = FindViewById<FloatingActionButton>(Resource.Id.fab_bus);
            MHome = FindViewById<FloatingActionButton>(Resource.Id.fab_home);
            MMaps = FindViewById<FloatingActionButton>(Resource.Id.fab_maps);
            MCamera = FindViewById<FloatingActionButton>(Resource.Id.fab_camera);
            MPlus = FindViewById<FloatingActionButton>(Resource.Id.fab_main);
            MenuContent = FindViewById<View>(Resource.Id.Menu);

            MCamera.Click += (o, e) => {
                Intent nextActivity = new Intent(this, typeof(CamActivity));
                StartActivity(nextActivity);
                CloseFabMenu();
            };

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

                Intent nextActivity = new Intent(this, typeof(CamActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MMaps.Click += (o, e) =>
            {
                var alertDialog = new Android.App.AlertDialog.Builder(this)
                      .SetTitle("Failure")
                      .SetMessage("Identify Glyph before opening this page.")
                      .SetPositiveButton("OK", (senderAlert, args) =>
                      {
                       

                      })
                      .Create();
                alertDialog.Show();
                CloseFabMenu(); //FATTO
                return;
            };

            MHome.Click += (o, e) =>
            {
                CloseFabMenu(); //FATTO
            };

            MBus.Click += (o, e) =>
            {
                var alertDialog = new Android.App.AlertDialog.Builder(this)
                       .SetTitle("Failure")
                       .SetMessage("Identify Glyph before opening this page.")
                       .SetPositiveButton("OK", (senderAlert, args) =>
                       {
                          

                       })
                       .Create();
                alertDialog.Show();
                CloseFabMenu(); //FATTO
                return;
            };

            MenuContent.Click += (o, e) => { CloseFabMenu(); };
        }
        protected override void OnStart()
        {
            base.OnStart();


        }
        private void CloseFabMenu()
        {
            menuopen = false;

            MPlus.Animate().Rotation(0f);
            MenuContent.Animate().Alpha(0f);

            MHome.Animate().TranslationY(0f).Rotation(90f);
            MBus.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContent, MBus, MMaps, MCamera, MHome));
            MMaps.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContent, MBus, MMaps, MCamera, MHome));
            MCamera.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabListener(MenuContent, MBus, MMaps, MCamera, MHome));
        }

        private void ShowFabMenu()
        {
            menuopen = true;
            MHome.Visibility = ViewStates.Visible;
            MBus.Visibility = Android.Views.ViewStates.Visible;
            MMaps.Visibility = Android.Views.ViewStates.Visible;
            MCamera.Visibility = Android.Views.ViewStates.Visible;
            MenuContent.Visibility = Android.Views.ViewStates.Visible;

            MPlus.Animate().Rotation(135f);
            MenuContent.Animate().Alpha(1f);

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
