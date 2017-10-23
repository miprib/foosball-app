using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Vid.Tests
{
    [TestClass]
    public class GameListTests
    {
        [TestMethod]
        public void FilePathTest()
        {
            // Arrange
            String regexPattern = @"^([\w]\:)(\\[A-z_\-\s0-9\.]+)+\.(txt)$";

            // Act
            String actual = GameList.path;

            // Assert
            Assert.IsTrue(Regex.IsMatch(actual, regexPattern));
        }

        [TestMethod]
        public void JsonTest()
        {
            // Arrange
            String regexPattern = @"(\[{.*}\])";

            // Act
            String path = GameList.path;
            String json = File.ReadAllText(path);

            // Assert
            Assert.IsTrue(Regex.IsMatch(json, regexPattern));
        }

        [TestMethod]
        public void IdTest()
        {
            // Arrange
            bool check = true;

            GameList gameList = GameList.NewInstance();
            int newIndex = gameList.NextId();

            // Act
            foreach(Game game in gameList)
            {
                if(newIndex == game.id)
                {
                    check = false;
                    break;
                }
            }

            // Assert
            Assert.IsTrue(check);
        }
    }
}