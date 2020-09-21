using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-search-for-products-25 </req>
    [TestClass]
    public class SearchProductsStoryTest : SystemTrackTest
    {
         
        string validProductName = "Lego";
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
        int amount = 3;
        int productID = 3;
        string productDetails = "bla bla";


        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, productID, productDetails, 3.0, validProductName, "lego", amount);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void SearchProductByValidNameTest() 
        {
            Assert.AreNotEqual(0, ViewProductByName(validProductName).Count);
        }

        [TestMethod]
        //bad
        public void SearchProductByInvalidNameTest()
        {
            Assert.AreEqual(0, ViewProductsByCategory("plane").Count); 
        }

        [TestMethod]
        //sad
        public void SearchProductIllegalCharsNameTest()
        {
            Assert.AreEqual(0, ViewProductByName("@#$@#%%$&               #$% ").Count);
        }
    }
}
