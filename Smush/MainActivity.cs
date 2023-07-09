using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Media;
using Android.Widget;
using Android.Content;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smush
{   //Property of Kuziwakwashe Stephen Mashingaidze
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Used to be for a send feedback message button named fab
            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

            //Locate Background
            RelativeLayout bg = FindViewById<RelativeLayout>(Resource.Id.mainlayout);

            //READ SETTINGS FILE
            int settingcase = ReadSmush();
            int smushsound = 1;
            int smushdark = 0;

            if (settingcase == 0)
            {
                smushsound = 0;
                smushdark = 0;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }
            else if (settingcase == 1)
            {
                smushsound = 0;
                smushdark = 1;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }
            else if (settingcase == 2)
            {
                smushsound = 1;
                smushdark = 0;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }
            else if (settingcase == 3)
            {
                smushsound = 1;
                smushdark = 1;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }


            int counter = 0;

            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += async delegate
            {
                
                counter++;
                if (counter==1)
                {
                    if (smushsound == 1)
                    {
                        MediaPlayer splatplayer;
                        splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);
                        splatplayer.Start();
                        await Task.Delay(500);
                        splatplayer.Release();
                    }
                    
                    var intent = new Intent(this, typeof(SmushMeat));
                    StartActivity(intent);
                    this.Finish();
                }
                

            };

        }



        //READ FROM THE SETTINGS FILE
        public int ReadSmush()
        {
            int currline = 0;
            int settingcase = 2;

            var smushsettings = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushsettings.txt");
            if (smushsettings == null || !File.Exists(smushsettings))
            {
                settingcase = 2;
            }
            else
            {
                int smushsound = 1;
                int smushdark = 0;
                using (var reader = new StreamReader(smushsettings, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        currline++;
                        if (int.TryParse(line, out var newsetting))
                        {
                            if (currline == 1)
                            {
                                smushsound = newsetting;
                            }
                            else if (currline == 2)
                            {
                                smushdark = newsetting;
                            }

                        }
                    }
                }

                //Manually define the different cases that might exist
                if (smushsound == 0)
                {
                    if (smushdark == 0) //Setting case 0. No sound, BG white
                    {
                        settingcase = 0;
                    }
                    else if (smushdark == 1) //Setting case 1. No sound, BG black
                    {
                        settingcase = 1;
                    }
                }
                else if (smushsound == 1)
                {
                    if (smushdark == 0) //Setting case 2. Sound on, BG white
                    {
                        settingcase = 2;
                    }
                    else if (smushdark == 1) //Setting case 3. Sound on, BG black
                    {
                        settingcase = 3;
                    }
                }
            }



            return (settingcase);

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_exit)
            {
                this.FinishAndRemoveTask();
                return true;
            }
            

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        
    
        
	}
}
