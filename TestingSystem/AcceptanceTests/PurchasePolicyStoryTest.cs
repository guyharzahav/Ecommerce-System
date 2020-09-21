using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{

    [TestClass]
    public class PurchasePolicyStoryTest : SystemTrackTest
    {
        string guestID;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            guestID = enterSystem().Item2;
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, 1, "", 4343, "43", "gfdfg", 24);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ValidPurchasePolicyTest()
        {
            Assert.IsTrue(updatePurchasePolicy(storeID, username, "p_min:2:1:20").Item1);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(updatePurchasePolicy(storeID, username, "p_min:2:100:20").Item1);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPolicyTest()
        {
            Assert.IsFalse(updatePurchasePolicy(storeID, username, "p_min:2:-1:20").Item1);

        }
    }
}
