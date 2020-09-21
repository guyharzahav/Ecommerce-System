using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-change-store-managers-permissions-46- </req>
    [TestClass]
    public class ChangeManagerPermissionsStoryTest : SystemTrackTest
    {
        string username1 = UserGenerator.GetValidUsernames()[0];
        string password1 = UserGenerator.GetPasswords()[0];
        string username2 = UserGenerator.GetValidUsernames()[1];
        string password2 = UserGenerator.GetPasswords()[1];
        string username3 = UserGenerator.GetValidUsernames()[2];
        string password3 = UserGenerator.GetPasswords()[2];
        int storeID;
        int[] permissions = new int[] { 0, 1, 0, 1 , 0};

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
            Assert.IsTrue(ChangePermissions(username1, username2, storeID, permissions).Item1, ChangePermissions(username1, username2, storeID, permissions).Item2);
        }

        [TestMethod]
        //sad
        public void NotAnAppointerChangePermissionsTest()
        {
            AppointStoreManage(username1, username2, storeID);
            AppointStoreManage(username2, username3, storeID);
            Assert.IsFalse(ChangePermissions(username1, username3, storeID, permissions).Item1, ChangePermissions(username1, username3, storeID, permissions).Item2);
        }

        [TestMethod]
        //bad
        public void EmptyPermissionsArrayTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsFalse(ChangePermissions(username1, username2, storeID, new int[] { }).Item1, ChangePermissions(username1, username2, storeID, new int[] { }).Item2);
        }
    }
}
