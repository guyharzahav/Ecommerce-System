using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---add-product-411- </req>
    [TestClass]
    public class AddProductStoryTest : SystemTrackTest
    {
        int productID = 1;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
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
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
            //Publisher.Instance.cleanup();
        }

        [TestMethod]
        //happy
        public void ValidProductAddToStoreTest()
        {
            amount = 1;
            Assert.IsTrue(AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                "err");
        }

    
        [TestMethod]
        //bad
        public void AddProductWithNegAmountToStoreTest()
        {
            amount = -1;
            Assert.IsFalse(AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
        }
    }
}
