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
using System;
//using Android.Gms.Ads;


namespace Smush
{   //Property of Kuziwakwashe Stephen Mashingaidze
    [Activity(Label = "@string/content_third_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CustomPassword : AppCompatActivity
    {
        //Declare global variables for ease of use.
        int currline = 0;
        int arrayind = 0;
        string[] titlearray = new string[100];
        string[] passwordarray = new string[100];
        string[] answerarray = new string[100];
        string[] questionarray = new string[100];
        bool load = false;
        private Toast toast = null;
        //protected AdView myadview;

        //Declare question variable as global, so it can be used in the drop-down list handler
        string question = "None";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_third);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar); There is already a toolbar, from the main activity layout


            //INITIALIZE ADS
            //MobileAds.Initialize(this); //Initialize Mobile Ads SDK. Needs to be done at the beginning
            //myadview = FindViewById<AdView>(Resource.Id.adView1);
            //var adrequest = new AdRequest.Builder().Build();
            //myadview.LoadAd(adrequest);

            //Locate Background
            RelativeLayout bg = FindViewById<RelativeLayout>(Resource.Id.cuspasslayout);

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


            //Define variables
            string title="";
            string password="";
            string answer= "Nonecalifragkuziwakwashestephenmashingaidze";

            //Test variables
            //TextView textview8 = FindViewById<TextView>(Resource.Id.textView8);
            //TextView textview9 = FindViewById<TextView>(Resource.Id.textView9);


            //If receiving a password from the Password Generator screen, load it in.
            if ((Intent.GetStringExtra("password"))!="")
            {
                password = Intent.GetStringExtra("password");
            }

            //INITIALIZE SCREEN ELEMENTS
            EditText edittext1 = FindViewById<EditText>(Resource.Id.editText1);
            EditText edittext2 = FindViewById<EditText>(Resource.Id.editText2);
            EditText edittext4 = FindViewById<EditText>(Resource.Id.editText4);
            toast = Toast.MakeText(this, "", ToastLength.Short);
            Button button1 = FindViewById<Button>(Resource.Id.button1);
            Button button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Enabled = false; //Initially, CLEAR button is disabled
            //Bind Security Question From Drop-Down List
            Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
            spinner1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner1_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.cuspassarray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner1.Adapter = adapter;


            //Bind User-Defined Title
            edittext1.TextChanged += (sender, args) => {
                //Handle forbidden characters
                if (edittext1.Text.Contains(" ") || edittext1.Text.Contains("\n")) 
                {
                    edittext1.Text = edittext1.Text.Replace("\n","").Replace("\r","").Replace(" ",""); //Remove new line characters, carriage returns (cursor to beginning), and spaces from the string
                    toast.SetText("New lines and spaces are not permitted in the title.");
                    toast.Show();
                    //Toast.MakeText(this, "New lines are not permitted in the password.", ToastLength.Short).Show();
                }

                //Limit length of text
                if (edittext1.Text.Length > 16)
                {
                    toast.SetText("The maximum number of characters allowed per field is 16.");
                    toast.Show();
                    edittext1.Text = edittext1.Text.Remove(16); //Remove every character from index 16 onwards. Remember, we only want to keep characters 0-15. 
                }

                //If all the edit text fields are empty and the drop down list is set to None, disable the CLEAR button
                if ((string.IsNullOrEmpty(edittext1.Text)) & (string.IsNullOrEmpty(edittext2.Text)) & (string.IsNullOrEmpty(edittext4.Text)) & (spinner1.SelectedItem.ToString() == "None"))
                {
                    button2.Enabled = false;   
                }
                else
                {
                    button2.Enabled = true;
                }


                title = edittext1.Text;
                
            };
            

            //Bind User-Defined Password
            edittext2.Text = password; //In case you are loading a password from the Password Generator page.
            edittext2.TextChanged += (sender, args) => {
                //Handle forbidden characters
                if (edittext2.Text.Contains(" ") || edittext2.Text.Contains("\n")) 
                {
                    edittext2.Text = edittext2.Text.Replace("\n","").Replace("\r","").Replace(" ",""); //Remove new line characters, carriage returns (cursor to beginning), and spaces from the string
                    toast.SetText("New lines and spaces are not permitted in the password.");
                    toast.Show();
                    //Toast.MakeText(this, "New lines are not permitted in the password.", ToastLength.Short).Show();
                }

                //Limit length of text
                if (edittext2.Text.Length > 16)
                {
                    toast.SetText("The maximum number of characters allowed per field is 16.");
                    toast.Show();
                    edittext2.Text = edittext2.Text.Remove(16); //Remove every character from index 16 onwards. Remember, we only want to keep characters 0-15. 
                }

                //If all the edit text fields are empty and the drop down list is set to None, disable the CLEAR button
                if ((string.IsNullOrEmpty(edittext1.Text)) & (string.IsNullOrEmpty(edittext2.Text)) & (string.IsNullOrEmpty(edittext4.Text)) & (spinner1.SelectedItem.ToString() == "None"))
                {
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                }


                password = edittext2.Text;
                


            };
            
            
            //Bind User-Defined Response To The Security Question
            edittext4.TextChanged += (sender, args) => {
                //Handle forbidden characters                   
                if (edittext4.Text.Contains("\n"))
                {
                    edittext4.Text = edittext4.Text.Replace("\n", "").Replace("\r", ""); //Remove new line characters from both ends of the string.Since the 'TextChanged' aspect of the edit text is constantly refreshed, this should work.
                    toast.SetText("New lines are not permitted in the security response.");
                    toast.Show();
                    //Toast.MakeText(this, "New lines are not permitted in the response.", ToastLength.Short).Show();
                }

                //Limit length of text
                if (edittext4.Text.Length > 16)
                {
                    toast.SetText("The maximum number of characters allowed per field is 16.");
                    toast.Show();
                    edittext4.Text = edittext4.Text.Remove(16); //Remove every character from index 16 onwards. Remember, we only want to keep characters 0-15. 
                }

                //If all the edit text fields are empty and the drop down list is set to None, disable the CLEAR button
                if ((string.IsNullOrEmpty(edittext1.Text)) & (string.IsNullOrEmpty(edittext2.Text)) & (string.IsNullOrEmpty(edittext4.Text)) & (spinner1.SelectedItem.ToString() == "None"))
                {
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                }

                answer = edittext4.Text;
                


            };


            
            //When the SAVE button is pressed
            button1.Click += async delegate
            {
                if (smushsound == 1)
                {
                    MediaPlayer splatplayer;
                    splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);
                    splatplayer.Start();
                    await Task.Delay(500);
                    splatplayer.Release();
                }

                if (string.IsNullOrEmpty(answer))
                {
                    answer = "Nonecalifragkuziwakwashestephenmashingaidze";
                }

                if (string.IsNullOrEmpty(question))
                {
                    question = "None";
                }


                //READ SMUSH BANK FILE
                ReadBank(); //Call function
                
                

                //Ensure mandatory fields are not left empty.
                if (string.IsNullOrEmpty(title))
                {
                    toast.SetText("Mandatory fields must be completed.");
                    toast.Show();
                    //Toast.MakeText(this, "Mandatory fields must be completed.", ToastLength.Short).Show();
                }
                else if ((titlearray.Contains(title)) & (load==true))
                {
                    toast.SetText("The title entered already exists in the password bank.");
                    toast.Show();
                    //Toast.MakeText(this, "The title entered already exists in the Password Bank.", ToastLength.Short).Show();
                }
                else if (string.IsNullOrEmpty(password))
                {
                    toast.SetText("Mandatory fields must be completed.");
                    toast.Show();
                    //Toast.MakeText(this, "Mandatory fields must be completed.", ToastLength.Short).Show();
                }
                else if ((question.Equals("None")) & (!answer.Equals("Nonecalifragkuziwakwashestephenmashingaidze"))) //Ensure that if no question is selected, you cannot enter a response.
                {
                    toast.SetText("To enter a response, a security question must be selected.");
                    toast.Show();
                    //Toast.MakeText(this, "To enter a response, a security question must be selected.", ToastLength.Short).Show();
                }
                else if ((!question.Equals("None")) & (answer.Equals("Nonecalifragkuziwakwashestephenmashingaidze")))
                {
                    toast.SetText("A response is required for the selected security question.");
                    toast.Show();
                    //Toast.MakeText(this, "A response is required for the selected security question.", ToastLength.Short).Show();
                }
                else
                {
                    if ((question.Equals("None")) & (answer.Equals("Nonecalifragkuziwakwashestephenmashingaidze")))
                    {
                        question = "None";
                        answer = "Nonecalifragkuziwakwashestephenmashingaidze";
                    }


                    //Check if the password bank is full
                    if (arrayind < titlearray.Length) //If value of arrayind, after ReadSmush was called, is less than the maximum array length
                    {
                        //Save password to the Smush Bank File
                        await SaveBank(title, password, question, answer);
                        toast.SetText("Save successful.");
                        toast.Show();
                        //Toast.MakeText(this, "Save successful.", ToastLength.Short).Show();
                    }
                    else //If arrayind is greater than titlearray.Length - 1, thus exceeding the max. array length
                    {
                        //Notify the user that the password bank is full.
                        toast.SetText("The password bank is full.");
                        toast.Show();
                    }

                    


                }
                

            };


            //When the CLEAR button is pressed
            button2.Click += async delegate
            {
                if (smushsound == 1)
                {
                    MediaPlayer splatplayer;
                    splatplayer = MediaPlayer.Create(this, Resource.Raw.splat);
                    splatplayer.Start();
                    await Task.Delay(500);
                    splatplayer.Release();
                }

                edittext1.Text = ""; //Clear title field
                edittext2.Text = ""; //Clear password field
                spinner1.SetSelection(0); //Reset drop down list
                edittext4.Text = ""; //Clear security answer field

                toast.SetText("Clear successful.");
                toast.Show();
                //Toast.MakeText(this, "Clear successful.", ToastLength.Short).Show();
            };


        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_third, menu);
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
            else if (id == Resource.Id.action_settings)
            {
                var intent = new Intent(this, typeof(SmushSettings));
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


        //READ FROM THE SMUSH BANK FILE TO AVOID SAVING DUPLICATE TITLE ENTRIESS
        public void ReadBank()
        {
            //Reset index variables
            arrayind = 0;
            currline = 0;

            var smushbank = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushbank.txt");
            if (smushbank == null || !File.Exists(smushbank)) //If the bank file doesn't exist
            {
                load = false;
            }
            else
            {
                load = true;
                using (var reader = new StreamReader(smushbank, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        currline++;

                        if (currline == 1)
                        {
                            titlearray[arrayind] = line;
                        }
                        else if (currline == 2)
                        {
                            passwordarray[arrayind] = line;
                        }
                        else if (currline == 3)
                        {
                            questionarray[arrayind] = line;
                        }
                        else if (currline == 4)
                        {
                            answerarray[arrayind] = line;
                            currline = 0;
                            arrayind++;
                        }


                    }
                }
            }
        }



        //WRITE TO THE SMUSH BANK FILE
        public async Task SaveBank(string title, string password, string question, string answer)
        {
            var smushbank = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushbank.txt");
            //If the bank file doesn't exist
            if (smushbank == null || !File.Exists(smushbank)) 
            {
                using (var writer = File.CreateText(smushbank)) //Create new password bank text file
                {
                    await writer.WriteLineAsync(title);
                    await writer.WriteLineAsync(password);
                    await writer.WriteLineAsync(question);
                    await writer.WriteLineAsync(answer);
                }
            }
            else //If the bank file already exists, we don't want to overwrite the content already in it
            {
                using (var writer = File.AppendText(smushbank)) //Append new passwords to password bank
                {
                    await writer.WriteLineAsync(title);
                    await writer.WriteLineAsync(password);
                    await writer.WriteLineAsync(question);
                    await writer.WriteLineAsync(answer);
                }
            }
            

        }




        //HANDLE DROP-DOWN LIST
        public void spinner1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Initialize some elements again, in the scope of the drop-down list.
            EditText edittext1 = FindViewById<EditText>(Resource.Id.editText1);
            EditText edittext2 = FindViewById<EditText>(Resource.Id.editText2);
            EditText edittext4 = FindViewById<EditText>(Resource.Id.editText4);
            Button button2 = FindViewById<Button>(Resource.Id.button2);

            Spinner spinner1 = (Spinner)sender;
            question = spinner1.SelectedItem.ToString();

            //If all the edit text fields are empty and the drop down list is set to None, disable the CLEAR button
            if ((string.IsNullOrEmpty(edittext1.Text)) & (string.IsNullOrEmpty(edittext2.Text)) & (string.IsNullOrEmpty(edittext4.Text)) & (spinner1.SelectedItem.ToString() == "None"))
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}