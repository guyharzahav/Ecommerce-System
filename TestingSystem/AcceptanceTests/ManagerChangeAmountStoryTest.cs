using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---change-discount-511 </req>
    [TestClass]
    public class ManagerChangeAmountStoryTest : SystemTrackTest
    {
        int productID = 1;
        int storeID;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string username2 = UserGenerator.GetValidUsernames()[1];
        string password2 = UserGenerator.GetPasswords()[1];
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
            Register(username2, password2);
            Login(username2, password2);
            storeID = OpenStore(username).Item1;
            AppointStoreManage(username,username2,storeID);
            ChangePermissions(username, username2, storeID, new int[] { 1, 1, 1 , 0, 0});
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ChangeValidProductAmountTest()
        {
            AddProductToStore(storeID, username2, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(decraseProductAmount(storeID, username2, productID, newAmount).Item1, decraseProductAmount(storeID, username2, productID, newAmount).Item2);
        }

        [TestMethod]
        //sad
        public void ChangeAmountToUnExistingProductTest()
        {
            Assert.IsFalse(decraseProductAmount(storeID, username2, productID, newAmount).Item1, decraseProductAmount(storeID, username2, productID, newAmount).Item2);
        }

        [TestMethod]
        //bad
        public void ChangeAmountToNegTest()
        {
            newAmount = -1;
            AddProductToStore(storeID, username2, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(decraseProductAmount(storeID, username2, productID, newAmount).Item1, decraseProductAmount(storeID, username2, productID, newAmount).Item2);
        }
    }
}
