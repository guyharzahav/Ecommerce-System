using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
    [TestClass]
    public class AdminViewAllPurchaseHistoryStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string paymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
        string address = "dani&Wollu&Wollurberg&wolocountry&12345678";
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            Init();
            Register(username, password);
            Login(username, password);
            Login("Admin","Admin");
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, 3, "lego", 3.0, "lego", "building", 2);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllPurchase();
            ClearAllUsers();
            ClearAllShops();
            //Publisher.Instance.cleanup();
        }

        [TestMethod]
        //happy
        public void ViewValidHistoryTest()
        {
            AddProductToBasket(username, storeID, 1, 1);
            PerformPurchase(username, paymentDetails, address);
            Assert.AreNotEqual(0, GetAllUsersHistory("Admin").Item1.Count);
        }

        [TestMethod]
        //sad
        public void ViewNoHistoryTest()
        {
            Assert.AreEqual(0, GetAllStoresHistory("Admin").Item1.Keys.Count);
        }

        [TestMethod]
        //bad
        public void ViewNotAdminHistoryTest()
        {
            Assert.IsNull(GetAllUsersHistory(username).Item1, GetAllUsersHistory(username).Item2);
        }
    }
}
