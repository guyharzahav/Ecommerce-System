using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.UnitTests.Stubs;
using TestingSystem.UnitTests.StoreTest;
using eCommerce_14a.StoreComponent.DomainLayer;
using System.Security.AccessControl;
using Server.UserComponent.Communication;
using System.Threading;

namespace TestingSystem.ConcurrencyTests
{
    [TestClass]
    public class ConcurrencyWhiteBoxTests
    {
        private PurchaseManagement purchaseManagement;
        private StoreManagment sm;
        private AppoitmentManager apm;
        private UserManager userManager;
        private Store store;
        private string buyer = "meni";
        private string admin = "Admin";
        private string PaymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        private string DeliveryDetails = "dani&Wollu&Wollurberg&wolocountry&12345678";

        [TestInitialize]
        public void TestInitialize()
        {
            apm = AppoitmentManager.Instance;
            sm = StoreManagment.Instance;
            store = StoreTest.initValidStore();
            userManager = UserManager.Instance;
            purchaseManagement = PurchaseManagement.Instance;
            UserManager.Instance.Register(buyer, "123");
            UserManager.Instance.Login(buyer, "123");
            UserManager.Instance.RegisterMaster(admin, admin);
            UserManager.Instance.Login(admin, admin);
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            purchaseManagement.ClearAll();
            UserManager.Instance.cleanup();
            Publisher.Instance.cleanup();
            StoreManagment.Instance.cleanup();
        }

        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void PurchaseSameProductWithUnsufficientAmount() 
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.AddProductToShoppingCart("yosi", 100, 1, 10, false);
            Assert.IsTrue(res2.Item1, res2.Item2);
            bool res3 = true;
            bool res4 = true;
            Task user1 = Task.Factory.StartNew(() => { res3 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails).Item1; });
            Task user2 = Task.Factory.StartNew(() => { res4 = purchaseManagement.PerformPurchase("yosi", PaymentDetails, DeliveryDetails).Item1; });
            Task.WaitAll(user1, user2);
            Assert.IsTrue(res3^res4, "Perform concurrency purchase failed");
            int amount = StoreManagment.Instance.getStore(100).Inventory.Inv[1].Item2;
            Assert.IsTrue(amount >= 0, "amount is negative: " + amount);
        }

        /// <tests cref="UserManager.Register(string, string)"/>
        [TestMethod]
        public void RegisterWithSameUserName() 
        {
            bool res1 = true;
            bool res2 = true;
            Task user1 = Task.Factory.StartNew(() => { res1 = userManager.Register("yoav","yoavi").Item1; });
            Task user2 = Task.Factory.StartNew(() => { res2 = userManager.Register("yoav", "yoavchik").Item1; });
            Task.WaitAll(user1, user2);
            Assert.IsTrue(res1 ^ res2, "Registration concurrency failed");
        }

        /// <tests cref="AppoitmentManager.AppointStoreManager(string, string, int)"/>
        [TestMethod]
        public void AppointSamePerson() 
        {
            apm.AppointStoreOwner("shimon", "yosi", 100);
            bool res1 = true;
            bool res2 = true;
            Task user1 = Task.Factory.StartNew(() => { res1 = apm.AppointStoreManager("yosi", buyer, 100).Item1; });
            Task user2 = Task.Factory.StartNew(() => { res2 = apm.AppointStoreManager("shimon", buyer, 100).Item1; });
            Task.WaitAll(user1, user2);
            Assert.IsTrue(res1 ^ res2, "Appoint concurrency failed");
        }

        /// <tests cref="StoreManagment.createStore(string, string)"/>
        [TestMethod]
        public void OpenTwoStores() 
        {
            int res1 = 0;
            int res2 = 0;
            Task user1 = Task.Factory.StartNew(() => { res1 = sm.createStore(buyer, "s1").Item1; });
            Task user2 = Task.Factory.StartNew(() => { res2 = sm.createStore("yosi", "s2").Item1; });
            Task.WaitAll(user1, user2);
            Assert.IsTrue(res1 != res2, "open store concurrency failed");
        }

        /// <tests cref="UserManager.Login(string, string, bool)"/>
        [TestMethod]
        public void LoginAsGuestTwice()
        {
            string res1 = "";
            string res2 = "";
            Task user1 = Task.Factory.StartNew(() => { res1 = userManager.Login("asd", "s1", true).Item2; });
            Task user2 = Task.Factory.StartNew(() => { res2 = userManager.Login("asd", "s1", true).Item2; });
            Task.WaitAll(user1, user2);
            Assert.IsTrue(res1 != res2, "login as guest concurrency failed");
        }
    }
}
