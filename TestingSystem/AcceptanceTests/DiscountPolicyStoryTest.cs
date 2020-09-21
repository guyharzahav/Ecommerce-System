using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281 </req>
    [TestClass]
    public class DiscountPolicyStoryTest : SystemTrackTest
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
            AddProductToStore(storeID, username,1, "", 4343, "43", "gfdfg", 24);
        }
        
        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ValidDiscountPolicyTest() 
        {
            Assert.IsTrue(updateDiscountPolicy(storeID, username, "r:20:1").Item1);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(updateDiscountPolicy(storeID, username, "r:20:100").Item1);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPolicyTest()
        {
            Assert.IsFalse(updateDiscountPolicy(storeID, username, "r:20:-6").Item1);
        }
    }
}
