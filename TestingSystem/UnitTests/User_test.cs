using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests.User_test
{

    [TestClass]
    public class User_test
    {
        private UserManager UM;
        [TestInitialize]
        public void TestInitialize()
        {
            UM = UserManager.Instance;
            UM.Register("GoodUser", "Test1");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            UM.cleanup();
        }
        /// <function cref ="eCommerce_14a.UserManager.RegisterMaster(string,string)
        [TestMethod]
        public void MasterRegistrationTestNullArgs()
        {
            Assert.IsFalse(UM.RegisterMaster(null, "Test1").Item1);
            Assert.IsFalse(UM.RegisterMaster("AAAAA", null).Item1);
        }
        [TestMethod]
        public void MasterRegistrationTestBlankArgs()
        {
            Assert.IsFalse(UM.RegisterMaster("AAAAA", "").Item1);
            Assert.IsFalse(UM.RegisterMaster("", "Test1").Item1);

        }
        [TestMethod]
        public void MasterRegistrationTwiceTest()
        {
            Assert.IsTrue(UM.RegisterMaster("test", "Test1").Item1);
            Assert.IsTrue(UM.GetUser("test").isSystemAdmin());
            Assert.IsTrue(UM.isUserExist("test"));
            Assert.IsFalse(UM.RegisterMaster("test", "Test1").Item1);
        }
        [TestMethod]
        public void MasterRegistrationTestUserFailed()
        {
            Assert.IsFalse(UM.RegisterMaster("12", "Test1").Item1);
            Assert.IsFalse(UM.RegisterMaster("$(*$#(*$", "Test1").Item1);
        }
        /// <function cref ="eCommerce_14a.UserManager.Register(string,string)
        [TestMethod]
        public void RegistrationTestNullArgs()
        {
            Assert.IsFalse(UM.Register("AAAAA", null).Item1);
            Assert.IsFalse(UM.Register(null, "Test1").Item1);
        }
        [TestMethod]
        public void RegistrationTestBlankArgs()
        {
            Assert.IsFalse(UM.Register("AAAAA", "").Item1);
            Assert.IsFalse(UM.Register("", "Test1").Item1);
        }
        [TestMethod]
        public void RegistrationTwiceTest()
        {
            Assert.IsTrue(UM.Register("test", "Test1").Item1);
            Assert.IsTrue(UM.isUserExist("test"));
            Assert.IsFalse(UM.Register("test", "Test1").Item1);
        }
        [TestMethod]
        public void RegistrationTestUserFailed()
        {
            Assert.IsFalse(UM.Register("12", "Test1").Item1);
            Assert.IsFalse(UM.Register("AAAAAAAAAAAAAAAAAAAAAAAAAAAA", "Test1").Item1);
            Assert.IsFalse(UM.Register("$(*$#(*$", "Test1").Item1);
            Assert.IsFalse(UM.Register("AA43A$(*$#(*$", "Test1").Item1);
        }
        /// <function cref ="eCommerce_14a.UserManager.Login(string,string,bool)
        //("GoodUser", "Test1")
        [TestMethod]
        public void LoginTestNullArgs()
        {
            Assert.IsFalse(UM.Login("GoodUser", null).Item1);
            Assert.IsFalse(UM.Login(null, null).Item1);
        }
        [TestMethod]
        public void LoginTestBlankArgs()
        {
            Assert.IsFalse(UM.Login("", "AA").Item1);
            Assert.IsFalse(UM.Login("test1", "").Item1);
        }
        [TestMethod]
        public void LoginTestNoUser()
        {
            Assert.IsFalse(UM.Login("AAAA", "AA").Item1);
            Assert.IsFalse(UM.Login("test1", "Test1").Item1);
        }
        public void LoginAgainTest()
        {
            Assert.IsTrue(UM.Login("GoodUser", "Test1").Item1);
            Assert.IsTrue(UM.GetAtiveUser("GoodUser").LoggedStatus());
            Assert.IsNotNull(UM.GetAtiveUser("GoodUser"));
            Assert.IsFalse(UM.Login("GoodUser", "Test1").Item1);
        }
        public void LoginWithWrongCredentialsTest()
        {
            Assert.IsTrue(UM.Login("GoodUser", "Test").Item1);
            Assert.IsFalse(UM.GetAtiveUser("GoodUser").LoggedStatus());
            Assert.IsNull(UM.GetAtiveUser("GoodUser"));
        }
        /// <function cref ="eCommerce_14a.UserManager.Login(string,string,bool)
        [TestMethod]
        public void LoginAsGuestTest()
        {
            Assert.IsTrue(UM.Login("", "", true).Item1);
            Assert.IsTrue(UM.GetAtiveUser("Guest2").LoggedStatus());
            Assert.IsTrue(UM.GetAtiveUser("Guest2").isguest());

        }
        /// <function cref ="eCommerce_14a.UserManager.Logout(string)
        [TestMethod]
        public void LogoutTestNullArgs()
        {
            UM.Login("GoodUser", "Test1");
            Assert.IsFalse(UM.Logout(null).Item1);
        }
        [TestMethod]
        public void LogoutTestBlankArgs()
        {
            UM.Login("GoodUser", "Test1");
            Assert.IsFalse(UM.Logout("").Item1);
        }
        [TestMethod]
        public void LogoutTestNotLoggedIn()
        {
            Assert.IsFalse(UM.Logout("GoodUser").Item1);
        }
        [TestMethod]
        public void LogoutTestDoubleLogout()
        {
            UM.Login("GoodUser", "Test1");
            Assert.IsTrue(UM.Logout("GoodUser").Item1);
            Assert.IsFalse(UM.Logout("GoodUser").Item1);
        }
        [TestMethod]
        public void LogoutTestGuestUser()
        {
            UM.Login("", "",true);
            Assert.IsFalse(UM.Logout("Guest1").Item1);
        }
        [TestMethod]
        public void LogoutTest()
        {
            UM.Login("GoodUser", "Test1");
            Assert.IsTrue(UM.Logout("GoodUser").Item1);
            Assert.IsFalse(UM.GetUser("GoodUser").LoggedStatus());
            Assert.IsNull(UM.GetAtiveUser("GoodUser"));
        }
    }
}
