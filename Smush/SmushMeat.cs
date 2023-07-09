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
using System.IO;
using Android.Media;
using System.Threading.Tasks;
//using Android.Gms.Ads;

namespace Smush
{   //Property of Kuziwakwashe Stephen Mashingaidze
    [Activity(Label = "@string/content_second_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SmushMeat : AppCompatActivity
    {
        private Toast toast = null;
        //protected AdView myadview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_second);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar); There is already a toolbar, from the main activity layout

            //Used to be for a send feedback message button named fab
            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

            //Locate Background
            RelativeLayout bg = FindViewById<RelativeLayout>(Resource.Id.passgenlayout);


            //INITIALIZE ADS
            //MobileAds.Initialize(this); //Initialize Mobile Ads SDK. Needs to be done at the beginning
            //myadview = FindViewById<AdView>(Resource.Id.adView1);
            //var adrequest = new AdRequest.Builder().Build();
            //myadview.LoadAd(adrequest);

            //READ SETTINGS FILE
            int settingcase = ReadSmush();
            int smushsound = 1;
            int smushdark = 0;

            if (settingcase == 0)
            {
                smushsound = 0;
                smushdark = 0;
                bg.SetBackgroundColor(Android.Graphics.Color.White);
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
                bg.SetBackgroundColor(Android.Graphics.Color.White);
            }
            else if (settingcase == 3)
            {
                smushsound = 1;
                smushdark = 1;
                bg.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CD8450"));
            }



            //Initialize counter variables
            int totalcount = 0;
            int lettercount = 0;
            int digitcount = 0;
            int symbolcount = 0;
            string pstring;
            int counter = 0;

            //Initialize Toast
            toast = Toast.MakeText(this, "", ToastLength.Short);


            //Handle number of letters entered
            EditText edittext1 = FindViewById<EditText>(Resource.Id.editText1);
            edittext1.TextChanged += (sender, args) => {
                //e.Handled = false;
                //if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                //{
                if (edittext1.Text != "")
                 {
                    //Toast.MakeText(this, edittext1.Text, ToastLength.Short).Show();
                    totalcount = Int32.Parse(edittext1.Text);

                    //Limit password length
                    if (totalcount > 16) //The maximum number of characters allowed is 16.
                    {
                        toast.SetText("The password must be 3 - 16 characters long.");
                        toast.Show();
                        edittext1.Text = ""; //Clear field
                    }
                }

                


                //e.Handled = true;

                //}
            };

            //Handle number of letters entered
            EditText edittext2 = FindViewById<EditText>(Resource.Id.editText2);
            edittext2.TextChanged += (sender, args) => {
                //e.Handled = false;
                //if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                //{
                if (edittext2.Text != "")
                 {
                    lettercount = Int32.Parse(edittext2.Text);
                    
                    //Limit number of letters
                    if (lettercount > 14) //The maximum number of characters allowed is 16. Note that there should be at least one occurence of each of the three character types. 
                    {
                        toast.SetText("The number of letters is too large.");
                        toast.Show();
                        edittext2.Text = ""; //Clear field 
                    }
                }

                


                //e.Handled = true;
                //}
            };

            //Handle number of digits entered
            EditText edittext3 = FindViewById<EditText>(Resource.Id.editText3);
            edittext3.TextChanged += (sender, args) => {
                //e.Handled = false;
                //if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                //{
                if (edittext3.Text != "")
                 {
                    //Toast.MakeText(this, edittext3.Text, ToastLength.Short).Show();
                    digitcount = Int32.Parse(edittext3.Text);
                    
                    //Limit number of digits. 
                    if (digitcount > 14) //The maximum number of characters allowed is 16. Note that there should be at least one occurence of each of the three character types. 
                    {
                        toast.SetText("The number of digits is too large.");
                        toast.Show();
                        edittext3.Text = ""; //Clear field 
                    }
                }

                //e.Handled = true;
                //}
            };

            //Handle number of symbols
            EditText edittext4 = FindViewById<EditText>(Resource.Id.editText4);
            edittext4.TextChanged += (sender, args) => {
                //e.Handled = false;
                //if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                //{
                if (edittext4.Text != "")
                 {
                    //Toast.MakeText(this, edittext4.Text, ToastLength.Short).Show();
                    symbolcount = Int32.Parse(edittext4.Text);

                    //Limit number of symbols
                    if (symbolcount > 14) //The maximum number of characters allowed is 16. Note that there should be at least one occurence of each of the three character types. 
                    {
                        toast.SetText("The number of symbols is too large.");
                        toast.Show();
                        edittext4.Text = ""; //Clear field 
                    }
                }

                //e.Handled = true;
                //}
            };

            //Declare output Textview
            TextView textview6 = FindViewById<TextView>(Resource.Id.textView6);
            //Declare test Textviews
            //TextView textview7 = FindViewById<TextView>(Resource.Id.textView7);
            //TextView textview8 = FindViewById<TextView>(Resource.Id.textView8);
            //TextView textview9 = FindViewById<TextView>(Resource.Id.textView9);
            //TextView textview10 = FindViewById<TextView>(Resource.Id.textView10);

            //When the 'SAVE' button is pressed
            Button button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Click += async delegate
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
                    
                    var intent = new Intent(this, typeof(CustomPassword));
                    intent.PutExtra("password", textview6.Text);
                    StartActivity(intent);
                    this.Finish();
                }

            };



            // When the 'GENERATE' button is pressed
            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += async delegate
            {
                if (smushsound==1)
                {
                    MediaPlayer splatplayer;
                    splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);
                    splatplayer.Start();
                    await Task.Delay(500);
                    splatplayer.Release();
                }
                

                pstring = ""; //Reinitialize the password string variable

                //Handle empty strings in total number of characters prompt
                if (string.IsNullOrEmpty(edittext1.Text))
                {
                    totalcount = 0;
                }

                //Handle 0s and Empty Strings in number of letters prompt
                if (string.IsNullOrEmpty(edittext2.Text))
                {
                    lettercount = 0;

                }
                else if (edittext2.Text[0] == '0')
                {
                    lettercount = 0;
                    edittext2.Text = "";
                    toast.SetText("Your password must contain at least 1 letter, digit, and symbol.");
                    toast.Show();
                    //Toast.MakeText(this, "Your password must contain at least 1 letter, digit, and symbol.", ToastLength.Short).Show();
                }

                //Handle 0s and Empty Strings in number of digits prompt
                if (string.IsNullOrEmpty(edittext3.Text))
                {
                    digitcount = 0;                    
                }
                else if (edittext3.Text[0] == '0')
                {
                    digitcount = 0;
                    edittext3.Text = "";
                    toast.SetText("Your password must contain at least 1 letter, digit, and symbol.");
                    toast.Show();
                    //Toast.MakeText(this, "Your password must contain at least 1 letter, digit, and symbol.", ToastLength.Short).Show();
                }

                //Handle 0s and Empty Strings in number of symbols prompt
                if (string.IsNullOrEmpty(edittext4.Text))
                {
                    symbolcount = 0;
                }
                else if (edittext4.Text[0] == '0')
                {
                    symbolcount = 0;
                    edittext4.Text = "";
                    toast.SetText("Your password must contain at least 1 letter, digit, and symbol.");
                    toast.Show();
                    //Toast.MakeText(this, "Your password must contain at least 1 letter, digit, and symbol.", ToastLength.Short).Show();
                }



                //Define all possible conditions
                if (totalcount<17) //Limit password to less than 16 characters
                {
                    if (totalcount>2) //Password must be at least 3 characters, in order to satisfy minimum requirements
                    {
                        if (lettercount<(totalcount-1))
                        {
                            if (digitcount<(totalcount-1))
                            {
                                if (symbolcount<(totalcount-1))
                                {
                                    if (lettercount > 0) //If the number of letters is given
                                    {
                                        if (digitcount > 0) //If the number of letters and digits are given
                                        {
                                            if (symbolcount > 0) //If the number of letters, digits, and symbols are all given
                                            {
                                                int tally = lettercount + digitcount + symbolcount;
                                                if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                {
                                                    pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                }
                                                else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                            else //If ONLY the number of letters and digits are given
                                            {
                                                symbolcount = totalcount - lettercount - digitcount; //Calculate the symbolcount
                                                if (symbolcount > 0) //Ensure there is at least one symbol in the password
                                                {
                                                    int tally = lettercount + digitcount + symbolcount;
                                                    if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                    {
                                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                    }
                                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                    {
                                                        toast.SetText("The optional values you have entered are out of bounds.");
                                                        toast.Show();
                                                        //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                    }
                                                }
                                                else
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                        }
                                        else //If the number of letters is given and the number of digits is not given
                                        {
                                            if (symbolcount > 0) //If the number of letters AND the number of symbols is given
                                            {
                                                digitcount = totalcount - lettercount - symbolcount;
                                                if (digitcount > 0)
                                                {
                                                    int tally = lettercount + digitcount + symbolcount;
                                                    if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                    {
                                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                    }
                                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                    {
                                                        toast.SetText("The optional values you have entered are out of bounds.");
                                                        toast.Show();
                                                        //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                    }
                                                }
                                                else
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                            else //If ONLY the number of letters is given
                                            {
                                                Random random = new Random();
                                                int randomindex = random.Next(1, (totalcount - lettercount - 1));
                                                digitcount = randomindex;
                                                symbolcount = totalcount - lettercount - digitcount; //At least one symbol in the password
                                                int tally = lettercount + digitcount + symbolcount;
                                                if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                {
                                                    pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                }
                                                else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }

                                        }
                                    }
                                    else //If the number of letters is NOT given
                                    {
                                        if (digitcount > 0) //If the number of digits is given
                                        {
                                            if (symbolcount > 0) //If ONLY the number of digits, and symbols is given
                                            {
                                                lettercount = totalcount - digitcount - symbolcount; //Calculate number of letters
                                                if (lettercount >0)
                                                {
                                                    int tally = lettercount + digitcount + symbolcount;
                                                    if (tally == totalcount) //If the number of letters, digits, and symbols tally with the given total password length
                                                    {
                                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                    }
                                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                    {
                                                        toast.SetText("The optional values you have entered are out of bounds.");
                                                        toast.Show();
                                                        //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                    }
                                                }
                                                else
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                            else //If ONLY the number of digits is given
                                            {
                                                Random random = new Random();
                                                int randomindex = random.Next(1, (totalcount - digitcount - 1)); //At least one symbol in the password
                                                lettercount = randomindex;
                                                symbolcount = totalcount - lettercount - digitcount; //Calculate the symbolcount
                                                int tally = lettercount + digitcount + symbolcount;
                                                if (tally == totalcount) //If the number of letters, digits, and symbols tally with the given total password length
                                                {
                                                    pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                }
                                                else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                        }
                                        else //If the number of digits is NOT given
                                        {
                                            if (symbolcount > 0) //If ONLY the number of symbols is given
                                            {
                                                Random random = new Random();
                                                int randomindex = random.Next(1, (totalcount - symbolcount - 1)); //At least one digit in the password
                                                lettercount = randomindex;
                                                digitcount = totalcount - lettercount - symbolcount;
                                                int tally = lettercount + digitcount + symbolcount;
                                                if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                {
                                                    pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                }
                                                else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }
                                            else //If NOTHING is given at all
                                            {
                                                Random random = new Random();
                                                int randomindex = random.Next(1, (totalcount - 2)); //At least one digit and one symbol in the password
                                                lettercount = randomindex;
                                                randomindex = random.Next(1, (totalcount - lettercount - 1)); //At least one symbol in the password
                                                digitcount = randomindex;
                                                symbolcount = totalcount - lettercount - digitcount; //At least one symbol in the password
                                                int tally = lettercount + digitcount + symbolcount;
                                                if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                                {
                                                    pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                                }
                                                else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                                {
                                                    toast.SetText("The optional values you have entered are out of bounds.");
                                                    toast.Show();
                                                    //Toast.MakeText(this, "The optional values you have entered are out of bounds.", ToastLength.Short).Show();
                                                }
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    toast.SetText("The number of symbols is too large.");
                                    toast.Show();
                                    //Toast.MakeText(this, "The number of symbols is too large.", ToastLength.Short).Show();
                                }
                            }
                            else
                            {
                                toast.SetText("The number of digits is too large.");
                                toast.Show();
                                //Toast.MakeText(this, "The number of digits is too large.", ToastLength.Short).Show();
                            }
                        }
                        else
                        {
                            toast.SetText("The number of letters is too large.");
                            toast.Show();
                            //Toast.MakeText(this, "The number of letters is too large.", ToastLength.Short).Show();
                        }
                    }
                    else //If the total password length is not greater than 2
                    {
                        toast.SetText("The password must be 3-16 characters long.");
                        toast.Show();
                        //Toast.MakeText(this, "The password must be 3-16 characters long.", ToastLength.Short).Show();
                    }
                }
                else //If the total password length is not less than 17
                {
                    toast.SetText("The password must be 3-16 characters long.");
                    toast.Show();
                    //Toast.MakeText(this, "The password must be 3-16 characters long.", ToastLength.Short).Show();
                }


                textview6.Text = pstring;


                //ENABLE OR DISABLE SAVE BUTTON
                if (textview6.Text=="")
                {
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                }



                //TEST CODE
                //textview7.Text = totalcount.ToString();
                //textview8.Text = lettercount.ToString();
                //textview9.Text = digitcount.ToString();
                //textview10.Text = symbolcount.ToString();


            };

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_second, menu);
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
            else if (id == Resource.Id.action_settings)
            {
                var intent = new Intent(this, typeof(SmushSettings));
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

        private string RandomPassword(int lettercount, int digitcount, int symbolcount)
        {

            // Initialize character arrays entered
            char[] letters = { 'a', 'A', 'b', 'B', 'c', 'C', 'd', 'D', 'e', 'E', 'f', 'F', 'g', 'G', 'h', 'H', 'i', 'I', 'j', 'J', 'k', 'K', 'l', 'L', 'm', 'M', 'n', 'N', 'o', 'O', 'p', 'P', 'q', 'Q', 'r', 'R', 's', 'S', 't', 'T', 'u', 'U', 'v', 'V', 'w', 'W', 'x', 'X', 'y', 'Y', 'z', 'Z' };
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] symbols = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '|', '[', ']', '{', '}', ':', ';', '?', '/', '.', ',' };


            char[] chosenletters = new char[lettercount];
            char[] chosendigits = new char[digitcount];
            char[] chosensymbols = new char[symbolcount];

            //GET RANDOM LETTERS
            for (int i = 0; i < lettercount; i++)
            {
                Random random = new Random();
                int randomindex = random.Next(0, letters.Length);
                chosenletters[i] = letters[randomindex];
            }

            //GET RANDOM DIGITS
            for (int i = 0; i < digitcount; i++)
            {
                Random random = new Random();
                int randomindex = random.Next(0, digits.Length);
                chosendigits[i] = digits[randomindex];
            }

            //GET RANDOM SYMBOLS
            for (int i = 0; i < symbolcount; i++)
            {
                Random random = new Random();
                int randomindex = random.Next(0, symbols.Length);
                chosensymbols[i] = symbols[randomindex];
            }

            //OBTAIN THE UNJUMBLED PASSWORD
            int totalcount = lettercount + digitcount + symbolcount;
            char[] unjumbled = new char[totalcount];
            for (int j=0; j<lettercount;j++)
            {
                unjumbled[j] = chosenletters[j];
            }

            for (int j =0; j<digitcount; j++)
            {
                unjumbled[j + lettercount] = chosendigits[j];
            }

            for (int j=0; j<symbolcount; j++)
            {
                unjumbled[j + lettercount + digitcount] = chosensymbols[j];
            }


            //JUMBLE UP THE LETTERS
            char[] jumbled = unjumbled; //Initialize a character array, jumbled, where we will swap character positions randomly
            char temp; ///Initialize a variable to temporarily store characters during swapping
            for (int k=0; k<totalcount; k++)
            {
                Random random = new Random();
                int randomindex = random.Next(0, totalcount);
                temp = jumbled[k]; //Assign current character of jumbled to temp
                jumbled[k] = jumbled[randomindex]; //Copy a random character in jumbled to the current position
                jumbled[randomindex] = temp; //Copy the original current character of jumbled to the random position, completing the swap
            }

            string pstring = new string(jumbled); //Create a string from the jumbled character array

            return (pstring);
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


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}