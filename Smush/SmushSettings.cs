using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Media;
using System.IO;
using System.Threading.Tasks;
//using Android.Gms.Ads;

namespace Smush
{   //Property of Kuziwakwashe Stephen Mashingaidze
    [Activity(Label = "@string/content_fifth_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SmushSettings : AppCompatActivity
    {
        private Toast toast = null;
        //protected AdView myadview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_fifth);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar); There is already a toolbar, from the main activity layout


            //INITIALIZE ADS
            //MobileAds.Initialize(this); //Initialize Mobile Ads SDK. Needs to be done at the beginning
            //myadview = FindViewById<AdView>(Resource.Id.adView1);
            //var adrequest = new AdRequest.Builder().Build();
            //myadview.LoadAd(adrequest);

            //Locate Background
            RelativeLayout bg = FindViewById<RelativeLayout>(Resource.Id.settingslayout);

            //Initialize Toast
            toast = Toast.MakeText(this, "", ToastLength.Short);

            //Locate check boxes
            CheckBox checkbox1 = FindViewById<CheckBox>(Resource.Id.checkBox1);
            CheckBox checkbox2 = FindViewById<CheckBox>(Resource.Id.checkBox2);
            

            //Initialize save/read from settings file variables
            int settingcase = ReadSmush();
            int smushsound=1;
            int smushdark=0;
            
            if (settingcase == 0)
            {
                smushsound = 0;
                smushdark = 0;
                checkbox1.Checked = false;
                checkbox2.Checked = false;
                bg.SetBackgroundColor(Android.Graphics.Color.White);
            }
            else if (settingcase == 1)
            {
                smushsound = 0;
                smushdark = 1;
                checkbox1.Checked = false;
                checkbox2.Checked = true;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }
            else if (settingcase ==2)
            {
                smushsound = 1;
                smushdark = 0;
                checkbox1.Checked = true;
                checkbox2.Checked = false;
                bg.SetBackgroundColor(Android.Graphics.Color.White);
            }
            else if (settingcase ==3)
            {
                smushsound = 1;
                smushdark = 1;
                checkbox1.Checked = true;
                checkbox2.Checked = true;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }



            //Handle sound check box
            checkbox1.Click += (o, e) =>
            {
                if (checkbox1.Checked)
                {
                    smushsound = 1;
                }
                else
                {
                    smushsound = 0;
                }
            };

            //Handle dark mode check box
            checkbox2.Click += (o, e) =>
            {
                if (checkbox2.Checked)
                {
                    smushdark = 1;
                    bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
                }
                else
                {
                    smushdark = 0;
                    bg.SetBackgroundColor(Android.Graphics.Color.White);
                }
            };



            //When the SAVE button is pressed
            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += async delegate
            {
                if (smushsound ==1)
                {
                    MediaPlayer splatplayer;
                    splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);                   
                    splatplayer.Start();
                    await Task.Delay(500);
                    splatplayer.Release();
                }


                //Save the Settings to a settings file, overwriting the existing settings
                await SaveSmush(smushsound, smushdark);
                toast.SetText("Save successful.");
                toast.Show();
                //Toast.MakeText(this, "Save successful.", ToastLength.Short).Show();

            };


        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_fifth, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_exit2)
            {
                this.FinishAndRemoveTask();
                return true;
            }
            else if (id == Resource.Id.action_pass_gen)
            {
                var intent = new Intent(this, typeof(SmushMeat));
                StartActivity(intent);
                this.Finish();
                return true;
            }
            else if (id == Resource.Id.action_pass_bank)
            {
                var intent = new Intent(this, typeof(PasswordBank));
                StartActivity(intent);
                this.Finish();
                return true;
            }
            else if (id == Resource.Id.action_custom_pass)
            {
                var intent = new Intent(this, typeof(CustomPassword));
                StartActivity(intent);
                this.Finish();
                return true;
            }
            else if (id == Resource.Id.action_about)
            {
                var intent = new Intent(this, typeof(AboutSmush));
                StartActivity(intent);
                this.Finish();
                return true;
            }
            else if (id == Resource.Id.action_eula)
            {
                AndroidX.AppCompat.App.AlertDialog.Builder eula = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
                eula.SetTitle("EULA");
                eula.SetMessage(Resources.GetString(Resource.String.eulatext));
                eula.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    //Do nothing
                });
                Dialog dialog = eula.Create();
                dialog.Show();

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

       
        //private void FabOnClick(object sender, EventArgs eventArgs)
        //{
        //View view = (View)sender;
        //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
        //.SetAction("Action", (View.IOnClickListener)null).Show();
        //}


        //READ FROM THE SETTINGS FILE
        public int ReadSmush()
        {
            int currline = 0;
            int settingcase = 2;
            
            var smushsettings = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushsettings.txt");
            if (smushsettings==null || !File.Exists(smushsettings))
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


        //WRITE TO THE SETTINGS FILE
        public async Task SaveSmush(int smushsound, int smushdark)
        {
            var smushsettings = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushsettings.txt");
            using (var writer = File.CreateText(smushsettings))
            {
                await writer.WriteLineAsync(smushsound.ToString());
                await writer.WriteLineAsync(smushdark.ToString());
            }
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}