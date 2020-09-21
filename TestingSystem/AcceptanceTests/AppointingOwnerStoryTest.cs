using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-store-owner-43 </req>
    [TestClass]
    public class AppointingOwnerStoryTest : SystemTrackTest
    {
        string username1 = UserGenerator.GetValidUsernames()[0];
        string password1 = UserGenerator.GetPasswords()[0];
        string username2 = UserGenerator.GetValidUsernames()[1];
        string password2 = UserGenerator.GetPasswords()[1];
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            Register(username1, password1);
            Login(username1, password1);
            Register(username2, password2);
            Login(username2, password2);
            storeID = OpenStore(username1).Item1;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
            //Publisher.Instance.cleanup();
        }

        [TestMethod]
        //happy
        public void AppointValidOwnerTest() 
        {
            Assert.IsTrue(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void AppointAlreadyOwnerTest()
        {
            AppointStoreOwner(username1, username2, storeID);
            Assert.IsFalse(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //bad
        public void AppointAlreadyManagerTest()
        {
            Assert.IsTrue(AppointStoreManage(username1, username2, storeID).Item1);
            Assert.IsTrue(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }
    }
}
