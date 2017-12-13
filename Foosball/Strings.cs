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

namespace Foosball
{
    public static class Strings
    {
        public static string myCon = "Server=tcp:myserver-20171207.database.windows.net,1433;Initial Catalog=foosballDatabase;User ID=ServerAdmin963;Password=Slapt4z0d1s;MultipleActiveResultSets=False;";

        public static string selectForUpdateWinner = "SELECT TournamentID, Winner FROM tabTournament";
        public static string updateForUpdateWinner = "UPDATE tabTournament SET Winner = @W WHERE TournamentID = @TID";

        public static string selectForUpdateLeftScore = "SELECT GameID, Score FROM tabLeftTournamentPlayer";
        public static string updateForUpdateLeftScore = "UPDATE tabLeftTournamentPlayer SET Score = @S WHERE GameID = @GID";

        public static string selectForUpdateRightScore = "SELECT GameID, Score FROM tabRightTournamentPlayer";
        public static string updateForUpdateRightScore = "UPDATE tabRightTournamentPlayer SET Score = @S WHERE GameID = @GID";

        public static string selectForAddRightPlayerToTournament = "SELECT RightPlayerID, TournamentID FROM tabRightTournamentPlayer";
        public static string insertForAddRightPlayerToTournament = "INSERT INTO tabRightTournamentPlayer (RightPlayerID, TournamentID) VALUES (@RID, @TID)";

        public static string selectForAddLeftPlayerToTournament = "SELECT LeftPlayerID, TournamentID FROM tabLeftTournamentPlayer";
        public static string insertForAddLeftPlayerToTournament = "INSERT INTO tabLeftTournamentPlayer (LeftPlayerID, TournamentID) VALUES (@LID, @TID)";

        public static string selectForAddNewTournament = "SELECT TournamentID, UserID FROM tabTournament";
        public static string insertForAddNewTournament = "INSERT INTO tabTournament (TournamentID, UserID) VALUES (@TID, @UID)";

        public static string selectForPlayersSelect = "SELECT UserID, Name FROM tabUser";

        public static string selectForAddNew = "SELECT UserID, Name FROM tabUser";
        public static string insertForAddNew = "INSERT INTO tabUser (UserID, Name) VALUES (@ID, @N)";

        public static string selectForDeleteTournament = "SELECT TournamentID, UserID, Winner FROM tabTournament";
        public static string deleteForDeleteTournament = "DELETE FROM tabTournament WHERE TournamentID = @ID";

        public static string selectForResultsSelect = "SELECT TournamentID, GameID, LeftScore, RightScore FROM viewResults";

        public static string selectForTournamentSelect = "SELECT TournamentID, UserID, Winner FROM tabTournament";

        public static string selectForUpdateExisting = "SELECT UserID, Name FROM tabUser";
        public static string updateForUpdateExisting = "UPDATE tabUser SET Name = @N WHERE UserID = @ID";

    }
}