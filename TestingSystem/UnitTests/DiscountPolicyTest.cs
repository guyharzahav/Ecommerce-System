
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
using System.Runtime.CompilerServices;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests.DiscountPolicyTest
{
    [TestClass]
    public class DiscountPolicyTest
    {
        Cart cart;
        Store store;
        Dictionary<int, PreCondition> preConditionsDict;

        [TestInitialize]
        public void TestInitialize()
        {
            preConditionsDict = new Dictionary<int, PreCondition>();
            preConditionsDict.Add(CommonStr.DiscountPreConditions.BasketPriceAboveX, new DiscountPreCondition(CommonStr.DiscountPreConditions.BasketPriceAboveX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.BasketProductPriceAboveEqX, new DiscountPreCondition(CommonStr.DiscountPreConditions.BasketProductPriceAboveEqX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX, new DiscountPreCondition(CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.NoDiscount, new DiscountPreCondition(CommonStr.DiscountPreConditions.NoDiscount));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX, new DiscountPreCondition(CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX));
            store = StoreTest.StoreTest.initValidStore();
            cart = new Cart("liav");
        }

        [TestMethod]
        public void TestRevealedDiscount1()
        {
            cart.AddProduct(store, 1, 10, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new RevealdDiscount(1, 30);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 30000;
            Assert.AreEqual(expected, discount);
        }

        [TestMethod]
        public void TestConditionalDiscount_MinBasketPrice()
        {

            cart.AddProduct(store, 1, 1, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new ConditionalBasketDiscount(preCondition:preConditionsDict[CommonStr.DiscountPreConditions.BasketPriceAboveX],discount:10,minBasketPrice: 2100);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 1090;
            Assert.AreEqual(expected, discount);
        }

        [TestMethod]
        public void TestConditionalDiscount_MinItemsAtBasket()
        {
            cart.AddProduct(store, 1, 1, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new ConditionalBasketDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX], discount: 20, minUnitsAtBasket: 2);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 1090 * 2;
            Assert.AreEqual(expected, discount);
        }

        [TestMethod]
        public void TestConditionalDiscount_BasketPrdouctPriceAboveX()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new ConditionalBasketDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.BasketProductPriceAboveEqX], discount: 10, minProductPrice: 10000);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 2000;
            Assert.AreEqual(expected, discount);
        }


        [TestMethod]
        public void TestConditionalDiscoun_NoDiscount()
        {
            cart.AddProduct(store, 1, 2, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new ConditionalBasketDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.NoDiscount], discount: 0);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 0;
            Assert.AreEqual(expected, discount);
        }

        [TestMethod]
        public void TestConditialDiscount_MinUnitsOfProductX()
        {
            cart.AddProduct(store, 1, 6, false);
            cart.AddProduct(store, 2, 2, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new ConditionalProductDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX], discount: 10, minUnits:5, productId:1);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 6000;
            Assert.AreEqual(expected, discount);

        }

        [TestMethod]
        public void TestCompundDiscountPolicy_XOR()
        {
            cart.AddProduct(store, 1, 1, false);
            cart.AddProduct(store, 2, 7, false);
            PurchaseBasket basket = cart.GetBasket(store);
           
            DiscountPolicy minItemsBasketPolicy = new ConditionalBasketDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX], discount: 20, minUnitsAtBasket: 7);
            DiscountPolicy MinUnitsProductPolicy = new ConditionalProductDiscount(preCondition: preConditionsDict[CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX], discount: 30, minUnits:1, productId:1);

            List<DiscountPolicy> policies_lst = new List<DiscountPolicy>();
            policies_lst.Add(minItemsBasketPolicy);
            policies_lst.Add(MinUnitsProductPolicy);
            DiscountPolicy compundDiscount = new CompundDiscount(CommonStr.DiscountMergeTypes.XOR, policies_lst);
            double discount = compundDiscount.CalcDiscount(basket);
            double expected = 3000;
            Assert.AreEqual(expected, discount);
        }

      


    }





}
