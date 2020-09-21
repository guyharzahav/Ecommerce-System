using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
    [TestClass]
    public class ChangeProductAmountStoryTest : SystemTrackTest
    {
        int productID = 1;
        int storeID;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int amount = 3;
        int newAmount = 1;
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
        public void ChangeValidProductAmountTest()
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(decraseProductAmount(storeID, username, productID, newAmount).Item1, decraseProductAmount(storeID, username, productID, newAmount).Item2);
        }

        [TestMethod]
        //sad
        public void ChangeAmountToUnExistingProductTest()
        {
            Assert.IsFalse(decraseProductAmount(storeID, username, productID, newAmount).Item1, decraseProductAmount(storeID, username, productID, newAmount).Item2);
        }

        [TestMethod]
        //bad
        public void ChangeAmountToNegTest()
        {
            newAmount = -1;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(decraseProductAmount(storeID, username, productID, newAmount).Item1, decraseProductAmount(storeID, username, productID, newAmount).Item2);
        }
    }
}
