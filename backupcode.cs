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


namespace Smush
{
    [Activity(Label = "@string/content_second_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SmushMeat : AppCompatActivity
    {
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

            //Initialize counter variables
            int totalcount = 0;
            int lettercount = 0;
            int digitcount = 0;
            int symbolcount = 0;
            string pstring;

            //Handle number of letters entered
            EditText edittext1 = FindViewById<EditText>(Resource.Id.editText1);
            edittext1.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (edittext1.Text != "")
                    {
                        Toast.MakeText(this, edittext1.Text, ToastLength.Short).Show();
                        totalcount = Int32.Parse(edittext1.Text);
                    }
                    else
                    {
                        totalcount = 0;
                    }
                    
                    e.Handled = true;
                }
            };

            //Handle number of letters entered
            EditText edittext2 = FindViewById<EditText>(Resource.Id.editText2);
            edittext2.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (edittext2.Text != "")
                    {
                        Toast.MakeText(this, edittext2.Text, ToastLength.Short).Show();
                        lettercount = Int32.Parse(edittext2.Text);
                    }
                    else
                    {
                        lettercount = 0;
                    }
                    
                    e.Handled = true;
                }
            };

            //Handle number of digits entered
            EditText edittext3 = FindViewById<EditText>(Resource.Id.editText3);
            edittext3.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (edittext3.Text != "")
                    {
                        Toast.MakeText(this, edittext3.Text, ToastLength.Short).Show();
                        digitcount = Int32.Parse(edittext3.Text);
                    }
                    else
                    {
                        digitcount = 0;
                    }
                    
                    e.Handled = true;
                }
            };

            //Handle number of symbols
            EditText edittext4 = FindViewById<EditText>(Resource.Id.editText4);
            edittext4.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    if (edittext4.Text != "")
                    {
                        Toast.MakeText(this, edittext4.Text, ToastLength.Short).Show();
                        symbolcount = Int32.Parse(edittext4.Text);
                    }
                    else
                    {
                        symbolcount = 0;
                    }
                    
                    e.Handled = true;
                }
            };

            //Declare output Textview
            TextView textview6 = FindViewById<TextView>(Resource.Id.textView6);

            // When the 'GENERATE' button is pressed
            Button button2 = FindViewById<Button>(Resource.Id.button1);
            button2.Click += delegate
            {
                MediaPlayer splatplayer;
                splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);
                splatplayer.Start();

                pstring = ""; //Reinitialize the password string variable

                //Define all possible conditions
                if (totalcount<17) //Limit password to less than 16 characters
                {
                    if (totalcount>0)
                    {
                        if (lettercount>0) //If the number of letters is given
                        {
                            if (digitcount>0) //If the number of letters and digits are given
                            {
                                if (symbolcount>0) //If the number of letters, digits, and symbols are all given
                                {
                                    int tally = lettercount + digitcount + symbolcount;
                                    if (tally==totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                    {
                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                    }
                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                    {
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
                                    }
                                }
                                else //If ONLY the number of letters and digits are given
                                {
                                    symbolcount = totalcount - lettercount - digitcount; //Calculate the symbolcount
                                    int tally = lettercount + digitcount + symbolcount;
                                    if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                    {
                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                    }
                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                    {
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
                                    }
                                }
                            }
                            else //If the number of letters is given and the number of digits is not given
                            {
                                if (symbolcount > 0) //If the number of letters AND the number of symbols is given
                                {
                                    digitcount = totalcount - lettercount - symbolcount;
                                    int tally = lettercount + digitcount + symbolcount;
                                    if (tally == totalcount) //If the numnber of letters, digts, and symbols tally with the given total password length
                                    {
                                        pstring = RandomPassword (lettercount, digitcount, symbolcount);
                                    }
                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                    {
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
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
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
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
                                    int tally = lettercount + digitcount + symbolcount;
                                    if (tally == totalcount) //If the number of letters, digits, and symbols tally with the given total password length
                                    {
                                        pstring = RandomPassword(lettercount, digitcount, symbolcount);
                                    }
                                    else //If the number of letters, digits, and symbols DO NOT tally with the given total password length
                                    {
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
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
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
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
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
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
                                        Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
                                    }
                                }

                            }
                        }
                    }
                }
                else //If the total password length given or omitted is invalid
                {
                    Toast.MakeText(this, "The optional values you have entered exceed the password bounds.", ToastLength.Long).Show();
                }


                textview6.Text = pstring;

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
                this.Finish();
                return true;
            }
            else if (id == Resource.Id.action_settings)
            {
                return true;
            }
            else if (id == Resource.Id.action_pass_bank)
            {
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

            string pstring = new string(unjumbled);

            return (pstring);
        }


        //private void FabOnClick(object sender, EventArgs eventArgs)
        //{
            //View view = (View)sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                //.SetAction("Action", (View.IOnClickListener)null).Show();
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}