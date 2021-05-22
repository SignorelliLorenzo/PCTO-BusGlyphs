﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Animation;
using Android.Support.Design.Widget;
using Android.Runtime;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ClientLibrary;
using Newtonsoft.Json;
using Android.Graphics;

namespace GlyphsBus
{

    [Activity(Theme = "@style/AppTheme", MainLauncher = false, Label = "Menu Bus")]
    public class BusActivity : AppCompatActivity
    {
        //Variabili per Menu
        private static bool menuopen;
        FloatingActionButton MBus;
        FloatingActionButton MHome;
        FloatingActionButton MMaps;
        FloatingActionButton MCamera;
        FloatingActionButton MPlus;
        View MenuContent;

        //Variabile per bus
        TextView HelpTextView; // Textview.Text=
        Spinner spinner;
        //INDIRIZZI
        static string indirizzo2 = ":8181";
        static string indirizzo3 = ":8282";
        static string indirizzo4 = ":8383";
        //---------------------------
        //CLIENT
        Client_Percorso_2 Client2;
        Client_Bus_3 Client3;
        Client_Immagine_4 Client4;
        //----------------------------
        private System.Timers.Timer _timer2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.BusActivity);
            ///Test
        }

        protected override void OnStart()
        {
            base.OnStart();

            //FindByID Menu
            MBus = FindViewById<FloatingActionButton>(Resource.Id.fab_bus);
            MHome = FindViewById<FloatingActionButton>(Resource.Id.fab_home);
            MMaps = FindViewById<FloatingActionButton>(Resource.Id.fab_maps);
            MCamera = FindViewById<FloatingActionButton>(Resource.Id.fab_camera);
            MPlus = FindViewById<FloatingActionButton>(Resource.Id.fab_main);
            MenuContent = FindViewById<View>(Resource.Id.MenuBus);

            //FindByID
            spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            HelpTextView = FindViewById<TextView>(Resource.Id.HelpTextView);

            //Code Spinner
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new string[1]);
            //
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            spinner.ItemSelected += (s, e) =>
            {
                if(!CamActivity.Nomifermate.ContainsValue(spinner.SelectedItem.ToString()))
                {
                    return;
                }
                IndexChanged(CamActivity.Nomifermate.FirstOrDefault(x=>x.Value == spinner.SelectedItem.ToString()).Key);
                
            };


            //Menu
            MPlus.Click += (o, e) =>
            {
                if (!menuopen)
                    ShowFabMenu();
                else
                    CloseFabMenu();
            };

            MCamera.Click += (o, e) => {
                Intent nextActivity = new Intent(this, typeof(CamActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MMaps.Click += (o, e) =>
            {
                Intent nextactivity = new Intent(this, typeof(MapActivity));
                StartActivity(nextactivity);
                CloseFabMenu(); //FATTO
            };

            MHome.Click += (o, e) => {
                Intent nextActivity = new Intent(this, typeof(MainActivity));
                StartActivity(nextActivity);
                CloseFabMenu(); //FATTO
            };

            MBus.Click += (o, e) =>
            {
                CloseFabMenu(); //FATTO
            };

            MenuContent.Click += (o, e) => { CloseFabMenu(); };

        }
        int whichone = 2;

        public object Client1 { get; private set; }

        private void OnTimedEvent2(object sender, ElapsedEventArgs e)
        {
            if (Client2.response || whichone == 2)
            {
                Client3 = new Client_Bus_3(indirizzo3, Client2.json);
                whichone++;
            }
            else if (Client3.response || whichone == 3)
            {
                Client4 = new Client_Immagine_4(indirizzo4, Client3.CodiceBus);
                whichone++;
            }
            else if (Client4.newImage || whichone == 4)
            {
                var bitmap=BitmapFactory.DecodeByteArray(Client4.immagine, 0, Client4.immagine.Length);
                MapActivity.IViewHelp.SetImageBitmap(bitmap);        
            }

        }
        private void IndexChanged(int destinazione)
        {
            _timer2 = new System.Timers.Timer();
            _timer2.Elapsed += OnTimedEvent2;
            _timer2.Interval = 1000;
            _timer2.Enabled = true;
            mexdestinazione messaggio = new mexdestinazione(destinazione, JsonConvert.DeserializeObject<mex>(CamActivity.Client1.json));
            Client2 = new Client_Percorso_2(indirizzo2, messaggio);
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