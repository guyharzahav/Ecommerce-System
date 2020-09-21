using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer-logout-31 </req>
    [TestClass]
    public class LogoutStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];

        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void ValidLogoutUserTestTest()
        {
            //pre-condition
            Login(username, password);
            Assert.IsTrue(Logout(username).Item1, Logout(username).Item2);
        }

        [TestMethod]
        //sad
        public void NotLoggedInUserTest()
        {
            Register(username, password);
            Assert.IsFalse(Logout(username).Item1, Logout(username).Item2);
        }

        [TestMethod]
        //bad
        public void TwiceLogoutTest()
        {
            Register(username, password);
            Login(username, password);
            Assert.IsTrue(Logout(username).Item1, Logout(username).Item2);
            Assert.IsFalse(Logout(username).Item1, Logout(username).Item2);
        }
    }
}
