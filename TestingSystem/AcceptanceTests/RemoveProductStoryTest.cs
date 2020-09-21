using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---remove-product-413- </req>
    [TestClass]
    public class RemoveProductStoryTest : SystemTrackTest
    {
        int productID = 1;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
        int amount = 1;
        int anotherStoreID;
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
            anotherStoreID = OpenStore(username).Item1;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void RemoveValidProductFromStoreTest()
        {
            amount = 1;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(RemoveProductFromStore(username, storeID, productID).Item1, RemoveProductFromStore(username, storeID, productID).Item2);
        }

        [TestMethod]
        //sad
        public void RemoveUnExistingProductFromStoreTest()
        {
            Assert.IsFalse(RemoveProductFromStore(username, storeID, productID).Item1, RemoveProductFromStore(username, storeID, productID).Item2);
        }

        [TestMethod]
        //bad
        public void RemoveProductFromAnotherStoreTest()
        {
            amount = 1;
            AddProductToStore(anotherStoreID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(RemoveProductFromStore(username, storeID, productID).Item1, RemoveProductFromStore(username, storeID, productID).Item2);
        }
    }
}
