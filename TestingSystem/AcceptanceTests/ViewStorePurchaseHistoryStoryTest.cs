using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
    [TestClass]
    public class ViewStorePurchaseHistoryStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
        string paymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        string address = "dani&Wollu&Wollurberg&wolocountry&12345678";
        int productID = 1;
        int amount = 1;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ViewAllPurchaseTest() 
        {
            AddProductToBasket(username, storeID, productID, amount);
            PerformPurchase(username ,paymentDetails, address);
            Assert.AreNotEqual(0, ViewAllStorePurchase(username, storeID).Item1.Count);
        }

        [TestMethod]
        //sad
        public void EmptyPurchaseHisstoryTest() 
        {
            Assert.AreEqual(0, ViewAllStorePurchase(username, storeID).Item1.Count);
        }

        [TestMethod]
        //bad
        public void BlankUsernamePurchaseHisstoryTest()
        {
            Assert.AreEqual(0,ViewAllStorePurchase("", storeID).Item1.Count);
        }
    }
}
