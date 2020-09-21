using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-demote-store-manager-47 </req>
    [TestClass]
    public class DemoteManagerStroyTest : SystemTrackTest
    {
        string username1 = UserGenerator.GetValidUsernames()[0];
        string password1 = UserGenerator.GetPasswords()[0];
        string username2 = UserGenerator.GetValidUsernames()[1];
        string password2 = UserGenerator.GetPasswords()[1];
        string username3 = UserGenerator.GetValidUsernames()[2];
        string password3 = UserGenerator.GetPasswords()[2];
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            Register(username1, password1);
            Login(username1, password1);
            Register(username2, password2);
            Login(username2, password2);
            Register(username3, password3);
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
        public void ChangeValidManagerPermissionsTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsTrue(RemoveStoreManager(username1, username2, storeID).Item1, RemoveStoreManager(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void NotAnAppointerDemoteManagerTest()
        {
            AppointStoreManage(username1, username2, storeID);
            AppointStoreManage(username2, username3, storeID);
            Assert.IsFalse(RemoveStoreManager(username1, username3, storeID).Item1, RemoveStoreManager(username1, username3, storeID).Item2);
        }

        [TestMethod]
        //bad
        public void BlankDemoterManagerTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsFalse(RemoveStoreManager(username1, "", storeID).Item1, RemoveStoreManager(username1, "", storeID).Item2);
        }
    }
}
