using Microsoft.VisualStudio.TestTools.UnitTesting;
using Foosball;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Foosball.Tests
{
    [TestClass()]
    public class TournamentActivityTests
    {
        [TestMethod()]
        public void ConnectionTest()
        {
            // Arrange
            String regexPattern = @"^([\w\=\:\-\.\,\;\ ]+)(myserver-20171207.database.windows.net)([\w\=\:\-\.\,\;\ ]+)$";

            // Act
            String connection = Strings.myCon;

            // Assert
            Assert.IsTrue(Regex.IsMatch(connection, regexPattern));
        }

        [TestMethod()]
        public void SelectForUpdateWinnerTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT TournamentID, Winner FROM tabTournament)$";

            // Act
            String select = Strings.selectForUpdateWinner;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void UpdateForUpdateWinnerTest()
        {
            // Arrange
            String regexPattern = @"^(UPDATE tabTournament SET Winner = @W WHERE TournamentID = @TID)$";

            // Act
            String update = Strings.updateForUpdateWinner;

            // Assert
            Assert.IsTrue(Regex.IsMatch(update, regexPattern));
        }

        [TestMethod()]
        public void UpdateForUpdateLeftScoreTest()
        {
            // Arrange
            String regexPattern = @"^(UPDATE tabTournament SET Winner = @W WHERE TournamentID = @TID)$";

            // Act
            String update = Strings.updateForUpdateWinner;

            // Assert
            Assert.IsTrue(Regex.IsMatch(update, regexPattern));
        }

        [TestMethod()]
        public void SelectForUpdateLeftScoreTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT GameID, Score FROM tabLeftTournamentPlayer)$";

            // Act
            String select = Strings.selectForUpdateLeftScore;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void UpdateForUpdateRightScoreTest()
        {
            // Arrange
            String regexPattern = @"^(UPDATE tabRightTournamentPlayer SET Score = @S WHERE GameID = @GID)$";

            // Act
            String update = Strings.updateForUpdateRightScore;

            // Assert
            Assert.IsTrue(Regex.IsMatch(update, regexPattern));
        }

        [TestMethod()]
        public void SelectForUpdateRightScoreTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT GameID, Score FROM tabRightTournamentPlayer)$";

            // Act
            String select = Strings.selectForUpdateRightScore;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void SelectForAddRightPlayerToTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT RightPlayerID, TournamentID FROM tabRightTournamentPlayer)$";

            // Act
            String select = Strings.selectForAddRightPlayerToTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void InsertForAddRightPlayerToTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(INSERT INTO tabRightTournamentPlayer \(RightPlayerID, TournamentID\) VALUES \(@RID, @TID\))$";

            // Act
            String insert = Strings.insertForAddRightPlayerToTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(insert, regexPattern));
        }

        [TestMethod()]
        public void SelectForAddLeftPlayerToTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT LeftPlayerID, TournamentID FROM tabLeftTournamentPlayer)$";

            // Act
            String select = Strings.selectForAddLeftPlayerToTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void InsertForAddLeftPlayerToTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(INSERT INTO tabLeftTournamentPlayer \(LeftPlayerID, TournamentID\) VALUES \(@LID, @TID\))$";

            // Act
            String insert = Strings.insertForAddLeftPlayerToTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(insert, regexPattern));
        }

        [TestMethod()]
        public void SelectForAddNewTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT TournamentID, UserID FROM tabTournament)$";

            // Act
            String select = Strings.selectForAddNewTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void InsertForAddNewTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(INSERT INTO tabTournament \(TournamentID, UserID\) VALUES \(@TID, @UID\))$";

            // Act
            String insert = Strings.insertForAddNewTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(insert, regexPattern));
        }

        [TestMethod()]
        public void PlayersSelectTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT UserID, Name FROM tabUser)$";

            // Act
            String select = Strings.selectForPlayersSelect;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void SelectForAddNewTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT UserID, Name FROM tabUser)$";

            // Act
            String select = Strings.selectForAddNew;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void InsertForAddNewTest()
        {
            // Arrange
            String regexPattern = @"^(INSERT INTO tabUser \(UserID, Name\) VALUES \(@ID, @N\))$";

            // Act
            String insert = Strings.insertForAddNew;

            // Assert
            Assert.IsTrue(Regex.IsMatch(insert, regexPattern));
        }

        [TestMethod()]
        public void SelectForDeleteTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT TournamentID, UserID, Winner FROM tabTournament)$";

            // Act
            String select = Strings.selectForDeleteTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void DeleteForDeleteTournamentTest()
        {
            // Arrange
            String regexPattern = @"^(DELETE FROM tabTournament WHERE TournamentID = @ID)$";

            // Act
            String delete = Strings.deleteForDeleteTournament;

            // Assert
            Assert.IsTrue(Regex.IsMatch(delete, regexPattern));
        }

        [TestMethod()]
        public void ResultsSelectTest()
        {        
            // Arrange
            String regexPattern = @"^(SELECT TournamentID, GameID, LeftScore, RightScore FROM viewResults)$";

            // Act
            String select = Strings.selectForResultsSelect;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void TournamentSelectTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT TournamentID, UserID, Winner FROM tabTournament)$";

            // Act
            String select = Strings.selectForTournamentSelect;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void SelectForUpdateExistingTest()
        {
            // Arrange
            String regexPattern = @"^(SELECT UserID, Name FROM tabUser)$";

            // Act
            String select = Strings.selectForUpdateExisting;

            // Assert
            Assert.IsTrue(Regex.IsMatch(select, regexPattern));
        }

        [TestMethod()]
        public void UpdateForUpdateExistingTest()
        {
            // Arrange
            String regexPattern = @"^(UPDATE tabUser SET Name = @N WHERE UserID = @ID)$";

            // Act
            String update = Strings.updateForUpdateExisting;

            // Assert
            Assert.IsTrue(Regex.IsMatch(update, regexPattern));
        }
    }
}