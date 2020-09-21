using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DAL.StoreDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--history-37 </req>
    [TestClass]
    public class ViewUserPurchaseHistoryStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string paymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        string address = "dani&Wollu&Wollurberg&wolocountry&12345678";
        int storeID;


       [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, 3, "lego", 3.0, "lego", "building", 2);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllPurchase();
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ViewValidHistoryTest() 
        {
            AddProductToBasket(username, storeID, 1, 1);
            PerformPurchase(username, paymentDetails, address);
            Assert.AreNotEqual(0, ViewPurchaseUserHistory(username).Item1.Count);
        }

        [TestMethod]
        //sad
        public void ViewNoHistoryTest()
        {
            SetUp();
            Assert.AreEqual(0, ViewPurchaseUserHistory(username).Item1.Count);
            TearDown();
        }

        [TestMethod]
        //bad
        public void ViewInvalidDetailsHistoryTest()
        {
            Assert.AreEqual(0, ViewPurchaseUserHistory(" ").Item1.Count);
        }
    }
}
