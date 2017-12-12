using System;
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

            // Set our view to Tournamet
            SetContentView(Resource.Layout.Tournament);

            // Update existing player
            Button updateButton = FindViewById<Button>(Resource.Id.UpdateButton);
            EditText updateID = FindViewById<EditText>(Resource.Id.UpdateID);
            EditText updateName = FindViewById<EditText>(Resource.Id.UpdateName);

            // Show all tournaments
            Button showTournaments = FindViewById<Button>(Resource.Id.ShowTournaments);

            // Show results
            Button showResults = FindViewById<Button>(Resource.Id.ShowResults);

            // Add new player
            EditText insertName = FindViewById<EditText>(Resource.Id.InsertName);
            Button insertButton = FindViewById<Button>(Resource.Id.InsertButton);

            // Delete tournament
            EditText deleteID = FindViewById<EditText>(Resource.Id.DeleteID);
            Button deleteButton = FindViewById<Button>(Resource.Id.DeleteTournament);


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

            // Show all tournaments button
            showTournaments.Click += (object sender, EventArgs e) =>
            {
                // Set our view to TournamentsAll
                SetContentView(Resource.Layout.TournamentsAll);
            };

            // Show results button
            showTournaments.Click += (object sender, EventArgs e) =>
            {
                // Set our view to TournamentResults
                SetContentView(Resource.Layout.TournamentResults);
            };

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

    }
}