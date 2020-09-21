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

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class PurchaseManagementTest
    {
        private PurchaseManagement purchaseManagement;
        private Store store;
        private string buyer = "meni";
        private string admin = "Admin";
        private string PaymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        private string DeliveryDetails = "dani&Wollu&Wollurberg&wolocountry&12345678";

        [TestInitialize]
        public void TestInitialize()
        {

            store = StoreTest.StoreTest.initValidStore();
            purchaseManagement = PurchaseManagement.Instance;
            UserManager.Instance.Register(buyer, "123");
            UserManager.Instance.Login(buyer, "123");
            UserManager.Instance.RegisterMaster(admin, admin);
            UserManager.Instance.Login(admin, admin);
            
            
            //purchaseManagement.SetupDependencies(
            //    new StoreManagementStub(123),
            //    PaymentHandler.Instance,
            //    DeliveryHandler.Instance,
            //    new UserManagerStub());
        }

        [TestCleanup]
        public void Cleanup()
        {
            purchaseManagement.ClearAll();
            UserManager.Instance.cleanup();
            Publisher.Instance.cleanup();
            StoreManagment.Instance.cleanup();
        }

        [TestMethod]
        public void PurchaseEmptyCart()
        {
            Tuple<bool, string> res = purchaseManagement.PerformPurchase(buyer,PaymentDetails,DeliveryDetails);
            Assert.IsFalse(res.Item1, res.Item2);
        }
        [TestMethod]
        public void TestAddProductToCarProductNumberIsLow()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10000, false);
            Assert.IsFalse(res.Item1, res.Item2);
        }
        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddProductToCartSuccess()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res.Item1, res.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddProductToCartUserNotExist()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart("Shomshmo", 100, 1, 10, false);
            Assert.IsFalse(res.Item1, res.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddProductToCartStoreNotExist()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart(buyer, 99, 1, 10, false);
            Assert.IsFalse(res.Item1, res.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddProductToCarProductNotExist()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart(buyer, 1, 99, 10, false);
            Assert.IsFalse(res.Item1, res.Item2);
        }


        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddProductToCartAmountToHigh()
        {
            Tuple<bool, string> res = purchaseManagement.AddProductToShoppingCart(buyer, 100, 4, 10, false);
            Assert.IsFalse(res.Item1, res.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestChangeAmountProductToCart()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, true);
            Assert.IsTrue(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestAddAmountProductToCartThatAlreadyExist()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsFalse(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.AddProductToShoppingCart(string, int, int, int, bool)"/>
        [TestMethod]
        public void TestChangeAmountProductToCartToExceed()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 31110, true);
            Assert.IsFalse(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.GetCartDetails(string)"/>
        [TestMethod]
        public void TestGetCartDetails_OneItem()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<Cart, string> res2 = purchaseManagement.GetCartDetails(buyer);
            Assert.AreEqual(1, res2.Item1.GetAmountOfUniqueProducts(), res2.Item2);
        }

        /// <tests cref="PurchaseManagement.GetCartDetails(string)"/>
        [TestMethod]
        public void TestGetCartDetails_ZeroItem()
        {
            Tuple<Cart, string> res2 = purchaseManagement.GetCartDetails(buyer);
            Assert.AreEqual(0, res2.Item1.GetAmountOfUniqueProducts(), res2.Item2);
        }

        /// <tests cref="PurchaseManagement.GetCartDetails(string)"/>
        [TestMethod]
        public void TestGetCartDetails_NoSuchUser()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<Cart, string> res2 = purchaseManagement.GetCartDetails("Shonmaj");
            Assert.IsNull(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.GetCartDetails(string)"/>
        [TestMethod]
        public void TestGetCartDetails_AddedAndRemoveProduct()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<Cart, string> res2 = purchaseManagement.GetCartDetails(buyer);
            Assert.AreEqual(1, res2.Item1.GetAmountOfUniqueProducts(), res2.Item2);
            Tuple<bool, string> res3 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 0, true);
            Assert.IsTrue(res3.Item1, res3.Item2);
            Tuple<Cart, string> res4 = purchaseManagement.GetCartDetails(buyer);
            Assert.IsTrue(res4.Item1.IsEmpty(), res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetCartDetails(string)"/>
        [TestMethod]
        public void TestGetCartDetails_AddedAndChangeProduct()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<Cart, string> res2 = purchaseManagement.GetCartDetails(buyer);
            Assert.AreEqual(1, res2.Item1.GetAmountOfUniqueProducts(), res2.Item2);
            Tuple<bool, string> res3 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 4, true);
            Assert.IsTrue(res3.Item1, res3.Item2);
            Tuple<Cart, string> res4 = purchaseManagement.GetCartDetails(buyer);
            Assert.AreEqual(1, res4.Item1.GetAmountOfUniqueProducts(), res4.Item2);
        }

        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void TestPerformPurchase_Success()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void TestPerformPurchase_BlankArg()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, "", "");
            Assert.IsFalse(res2.Item1, res2.Item2);
            Tuple<bool, string> res3 = purchaseManagement.PerformPurchase(buyer, "", DeliveryDetails);
            Assert.IsFalse(res3.Item1, res3.Item2);
            Tuple<bool, string> res4 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, "");
            Assert.IsFalse(res4.Item1, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void TestPerformPurchase_NoSuchUser()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 10, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase("Nosuchi userio", PaymentDetails, DeliveryDetails);
            Assert.IsFalse(res2.Item1, res2.Item2);
        }

        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void TestPerformPurchase_TwoUserRisk()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res3 = purchaseManagement.AddProductToShoppingCart("yosi", 100, 1, 10, false);
            Assert.IsTrue(res3.Item1, res3.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<bool, string> res4 = purchaseManagement.PerformPurchase("yosi", PaymentDetails, DeliveryDetails);
            Assert.IsFalse(res4.Item1, res4.Item2);
        }
        [TestMethod]
        public void TestInventoryUnchanged()
        {
            //Store have only 100 products if it fails but inventory changed second user cannot buy the products
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res3 = purchaseManagement.AddProductToShoppingCart("yosi", 100, 1, 10, false);
            Assert.IsTrue(res3.Item1, res3.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, "");
            Assert.IsFalse(res2.Item1, res2.Item2);
            Tuple<bool, string> res4 = purchaseManagement.PerformPurchase("yosi", PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res4.Item1, res4.Item2);
        }
        [TestMethod]
        public void TestInventoryUnchangedNoDelivery()
        {
            //Store have only 100 products if it fails but inventory changed second user cannot buy the products
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res3 = purchaseManagement.AddProductToShoppingCart("yosi", 100, 1, 10, false);
            Assert.IsTrue(res3.Item1, res3.Item2);
            purchaseManagement.GetDeliveryHandler().setConnection(false);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails,true);
            Assert.IsFalse(res2.Item1, res2.Item2);
            purchaseManagement.GetDeliveryHandler().setConnection(true);
            Tuple<bool, string> res4 = purchaseManagement.PerformPurchase("yosi", PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res4.Item1, res4.Item2);
        }
        [TestMethod]
        public void TestUnchangedCart()
        {
            //Store have only 100 products if it fails but inventory changed second user cannot buy the products
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Assert.IsFalse(purchaseManagement.GetCartDetails(buyer).Item1.IsEmpty());
            Cart c1 = purchaseManagement.GetCartDetails(buyer).Item1;
            purchaseManagement.GetDeliveryHandler().setConnection(false);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails,true);
            Assert.IsFalse(res2.Item1, res2.Item2);
            Cart c2 = purchaseManagement.GetCartDetails(buyer).Item1;
            Assert.IsTrue(c1.Equals(c2));
            Assert.IsFalse(purchaseManagement.GetCartDetails(buyer).Item1.IsEmpty());
        }
        [TestMethod]
        public void TestRefundWasPerfomed()
        {
            //Store have only 100 products if it fails but inventory changed second user cannot buy the products
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            purchaseManagement.GetDeliveryHandler().setConnection(false);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            if(!res2.Item1)
            {
                Assert.IsTrue(purchaseManagement.GetPaymentHandler().refund("WoloCard",100).Item1);
            }
            purchaseManagement.GetDeliveryHandler().setConnection(true);
        }
        /// <tests cref="PurchaseManagement.PerformPurchase(string, string, string)"/>
        [TestMethod]
        public void TestPerformPurchase_DoublePurchase()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<bool, string> res3 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsFalse(res3.Item1, res3.Item2);
        }

        /// <tests cref="PurchaseManagement.GetBuyerHistory(string)"/>
        [TestMethod]
        public void TestGetBuyerHistory_Success()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<List<Purchase>, string> res3 = purchaseManagement.GetBuyerHistory(buyer);
            Assert.AreEqual(1,res3.Item1.Count, res3.Item2);
        }

        /// <tests cref="PurchaseManagement.GetBuyerHistory(string)"/>
        [TestMethod]
        public void TestGetBuyerHistory_NoHistory()
        {
            Tuple<List<Purchase>, string> res3 = purchaseManagement.GetBuyerHistory(buyer);
            Assert.AreEqual(0, res3.Item1.Count, res3.Item2);
        }

        /// <tests cref="PurchaseManagement.GetBuyerHistory(string)"/>
        [TestMethod]
        public void TestGetBuyerHistory_NoSuchUser()
        {
            Tuple<List<Purchase>, string> res3 = purchaseManagement.GetBuyerHistory("Kipud");
            Assert.AreEqual(0, res3.Item1.Count, res3.Item2);
        }

        /// <tests cref="PurchaseManagement.GetStoreHistory(string, int)"/>
        [TestMethod]
        public void TestGetStoreHistory_Success()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<List<PurchaseBasket>, string> res3 = purchaseManagement.GetStoreHistory("shimon", 100);
            Assert.AreEqual(1, res3.Item1.Count, res3.Item2);
            Tuple<List<PurchaseBasket>, string> res4 = purchaseManagement.GetStoreHistory("yosi", 100);
            Assert.AreEqual(1, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetStoreHistory(string, int)"/>
        [TestMethod]
        public void TestGetStoreHistory_NotAuthorized()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<List<PurchaseBasket>, string> res4 = purchaseManagement.GetStoreHistory("shmuel", 1);
            Assert.AreEqual(0, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetStoreHistory(string, int)"/>
        [TestMethod]
        public void TestGetStoreHistory_NoSuchUser()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<List<PurchaseBasket>, string> res4 = purchaseManagement.GetStoreHistory("wallE", 1);
            Assert.AreEqual(0, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetStoreHistory(string, int)"/>
        [TestMethod]
        public void TestGetStoreHistory_Admin()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<List<PurchaseBasket>, string> res4 = purchaseManagement.GetStoreHistory(admin, 100);
            Assert.AreEqual(1, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetAllStoresHistory(string)"/>
        [TestMethod]
        public void TestGetAllStoresHistory_Success()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<Store, List<PurchaseBasket>>, string> res4 = purchaseManagement.GetAllStoresHistory(admin);
            Assert.AreEqual(1, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetAllStoresHistory(string)"/>
        [TestMethod]
        public void TestGetAllStoresHistory_NotAdmin()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<Store, List<PurchaseBasket>>, string> res4 = purchaseManagement.GetAllStoresHistory(buyer);
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllStoresHistory(buyer);
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllStoresHistory("shimon");
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllStoresHistory("yosi");
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllStoresHistory("shmuel");
            Assert.IsNull(res4.Item1, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetAllStoresHistory(string)"/>
        [TestMethod]
        public void TestGetAllStoresHistory_NoSuchUser()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<Store, List<PurchaseBasket>>, string> res4;
            res4 = purchaseManagement.GetAllStoresHistory("NoSuchiSuch");
            Assert.IsNull(res4.Item1, res4.Item2);
        }


        /// <tests cref="PurchaseManagement.GetAllUsersHistory(string)"/>
        [TestMethod]
        public void TestGetAllUsersHistory_Success()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<string, List<Purchase>>, string> res4 = purchaseManagement.GetAllUsersHistory(admin);
            Assert.AreEqual(1, res4.Item1.Count, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetAllUsersHistory(string)"/>
        [TestMethod]
        public void TestGetAllUsersHistory_NotAdmin()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<string, List<Purchase>>, string> res4 = purchaseManagement.GetAllUsersHistory(buyer);
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllUsersHistory(buyer);
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllUsersHistory("shimon");
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllUsersHistory("yosi");
            Assert.IsNull(res4.Item1, res4.Item2);
            res4 = purchaseManagement.GetAllUsersHistory("shmuel");
            Assert.IsNull(res4.Item1, res4.Item2);
        }

        /// <tests cref="PurchaseManagement.GetAllUsersHistory(string)"/>
        [TestMethod]
        public void TestGetAllUsersHistory_NoSuchUser()
        {
            Tuple<bool, string> res1 = purchaseManagement.AddProductToShoppingCart(buyer, 100, 1, 100, false);
            Assert.IsTrue(res1.Item1, res1.Item2);
            Tuple<bool, string> res2 = purchaseManagement.PerformPurchase(buyer, PaymentDetails, DeliveryDetails);
            Assert.IsTrue(res2.Item1, res2.Item2);
            Tuple<Dictionary<string, List<Purchase>>, string> res4;
            res4 = purchaseManagement.GetAllUsersHistory("NoSuchiSuch");
            Assert.IsNull(res4.Item1, res4.Item2);
        }
    }
}
