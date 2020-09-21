using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---change-discount-511 </req>
    class ManagerChangeDiscountStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string userManager = UserGenerator.GetValidUsernames()[1];
        string passManager = UserGenerator.GetPasswords()[1];
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            Register(userManager, passManager);
            Login(userManager, passManager);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AppointStoreManage(username, userManager, storeID);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }


    }
}
