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
    [Activity(Label = "@string/content_fourth_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PasswordBank : AppCompatActivity
    {
        //Declare global variables for ease of use.
        int currline = 0;
        int arrayind = 0;
        string [] titlearray = new string[100];
        string [] passwordarray = new string[100];
        string [] answerarray = new string[100];
        string [] questionarray = new string[100];
        string title = "";
        int index = 0;
        bool load = false;
        bool smushbankcleared = false;
        private Toast toast = null;
        //protected AdView myadview;

        //titlearray cannot have any blank values
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_fourth);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar); There is already a toolbar, from the main activity layout

            //Used to be for a send feedback message button named fab
            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;


            //INITIALIZE ADS
            //MobileAds.Initialize(this); //Initialize Mobile Ads SDK. Needs to be done at the beginning
            //myadview = FindViewById<AdView>(Resource.Id.adView1);
            //var adrequest = new AdRequest.Builder().Build();
            //myadview.LoadAd(adrequest);

            //Locate Background
            RelativeLayout bg = FindViewById<RelativeLayout>(Resource.Id.passbanklayout);

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

            //Initialize visual elements
            TextView textview1 = FindViewById<TextView>(Resource.Id.textView1); //Initialize question prompt
            TextView textview2 = FindViewById<TextView>(Resource.Id.textView2); //Initialize question prompt
            TextView textview3 = FindViewById<TextView>(Resource.Id.textView3); //Initialize answer prompt
            TextView textview4 = FindViewById<TextView>(Resource.Id.textView4); //Initialize question display
            TextView textview6 = FindViewById<TextView>(Resource.Id.textView6); //Initialize output display
            EditText edittext3 = FindViewById<EditText>(Resource.Id.editText3); //Initialize answer box
            Button button1 = FindViewById<Button>(Resource.Id.button1); //Do not add 1-tap counter here. This item will be reused.
            Button button2 = FindViewById<Button>(Resource.Id.button2);
            ImageView imageview1 = FindViewById<ImageView>(Resource.Id.imageView1);
            toast = Toast.MakeText(this, "", ToastLength.Short);

            textview1.Visibility = ViewStates.Invisible;
            textview2.Visibility = ViewStates.Invisible;
            textview3.Visibility = ViewStates.Invisible;
            textview4.Visibility = ViewStates.Invisible;
            edittext3.Visibility = ViewStates.Invisible;
            edittext3.Enabled = false; //Initially, you cannot enter a response
            textview6.Text = ""; //Clear output display
            button1.Enabled = false; //Initially, unlock button is disabled
            button1.Visibility = ViewStates.Invisible; //Initially, unlock button is invisible
            button2.Enabled = false; //Initially, delete button is disabled
            button2.Visibility = ViewStates.Invisible; //Initially, delete button is invisible
            imageview1.Visibility = ViewStates.Invisible; //Initially, nobody home graphic is invisible

           
            //READ SMUSH BANK FILE
            ReadBank(); //Call function

            //If the bank file has loaded successfully
            if (load == true)
            {
                textview1.Visibility = ViewStates.Visible; //First prompt becomes visible
                button1.Visibility = ViewStates.Visible; //Unlock button becomes visible
                button2.Visibility = ViewStates.Visible; //Delete button becomes visible
                imageview1.Visibility = ViewStates.Invisible; //Hide nobody home graphic

                //Programmatically Populate Drop-Down List With Titles
                //First create an array that is just the size of the number of saved titles, so list is short, despite large storage size
                string[] spinnerarray = new string[arrayind];
                for (int i = 0; i<arrayind; i++)
                {
                    spinnerarray[i] = titlearray[i];
                }
                var items = new List<string>(spinnerarray);
                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                var spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
                spinner1.Adapter = adapter;
                spinner1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner1_ItemSelected);

                //Handle Security Response Entry
                edittext3.TextChanged += (sender, args) => {
                    //Handle forbidden characters                   
                    if (edittext3.Text.Contains("\n"))
                    {
                        edittext3.Text = edittext3.Text.Replace("\n", "").Replace("\r", ""); //Remove new line characters from both ends of the string.Since the 'TextChanged' aspect of the edit text is constantly refreshed, this should work.
                        toast.SetText("New lines are not permitted in the security response.");
                        toast.Show();
                        //Toast.MakeText(this, "New lines are not permitted in the response.", ToastLength.Short).Show();
                    }

                    //Limit length of text
                    if (edittext3.Text.Length > 16)
                    {
                        toast.SetText("The maximum number of characters allowed per field is 16.");
                        toast.Show();
                        edittext3.Text = edittext3.Text.Remove(16); //Remove every character from index 16 onwards. Remember, we only want to keep characters 0-15. 
                    }

                    
                };



                //When the unlock button is pressed
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

                    //DISPLAY PASSWORD OR NOT
                    if (questionarray[index].Equals("None")) //If password is not protected
                    {
                        textview6.Text = passwordarray[index];
                        button2.Enabled = true; //Allow the user to delete the password if it is unlocked
                        toast.SetText("Password unlocked.");
                        toast.Show();
                        //Toast.MakeText(this, "Password unlocked.", ToastLength.Short).Show();
                    }
                    else if (answerarray[index].Equals(edittext3.Text)) //Else if the user enters the correct security answer
                    {
                        textview6.Text = passwordarray[index]; //Else an invalid security answer is provided
                        button2.Enabled = true; //Allow the user to delete the password if it is unlocked
                        toast.SetText("Password unlocked.");
                        toast.Show();
                        //Toast.MakeText(this, "Password unlocked.", ToastLength.Short).Show();
                    }
                    else
                    {
                        textview6.Text = ""; //Clear output display
                        button2.Enabled = false; //Prevent the user from deleting a locked password
                        toast.SetText("Invalid security response.");
                        toast.Show();
                        //Toast.MakeText(this, "Invalid security response.", ToastLength.Short).Show();
                    }



                };

                //When the delete button is pressed
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

                    toast.SetText("Delete successful.");
                    toast.Show();
                    //Toast.MakeText(this, "Delete successful.", ToastLength.Short).Show();

                    //If the Smush Bank only has one item
                    //After deleting the final item, we will delete the entire Smush Bank file, so we go back to the nobody home page
                    if (arrayind == 1)
                    {
                        var smushbank = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushbank.txt");
                        File.Delete(smushbank);
                    }
                    else //If the Smush Bank has more than one item, business as usual
                    {
                        //Delete the current item, then fill the void
                        for (int i = index; i < (titlearray.Length - 1); i++)
                        {
                            //Initiate leftward shift, to fill the void of the deleted item.
                            titlearray[i] = titlearray[i + 1];
                            passwordarray[i] = passwordarray[i + 1];
                            questionarray[i] = questionarray[i + 1];
                            answerarray[i] = answerarray[i + 1];
                            //Clear last item in the aray, completing leftward shift. Implemented separately from the above code to avoid array out of bounds error
                            titlearray[(titlearray.Length) - 1] = "";
                            passwordarray[(titlearray.Length) - 1] = "";
                            questionarray[(titlearray.Length) - 1] = "";
                            answerarray[(titlearray.Length) - 1] = "";

                        }

                        //Write the shifted items to the Smush Bank file, omitting the blank entries of the super large array.
                        for (int i = 0; i < (arrayind - 1); i++) //arrayind-1 is used since we have deleted one of the entries and shifted left.
                        {
                            //Save items without the title, password, question, and array components of the deleted item
                            await SaveBank(titlearray[i], passwordarray[i], questionarray[i], answerarray[i]);
                        }
                    }

                    

                    //Restart the activity
                    var intent = new Intent(this, typeof(PasswordBank));
                    this.Finish();
                    StartActivity(intent);
                    
                };


            }
            else //If there are no saved passwords
            {
                textview1.Visibility = ViewStates.Invisible;
                imageview1.Visibility = ViewStates.Visible; //Reveal nobody home graphic
                toast.SetText("There are no saved passwords.");
                toast.Show();
                //Toast.MakeText(this, "There are no saved passwords available.", ToastLength.Short).Show();
            };

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_fourth, menu);
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
            else if (id == Resource.Id.action_settings)
            {
                var intent = new Intent(this, typeof(SmushSettings));
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



        //READ FROM THE SMUSH BANK FILE
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
                        else if (currline ==3)
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


        //CLEAR THE EXISTING SMUSH BANK FILE, THEN APPEND EACH ITEM PASSED FROM THE DELETE SUBROUTINE
        public async Task SaveBank(string title, string password, string question, string answer)
        {
            var smushbank = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smushbank.txt");
            if (smushbankcleared == false)
            {
                using (var writer = File.CreateText(smushbank)) //Create new password bank text file, erasing the old one.
                {
                    await writer.WriteLineAsync(title);
                    await writer.WriteLineAsync(password);
                    await writer.WriteLineAsync(question);
                    await writer.WriteLineAsync(answer);
                    smushbankcleared = true; //First overwrite is done. Now every item after this needs to be appended
                }
            }
            else if (smushbankcleared == true) //Overwrite has already been performed. Now the rest of the data is appended
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
        public void Spinner1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner1 = (Spinner)sender;
            title = spinner1.SelectedItem.ToString();
            index = spinner1.SelectedItemPosition;

            Button button1 = FindViewById<Button>(Resource.Id.button1); //Initialize unlock button
            Button button2 = FindViewById<Button>(Resource.Id.button2); //Initialize delete button
            TextView textview2 = FindViewById<TextView>(Resource.Id.textView2); //Initialize question prompt
            TextView textview3 = FindViewById<TextView>(Resource.Id.textView3); //Initialize answer prompt
            TextView textview4 = FindViewById<TextView>(Resource.Id.textView4); //Initialize question display
            TextView textview6 = FindViewById<TextView>(Resource.Id.textView6); //Initialize output display
            EditText edittext3 = FindViewById<EditText>(Resource.Id.editText3); //Initialize answer box
            textview6.Text = ""; //Clear output field
            edittext3.Text = ""; //Clear security response input field
            button2.Enabled = false; //Prevent the user from deleting a locked password
            if (string.IsNullOrEmpty(title))
            {
                button1.Enabled = false;
                textview2.Visibility = ViewStates.Invisible;
                textview4.Visibility = ViewStates.Invisible;
                textview3.Visibility = ViewStates.Invisible;
                edittext3.Visibility = ViewStates.Invisible;

                textview4.Text = "";

            }
            else
            {

                button1.Enabled = true;
                textview2.Visibility = ViewStates.Visible;
                textview4.Visibility = ViewStates.Visible;
                textview3.Visibility = ViewStates.Visible;
                edittext3.Visibility = ViewStates.Visible;

                textview4.Text = questionarray[index]; //Display security question prompt

                if (questionarray[index].Equals("None"))
                {
                    edittext3.Enabled = false;
                    edittext3.Visibility = ViewStates.Invisible;
                    textview3.Visibility = ViewStates.Invisible;

                }
                else
                {
                    edittext3.Enabled = true;
                    edittext3.Visibility = ViewStates.Visible;
                    textview3.Visibility = ViewStates.Visible;
                }

            }

            



        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}