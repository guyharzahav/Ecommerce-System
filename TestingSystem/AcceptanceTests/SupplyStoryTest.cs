using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-supply-system-send-products-284 </req>
    [TestClass]
    public class SupplyStoryTest : SystemTrackTest
    {
        string userID;

        [TestInitialize]
        public void SetUp()
        {
            Init();
            userID = enterSystem().Item2;
        }

        [TestCleanup]
        public void TearDown()
        {
            SetPaymentSystemConnection(true);
            SetSupplySystemConnection(true);
            Logout(userID);
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void LegalSupplyDetailsTest()
        {
            Assert.IsTrue(ProvideDeliveryForUser("BeerSheba&Israel&Rotenbereg&eser&ehadEsra", false).Item1);           
        }

        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(ProvideDeliveryForUser("BeerSheba&Israel&Rotenbereg", false).Item1);

        }

        [TestMethod]
        //bad
        public void ConnectionLostWithSupplySystemTest()
        {

            Assert.IsFalse(ProvideDeliveryForUser("BeerSheba&Israel&Rotenbereg&eser&ehadEsra", true).Item1);

        }
    }
}
