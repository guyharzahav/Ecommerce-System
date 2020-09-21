using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-a-store-manager-45 </req>
    [TestClass]
    public class AppointingManagerStoryTest : SystemTrackTest
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
        }

        [TestMethod]
        //happy
        public void AppointValidManagerTest()
        {
            Assert.IsTrue(AppointStoreManage(username1, username2, storeID).Item1, AppointStoreManage(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void AppointAlreadyManagerTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsFalse(AppointStoreManage(username1, username2, storeID).Item1, AppointStoreManage(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //bad
        public void AppointAlreadySellerTest()
        {
            Assert.IsFalse(AppointStoreManage(username1, username1, storeID).Item1, AppointStoreManage(username1, username1, storeID).Item2);
        }
    }
}
