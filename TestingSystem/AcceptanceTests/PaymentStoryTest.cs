using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-pay-for-products-283 </req>
    [TestClass]
    public class PaymentStoryTest :SystemTrackTest
    {
        string paymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        string address = "dani&Wollu&Wollurberg&wolocountry&12345678";
        string userID;
        int storeID;
        int amount = 1;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";
        int productID = 1;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];


        [TestInitialize]
        public void SetUp()
        {
            Init();
            Publisher.Instance.cleanup();
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            userID = enterSystem().Item2;
            
        }

        [TestCleanup]
        public void TearDown()
        {
            SetPaymentSystemConnection(true);
            SetSupplySystemConnection(true);
            ClearAllPurchase();
            ClearAllUsers();
            ClearAllShops();
            Publisher.Instance.cleanup();
        }

        [TestMethod]
        //happy
        public void LegalPaymentDetailsTest() 
        {
            AddProductToStore(storeID, username,productID,productDetails,productPrice, productName,productCategory,6);
            AddProductToBasket(userID, storeID, productID, amount);
            Assert.IsTrue(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }

        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(PayForProduct(userID, "", address).Item1, PayForProduct(userID, "", address).Item2);
        }

        [TestMethod]
        //bad
        public void ConnectionLostWithPaymentSystemTest()
        {
            SetPaymentSystemConnection(false);
            Assert.IsFalse(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }
    }
}
