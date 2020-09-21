
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.Utils;
using System.Windows.Documents;
using System.Collections.Generic;
using TestingSystem.UnitTests.Stubs;
using TestingSystem.UnitTests.StoreTest;
using System.Linq.Expressions;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class PurchasePolicyTest
    {

        Cart cart;
        Store store;
        Dictionary<int, PreCondition> preConditionsDict;
        Dictionary<string, User> users;
        Dictionary<int, Store> stores;

        [TestInitialize]
        public void TestInitialize()
        {
            users = new Dictionary<string, User>();
            users.Add("liav", new User(1, "liav", false, false));
            users.Add("shay", new User(2, "shay", true, false));
            store = StoreTest.StoreTest.initValidStore();
            stores = new Dictionary<int, Store>();
            stores.Add(1, store);


            preConditionsDict = new Dictionary<int, PreCondition>();
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MaxUnitsOfProductType, new PurchasePreCondition(CommonStr.PurchasePreCondition.MaxUnitsOfProductType));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MaxItemsAtBasket, new PurchasePreCondition(CommonStr.PurchasePreCondition.MaxItemsAtBasket));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.StoreMustBeActive, new PurchasePreCondition(CommonStr.PurchasePreCondition.StoreMustBeActive));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.OwnerCantBuy, new PurchasePreCondition(CommonStr.PurchasePreCondition.OwnerCantBuy));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.allwaysTrue, new PurchasePreCondition(CommonStr.PurchasePreCondition.allwaysTrue));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MaxBasketPrice, new PurchasePreCondition(CommonStr.PurchasePreCondition.MaxBasketPrice));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MinBasketPrice, new PurchasePreCondition(CommonStr.PurchasePreCondition.MinBasketPrice));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MinItemsAtBasket, new PurchasePreCondition(CommonStr.PurchasePreCondition.MinItemsAtBasket));
            preConditionsDict.Add(CommonStr.PurchasePreCondition.MinUnitsOfProductType, new PurchasePreCondition(CommonStr.PurchasePreCondition.MinUnitsOfProductType));
            store = StoreTest.StoreTest.initValidStore();
            cart = new Cart("liav");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            UserManager.Instance.cleanup();
            StoreManagment.Instance.cleanup();
        }

        [TestMethod]
        public void TestSimpleByProduct_maxAmountValid()
        {

            cart.AddProduct(store, 1, 10, false);
            cart.AddProduct(store, 2, 1, false);
            cart.AddProduct(store, 3, 3, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new ProductPurchasePolicy(maxAmount:10, pre:preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], productId:1);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }

        [TestMethod]
        public void TestSimpleByProduct_maxAmountInValid()
        {

            cart.AddProduct(store, 1, 10, false);
            cart.AddProduct(store, 2, 1, false);
            cart.AddProduct(store, 3, 3, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new ProductPurchasePolicy(maxAmount: 9, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], productId: 1);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }


        [TestMethod]
        public void TestSimpleByBasket1_MinAmountValid()
        {

            cart.AddProduct(store, 1, 10, false);
            cart.AddProduct(store, 2, 1, false);
            cart.AddProduct(store, 3, 3, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new ProductPurchasePolicy(minAmount: 4, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinUnitsOfProductType], productId: 1);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }

        [TestMethod]
        public void TestSimpleByBasket1_MinAmountInValid()
        {

            cart.AddProduct(store, 1, 10, false);
            cart.AddProduct(store, 2, 1, false);
            cart.AddProduct(store, 3, 3, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new ProductPurchasePolicy(minAmount: 11, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinUnitsOfProductType], productId: 1);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MaxItemPerBasketValid()
        {
            cart.AddProduct(store, 1, 7, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(maxItems:11,pre:preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }


        [TestMethod]
        public void TestBasketPurchasePolicy_MaxItemPerBasketInValid()
        {
            cart.AddProduct(store, 1, 7, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(maxItems: 10, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MinItemPerBasketInValid()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(minItems: 7, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinItemsAtBasket]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MinItemPerBasketValid()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(minItems: 6, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinItemsAtBasket]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MinBasketPriceValid()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(minBasketPrice: 22899,pre: preConditionsDict[CommonStr.PurchasePreCondition.MinBasketPrice]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MinBasketPriceInValid()
        {
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(minBasketPrice: 10000, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinBasketPrice]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }



        [TestMethod]
        public void TestBasketPurchasePolicy_MaxBasketPriceValid()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(maxBasketPrice: 22900, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxBasketPrice]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_MaxBasketPriceInValid()
        {
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(maxBasketPrice: 100, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxBasketPrice]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestBasketPurchasePolicy_NoCondtion()
        {
            cart.AddProduct(store, 2, 2, false);
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new BasketPurchasePolicy(pre: preConditionsDict[CommonStr.PurchasePreCondition.allwaysTrue]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }



        [TestMethod]
        public void TestSimpleByUser()
        {
            cart.AddProduct(store, 3, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            basket.User = "shimon";
            PurchasePolicy purchaseplc = new UserPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.OwnerCantBuy]);
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestSimpleBySystem1_Valid()
        {
            cart = new Cart("liav");
            cart.AddProduct(store, 1, 7, false);
            PurchaseBasket basket = cart.GetBasket(store);
            PurchasePolicy purchaseplc = new SystemPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.StoreMustBeActive], 100);
            store.ActiveStore = false;
            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

        [TestMethod]
        public void TestCompund_Legal()
        {
            // cant buy more than 10 prods and cant buy more than 1 of item 2
            cart.AddProduct(store, 1, 7, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            store.ActiveStore = false;
            PurchasePolicy purchaseplcMinItemsAtBasket = new BasketPurchasePolicy(minItems: 1, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinItemsAtBasket]);
            PurchasePolicy purchaseplcMaxItemAtBasket = new BasketPurchasePolicy(maxItems: 10, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.AND, null);
            compund.add(purchaseplcMinItemsAtBasket);
            compund.add(purchaseplcMaxItemAtBasket);
            bool eligiblePurchase = compund.IsEligiblePurchase(basket);
            Assert.AreEqual(true, eligiblePurchase);
        }


        [TestMethod]
        public void TestCompund_ILLegal()
        {
            // cant buy more than 10 prods and cant buy more than 1 of item 2
            cart.AddProduct(store, 1, 7, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            store.ActiveStore = false;
            PurchasePolicy purchaseplcMinItemsAtBasket = new BasketPurchasePolicy(minItems: 12, pre: preConditionsDict[CommonStr.PurchasePreCondition.MinItemsAtBasket]);
            PurchasePolicy purchaseplcMaxItemAtBasket = new BasketPurchasePolicy(maxItems: 10, pre: preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.AND, null);
            compund.add(purchaseplcMinItemsAtBasket);
            compund.add(purchaseplcMaxItemAtBasket);
            bool eligiblePurchase = compund.IsEligiblePurchase(basket);
            Assert.AreEqual(false, eligiblePurchase);
        }

      

    }





}
