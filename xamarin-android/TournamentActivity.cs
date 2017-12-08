﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data.SqlClient;
using System.Data;

namespace xamarin_android
{
    [Activity(Label = "TournamentActivity")]
    public class TournamentActivity : Activity
    {
        //Button bExisting;
        //Button bNew;

        string myCon = "Server=tcp:myserver-20171207.database.windows.net,1433;" +
               "Initial Catalog=foosballDatabase;" +
               "User ID=ServerAdmin963;" +
               "Password=Slapt4z0d1s;" +
               "MultipleActiveResultSets=False;";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Tournament);

            Button updateButton = FindViewById<Button>(Resource.Id.UpdateButton);
            EditText updateID = FindViewById<EditText>(Resource.Id.UpdateID);
            EditText updateName = FindViewById<EditText>(Resource.Id.UpdateName);

            int id;

            // Update button
            updateButton.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(updateID.Text, out id))
                {
                    string name = updateName.Text.ToString();
                    Update(id, name);
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", updateID.Text), ToastLength.Long).Show();
            };

            /*SetContentView(Resource.Layout.a_tournament);

            bExisting = (Button)FindViewById(Resource.Id.bExisting);
            bNew = (Button)FindViewById(Resource.Id.bNew);

            bExisting.Click += delegate
            {
                InputCode();
            };

            bNew.Click += delegate
            {
                Intent create = new Intent(this, typeof(NewTournamentActivity));
                StartActivity(create);
            };*/
        }

        public void Update(int id, string name)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT UserID, Name FROM tabUser", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL update command
                SqlCommand update = new SqlCommand();
                update.Connection = cn;
                update.CommandType = CommandType.Text;
                update.CommandText = "UPDATE tabUser SET Name = @N WHERE UserID = @ID";

                //Establish existing (those used in SELECT)
                update.Parameters.Add(new SqlParameter("@N", SqlDbType.NVarChar, 50, "Name"));
                update.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 10, "UserID"));

                //Data adapter (using statement)
                da.UpdateCommand = update;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabUser");

                //Establish the name and new data for the column to be updated
                ds.Tables[0].Rows[id - 1]["Name"] = name;

                //Update done
                da.Update(ds.Tables[0]);
            }
        }

        private void InputCode()
        {
            AlertDialog.Builder Code = new AlertDialog.Builder(this);
            Code.SetTitle(Resource.String.t_code);
            View input = LayoutInflater.Inflate(Resource.Layout.d_PIN, null);
            EditText pin = (EditText)input.FindViewById(Resource.Id.PIN);
            Code.SetView(input);
            Code.SetPositiveButton("Enter", (senderAlert, args) =>
            {
                var intent = new Intent(this, typeof(TournamentActivity));
                intent.PutExtra("pin", pin.Text);
                StartActivity(intent);
                Finish();
            });

            Code.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                Code.Dispose();
            });

            Dialog dialog = Code.Create();
            dialog.Show();
        }
    }
}