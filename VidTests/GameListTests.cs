using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            GameList.NewInstance();
            String actual = GameList.path;

            // Assert
            Assert.IsTrue(Regex.IsMatch(actual, regexPattern));
        }

    }
}