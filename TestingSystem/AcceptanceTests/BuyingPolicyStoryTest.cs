using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282 </req>
    [TestClass]
    public class BuyingPolicyStoryTest : SystemTrackTest
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
        }

        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }

        [TestMethod]
        //happy
        public void ValidBuyingPolicyTest() 
        {
            Assert.IsTrue(CheckBuyingPolicy(guestID, 1, true).Item1, CheckBuyingPolicy(guestID, 1, true).Item2);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(CheckBuyingPolicy("  ", 0, false).Item1, CheckBuyingPolicy("  ", 0, false).Item2);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPurchaseTest()
        {
            Assert.IsFalse(CheckBuyingPolicy("  ", 0, false).Item1, CheckBuyingPolicy("  ", 0,false).Item2);
        }

    }
}
