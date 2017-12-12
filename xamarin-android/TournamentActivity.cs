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
using Android.Text.Method;

namespace xamarin_android
{
    [Activity(Label = "TournamentActivity")]
    public class TournamentActivity : Activity
    {
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
                    UpdateExisting(id, name);
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", updateID.Text), ToastLength.Long).Show();
            };

            // Show all tournaments button
            showTournaments.Click += (object sender, EventArgs e) =>
            {
                // Set our view to TournamentsAll
                SetContentView(Resource.Layout.TournamentResults);

                TournamentSelect();
            };

            // Show results button
            showTournaments.Click += (object sender, EventArgs e) =>
            {
                // Set our view to TournamentResults
                SetContentView(Resource.Layout.TournamentResults);

                ResultsSelect();
            };
        }

        public void ResultsSelect()
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL select command
                SqlCommand select = new SqlCommand("SELECT TournamentID, GameID, LeftScore, RightScore FROM viewResults", cn);

                da.SelectCommand = select;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "viewResults");

                TextView tournamentResults = FindViewById<TextView>(Resource.Id.TournamentResultsText);
                tournamentResults.MovementMethod = new ScrollingMovementMethod();

                tournamentResults.Append("Tournament" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Game" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Left Score" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Right Score" + "\n");

                //  TO-DO: change 20 to something more meaningful
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        string collumn1 = (ds.Tables[0].Rows[i]["TournamentID"]).ToString();
                        string collumn2 = (ds.Tables[0].Rows[i]["GameID"]).ToString();
                        string collumn3 = (ds.Tables[0].Rows[i]["LeftScore"]).ToString();
                        string collumn4 = (ds.Tables[0].Rows[i]["RightScore"]).ToString();

                        tournamentResults.Append(collumn1 + string.Empty.PadLeft(9, '\t'));
                        tournamentResults.Append(collumn2 + string.Empty.PadLeft(5, '\t'));
                        tournamentResults.Append(collumn3 + string.Empty.PadLeft(8, '\t'));
                        tournamentResults.Append(collumn4 + "\n");
                    }

                    catch (Exception) { tournamentResults.Append("\nROWS: " + i + "\n"); break; }
                }
            }
        }

        public void TournamentSelect()
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL select command
                SqlCommand select = new SqlCommand("SELECT TournamentID, UserID, Winner FROM tabTournament", cn);

                da.SelectCommand = select;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabTournament");

                TextView tournamentResults = FindViewById<TextView>(Resource.Id.TournamentResultsText);
                tournamentResults.MovementMethod = new ScrollingMovementMethod();

                tournamentResults.Append("Tournament" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("User" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Winner" + "\n");

                //  TO-DO: change 10 to something more meaningful                
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        string collumn1 = (ds.Tables[0].Rows[i]["TournamentID"]).ToString();
                        string collumn2 = (ds.Tables[0].Rows[i]["UserID"]).ToString();
                        string collumn3 = (ds.Tables[0].Rows[i]["Winner"]).ToString();

                        tournamentResults.Append(collumn1 + string.Empty.PadLeft(9, '\t'));
                        tournamentResults.Append(collumn2 + string.Empty.PadLeft(4, '\t'));
                        tournamentResults.Append(collumn3 + "\n");
                    }

                    catch (Exception) { tournamentResults.Append("\nROWS: " + i + "\n"); break; }
                }
            }
        }

        public void UpdateExisting(int id, string name)
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