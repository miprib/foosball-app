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
    public class AddNamesTests
    {
        [TestMethod]
        public void Ret_namesTest()
        {
            // Arrange
            String regexPattern = @"(\w)\^(\w)";

            // Act
            String names = AddNames.Ret_names("let", "play");

            Boolean a = Regex.IsMatch(names, regexPattern);

            // Assert
            Assert.IsTrue(a);
        }
    }
}