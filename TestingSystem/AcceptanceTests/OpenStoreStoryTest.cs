using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--open-store-32 </req>
    [TestClass]
    public class OpenStoreStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];

        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void OpenValidStoreDetailsTest() 
        {
            Assert.AreNotEqual(-1, OpenStore(username).Item1);
        }

        [TestMethod]
        //sad
        public void OpenInvalidStoreDetailsTest()
        {
            Assert.AreEqual(-1, OpenStore("hello").Item1);
        }

        [TestMethod]
        //bad
        public void OpenStoreWIthInvalisUsernameTest()
        {
            Assert.AreEqual(-1, OpenStore("\n\n\n\\t#@$#$@").Item1);
        }
    }
}
