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
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Android.Graphics;

namespace Foosball
{
    [Activity(Label = "TournamentActivity")]
    public class TournamentActivity : Activity
    {
        /*public static string myCon = "Server=tcp:myserver-20171207.database.windows.net,1433;" +
               "Initial Catalog=foosballDatabase;" +
               "User ID=ServerAdmin963;" +
               "Password=Slapt4z0d1s;" +
               "MultipleActiveResultSets=False;";*/
        string myCon = Strings.myCon;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here

            // Set our view to Tournamet
            SetContentView(Resource.Layout.Tournament);

            // Update existing player
            Button updateButton = FindViewById<Button>(Resource.Id.UpdateButton);
            EditText ID = FindViewById<EditText>(Resource.Id.ID);
            EditText Name = FindViewById<EditText>(Resource.Id.Name);

            // Show all tournaments
            Button showTournaments = FindViewById<Button>(Resource.Id.ShowTournaments);

            // Show results
            Button showResults = FindViewById<Button>(Resource.Id.ShowResults);

            // Add new player
            Button insertButton = FindViewById<Button>(Resource.Id.InsertButton);

            //Add player to tournament
            Button fight = FindViewById<Button>(Resource.Id.FightButton);
            EditText tournamentFightID = FindViewById<EditText>(Resource.Id.TournamentFightID);
            EditText leftID = FindViewById<EditText>(Resource.Id.LeftID);
            EditText rightID = FindViewById<EditText>(Resource.Id.RightID);

            // Show players
            Button showPlayers = FindViewById<Button>(Resource.Id.ShowPlayersButton);   

            // Delete tournament
            EditText tournamentID = FindViewById<EditText>(Resource.Id.TournamentID);
            Button deleteTournament = FindViewById<Button>(Resource.Id.DeleteTournament);

            // Create a new tournament
            Button insertTournament = FindViewById<Button>(Resource.Id.InsertTournament);
            EditText creatorsID = FindViewById<EditText>(Resource.Id.CreatorsID);

            // Start tournament
            Button startTournament = FindViewById<Button>(Resource.Id.StartTournament);

            int id;
            int idTournament;
            int idCreator;
            int idLeft;
            int idRight;

            // Test data
            int score = 69;
            int idGame = 8;
            int idT = 2;
            string nameT = "TestWinner";

            // Start tournament
            startTournament.Click += (object sender, EventArgs e) =>
            {
                // TO-DO: tourmanet start
                SetContentView(Resource.Layout.TournamentResults);

                GameSelect();
            };

            // Add player to tournament
            fight.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(tournamentFightID.Text, out idTournament))
                {
                    try
                    {
                        // LEFT
                        if (Int32.TryParse(leftID.Text, out idLeft))
                        {
                            try
                            {
                                AddLeftPlayerToTournament(idTournament, idLeft);
                                Toast.MakeText(this, string.Format("Inserted successfully at id {0}", leftID.Text), ToastLength.Long).Show(); 
                            }
                            catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", leftID.Text), ToastLength.Long).Show(); }
                            
                        }
                        else Toast.MakeText(this, string.Format("Invalid ID: {0}", leftID.Text), ToastLength.Long).Show();

                        // RIGHT
                        if (Int32.TryParse(rightID.Text, out idRight))
                        {
                            try
                            {
                                AddRightPlayerToTournament(idTournament, idRight);
                                Toast.MakeText(this, string.Format("Inserted successfully at id {0}", rightID.Text), ToastLength.Long).Show();
                            }
                            catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", rightID.Text), ToastLength.Long).Show(); }
                        }
                        else Toast.MakeText(this, string.Format("Invalid ID: {0}", rightID.Text), ToastLength.Long).Show();
                    }
                    catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", tournamentFightID.Text), ToastLength.Long).Show(); }
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", tournamentFightID.Text), ToastLength.Long).Show();
            };

            // Update existing player
            updateButton.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(ID.Text, out id))
                {
                    try
                    {
                        string name = Name.Text.ToString();
                        UpdateExisting(id, name);
                        Toast.MakeText(this, string.Format("Updated successfully at id {0}", ID.Text), ToastLength.Long).Show();
                    }
                    catch (Exception) { Toast.MakeText(this, string.Format("Error updating at id {0}", ID.Text), ToastLength.Long).Show(); }
                    
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", ID.Text), ToastLength.Long).Show();
            };

            // Add new player
            insertButton.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(ID.Text, out id))
                {
                    try
                    {
                        string name = Name.Text.ToString();
                        AddNew(id, name);
                        Toast.MakeText(this, string.Format("Inserted successfully at id {0}", ID.Text), ToastLength.Long).Show();
                    }
                    catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", ID.Text), ToastLength.Long).Show(); }
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", ID.Text), ToastLength.Long).Show();
            };

            // Show players
            showPlayers.Click += (object sender, EventArgs e) =>
            {
                SetContentView(Resource.Layout.TournamentResults);

                PlayersSelect();
            };

            // Delete tournament
            deleteTournament.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(tournamentID.Text, out id))
                {
                    try
                    {
                        DeleteTournament(id);
                        Toast.MakeText(this, string.Format("Deleted successfully at id {0}", tournamentID.Text), ToastLength.Long).Show();
                    }
                    catch (Exception) { Toast.MakeText(this, string.Format("Error deleting at id {0}", tournamentID.Text), ToastLength.Long).Show(); }
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", tournamentID.Text), ToastLength.Long).Show();
            };

            // Add new tournament
            insertTournament.Click += (object sender, EventArgs e) =>
            {
                if (Int32.TryParse(tournamentID.Text, out idTournament))
                {
                    try
                    {
                        if (Int32.TryParse(creatorsID.Text, out idCreator))
                        {
                            try
                            {
                                AddNewTournament(idTournament, idCreator);
                                Toast.MakeText(this, string.Format("Inserted successfully at id {0}", creatorsID.Text), ToastLength.Long).Show();
                            }
                            catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", creatorsID.Text), ToastLength.Long).Show(); }
                        }
                        else Toast.MakeText(this, string.Format("Invalid ID: {0}", creatorsID.Text), ToastLength.Long).Show();
                    }
                    catch (Exception) { Toast.MakeText(this, string.Format("Error inserting at id {0}", tournamentID.Text), ToastLength.Long).Show(); }
                }
                else Toast.MakeText(this, string.Format("Invalid ID: {0}", tournamentID.Text), ToastLength.Long).Show();
            };

            // Show all tournaments button
            showTournaments.Click += (object sender, EventArgs e) =>
            {
                // Set our view to TournamentsAll
                SetContentView(Resource.Layout.TournamentResults);

                TournamentSelect();
            };

            // Show results button
            showResults.Click += (object sender, EventArgs e) =>
            {
                SetContentView(Resource.Layout.TournamentResults);

                ResultsSelect();
            };
        }

        public void GameSelect()
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL select command
                SqlCommand select = new SqlCommand("SELECT GameID FROM tabRightTournamentPlayer", cn);

                da.SelectCommand = select;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabRightTournamentPlayer");

                TextView tournamentResults = FindViewById<TextView>(Resource.Id.TournamentResultsText);
                tournamentResults.MovementMethod = new ScrollingMovementMethod();

                tournamentResults.Append("GameID" + "\n");

                //  TO-DO: change 20 to something more meaningful
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        string collumn1 = (ds.Tables[0].Rows[i]["GameID"]).ToString();

                        tournamentResults.Append(collumn1 + "\n");
                    }

                    catch (Exception) { tournamentResults.Append("\nROWS: " + i + "\n"); break; }
                }
            }
        }

        public void UpdateWinner(int idT, string nameT)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(Strings.selectForUpdateWinner, cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL update command
                SqlCommand update = new SqlCommand();
                update.Connection = cn;
                update.CommandType = CommandType.Text;
                update.CommandText = Strings.updateForUpdateWinner;

                //Establish existing (those used in SELECT)
                update.Parameters.Add(new SqlParameter("@TID", SqlDbType.Int, 50, "TournamentID"));
                update.Parameters.Add(new SqlParameter("@W", SqlDbType.NVarChar, 50, "Winner"));

                //Data adapter (using statement)
                da.UpdateCommand = update;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabTournament");

                //Establish the name and new data for the column to be updated
                ds.Tables[0].Rows[idT - 1]["Winner"] = nameT;

                //Update done
                da.Update(ds.Tables[0]);
            }
        }

        public void UpdateLeftScore(int idGame, int score)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(Strings.selectForUpdateLeftScore, cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL update command
                SqlCommand update = new SqlCommand();
                update.Connection = cn;
                update.CommandType = CommandType.Text;
                update.CommandText = Strings.updateForUpdateLeftScore;

                //Establish existing (those used in SELECT)
                update.Parameters.Add(new SqlParameter("@GID", SqlDbType.Int, 50, "GameID"));
                update.Parameters.Add(new SqlParameter("@S", SqlDbType.Int, 50, "Score"));

                //Data adapter (using statement)
                da.UpdateCommand = update;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabLeftTournamentPlayer");

                //Establish the name and new data for the column to be updated
                ds.Tables[0].Rows[idGame - 1]["Score"] = score;

                //Update done
                da.Update(ds.Tables[0]);
            }
        }

        public void UpdateRightScore(int idGame, int score)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT GameID, Score FROM tabRightTournamentPlayer", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL update command
                SqlCommand update = new SqlCommand();
                update.Connection = cn;
                update.CommandType = CommandType.Text;
                update.CommandText = "UPDATE tabRightTournamentPlayer SET Score = @S WHERE GameID = @GID";

                //Establish existing (those used in SELECT)
                update.Parameters.Add(new SqlParameter("@GID", SqlDbType.Int, 50, "GameID"));
                update.Parameters.Add(new SqlParameter("@S", SqlDbType.Int, 50, "Score"));

                //Data adapter (using statement)
                da.UpdateCommand = update;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabRightTournamentPlayer");

                //Establish the name and new data for the column to be updated
                ds.Tables[0].Rows[idGame - 1]["Score"] = score;

                //Update done
                da.Update(ds.Tables[0]);
            }
        }

        public void AddRightPlayerToTournament(int idTournament, int idRight)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT RightPlayerID, TournamentID FROM tabRightTournamentPlayer", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL insert command
                SqlCommand insert = new SqlCommand();
                insert.Connection = cn;
                insert.CommandType = CommandType.Text;
                insert.CommandText = "INSERT INTO tabRightTournamentPlayer (RightPlayerID, TournamentID) VALUES (@RID, @TID)";

                //Establish existing (those used in SELECT)
                insert.Parameters.Add(new SqlParameter("@RID", SqlDbType.Int, 50, "RightPlayerID"));
                insert.Parameters.Add(new SqlParameter("@TID", SqlDbType.Int, 50, "TournamentID"));

                //Data adapter (using statement)
                da.InsertCommand = insert;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabRightTournamentPlayer");

                //Establish the new data for the row to be inserted
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["RightPlayerID"] = idRight;
                newRow["TournamentID"] = idTournament;
                ds.Tables[0].Rows.Add(newRow);

                //Insert done
                da.Update(ds.Tables[0]);
            }     
        }

        public void AddLeftPlayerToTournament(int idTournament, int idLeft)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT LeftPlayerID, TournamentID FROM tabLeftTournamentPlayer", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL insert command
                SqlCommand insert = new SqlCommand();
                insert.Connection = cn;
                insert.CommandType = CommandType.Text;
                insert.CommandText = "INSERT INTO tabLeftTournamentPlayer (LeftPlayerID, TournamentID) VALUES (@LID, @TID)";

                //Establish existing (those used in SELECT)
                insert.Parameters.Add(new SqlParameter("@LID", SqlDbType.Int, 50, "LeftPlayerID"));
                insert.Parameters.Add(new SqlParameter("@TID", SqlDbType.Int, 50, "TournamentID"));

                //Data adapter (using statement)
                da.InsertCommand = insert;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabLeftTournamentPlayer");

                //Establish the new data for the row to be inserted
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["LeftPlayerID"] = idLeft;
                newRow["TournamentID"] = idTournament;
                ds.Tables[0].Rows.Add(newRow);

                //Insert done
                da.Update(ds.Tables[0]);
            }
        }

        public void AddNewTournament(int idTournament, int idCreator)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT TournamentID, UserID FROM tabTournament", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL insert command
                SqlCommand insert = new SqlCommand();
                insert.Connection = cn;
                insert.CommandType = CommandType.Text;
                insert.CommandText = "INSERT INTO tabTournament (TournamentID, UserID) VALUES (@TID, @UID)";

                //Establish existing (those used in SELECT)
                insert.Parameters.Add(new SqlParameter("@TID", SqlDbType.Int, 50, "TournamentID"));
                insert.Parameters.Add(new SqlParameter("@UID", SqlDbType.Int, 10, "UserID"));

                //Data adapter (using statement)
                da.InsertCommand = insert;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabTournament");

                //Establish the new data for the row to be inserted
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["TournamentID"] = idTournament;
                newRow["UserID"] = idCreator;
                ds.Tables[0].Rows.Add(newRow);

                //Insert done
                da.Update(ds.Tables[0]);
            }
        }

        public void PlayersSelect()
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL select command
                SqlCommand select = new SqlCommand("SELECT UserID, Name FROM tabUser", cn);

                da.SelectCommand = select;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabUser");

                TextView tournamentResults = FindViewById<TextView>(Resource.Id.TournamentResultsText);
                tournamentResults.MovementMethod = new ScrollingMovementMethod();

                tournamentResults.Append("UserID" + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Name" + "\n");

                //  TO-DO: change 20 to something more meaningful
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        string collumn1 = (ds.Tables[0].Rows[i]["UserID"]).ToString();
                        string collumn2 = (ds.Tables[0].Rows[i]["Name"]).ToString();

                        tournamentResults.Append(collumn1 + string.Empty.PadLeft(6, '\t'));
                        tournamentResults.Append(collumn2 + "\n");
                    }

                    catch (Exception) { tournamentResults.Append("\nROWS: " + i + "\n"); break; }
                }
            }
        }

        public void AddNew(int id, string name)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT UserID, Name FROM tabUser", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL insert command
                SqlCommand insert = new SqlCommand();
                insert.Connection = cn;
                insert.CommandType = CommandType.Text;
                insert.CommandText = "INSERT INTO tabUser (UserID, Name) VALUES (@ID, @N)";

                //Establish existing (those used in SELECT)
                insert.Parameters.Add(new SqlParameter("@N", SqlDbType.NVarChar, 50, "Name"));
                insert.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 10, "UserID"));

                //Data adapter (using statement)
                da.InsertCommand = insert;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabUser");

                //Establish the new data for the row to be inserted
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["UserID"] = id;
                newRow["Name"] = name;
                ds.Tables[0].Rows.Add(newRow);

                //Insert done
                da.Update(ds.Tables[0]);
            }
        }

        public void DeleteTournament(int id)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT TournamentID, UserID, Winner FROM tabTournament", cn))
            {
                // Establish connection string
                cn.ConnectionString = myCon;

                //Establish SQL select command
                SqlCommand delete = new SqlCommand();
                delete.Connection = cn;
                delete.CommandType = CommandType.Text;
                delete.CommandText = "DELETE FROM tabTournament WHERE TournamentID = @ID";

                delete.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 50, "TournamentId"));
                da.DeleteCommand = delete;

                //Establish table to be updated
                DataSet ds = new DataSet();
                da.Fill(ds, "tabTournament");

                ds.Tables[0].Rows[id - 1].Delete();

                //Delete done
                da.Update(ds.Tables[0]);
            }
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

                tournamentResults.Append("Tournament"   + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Game"         + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Left Score"   + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Right Score"  + "\n");

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

                tournamentResults.Append("Tournament"   + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("User"         + string.Empty.PadLeft(2, '\t'));
                tournamentResults.Append("Winner" + "\n");

                //  TO-DO: change 10 to something more meaningful                
                for (int i=0; i<10 ; i++)
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