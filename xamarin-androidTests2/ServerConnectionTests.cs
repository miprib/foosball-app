using Microsoft.VisualStudio.TestTools.UnitTesting;
using xamarin_android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace xamarin_android.Tests
{
    [TestClass()]
    public class ServerConnectionTests
    {
        [TestMethod]
        public void UrlTest()
        {
            // Arrange
            String regexPattern = @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";

            // Act
            String url = ServerConnection.url;

            // Assert
            Assert.IsTrue(Regex.IsMatch(url, regexPattern));
        }

        [TestMethod]
        public void PortTest()
        {
            // Arrange
            String regexPattern = @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)+([\w\-\.]+[^#?\s]+)(5000)(.*)?(#[\w\-]+)?$";

            // Act
            String url = ServerConnection.url;

            // Assert
            Assert.IsTrue(Regex.IsMatch(url, regexPattern));
        }
    }
}