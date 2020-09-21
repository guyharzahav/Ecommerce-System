using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class PaymentSystemTests
    {

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.IsAlive()"
        [TestMethod]
        public void PaymentSystemHandshaketest()
        {
            Assert.IsTrue(PaymentSystemMock.IsAlive(true));
            Assert.IsFalse(PaymentSystemMock.IsAlive(false));
        }

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.Pay(string, int, int, string, string, string)"
        //[TestMethod]
        //public void SuccessfulPaymentTest()
        //{
        //    int res = PaymentSystem.Pay("5440988565421665", 11, 2019, "Israel Israeli", "400", "56080138");
        //    Assert.IsTrue (res >= 10000 && res <= 100000);
        //}

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.CancelPayment(int)"
        //[TestMethod]
        //public void SuccessfulCancelPaymentTest()
        //{
        //    int res = PaymentSystem.CancelPayment(90914);
        //    Assert.AreEqual(res, 1);
        //}
        [TestMethod]
        public void UnSuccesfullPaymentBlankArgs()
        {
            string paymentDetails = "3333444455556666&&11&&333&222222222";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsFalse(res >= 10000 && res <= 100000);
        }
        [TestMethod]
        public void UnSuccesfullPaymentNullArgs()
        {
            int res = PaymentHandler.Instance.pay(null);
            Assert.IsFalse(res >= 10000 && res <= 100000);
        }
        [TestMethod]
        public void UnSuccesfullPaymentNotEnoughArgs()
        {
            string paymentDetails = "3333444455556666&11&333&222222222";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsTrue(res == -1);
        }
        [TestMethod]
        public void SuccesfullPayment()
        {
            PaymentHandler.Instance.mock = true;
            string paymentDetails = "3333444455556666&4&11&333&222222222&4568";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsTrue(res != -1);
            PaymentHandler.Instance.mock = false;
        }
        [TestMethod]
        public void MonthNotGood()
        {
            string paymentDetails = "3333444455556666&78&11&333&222222222&4568";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsTrue(res == -1);
        }
        [TestMethod]
        public void SystemIsNouTp()
        {
            
            string paymentDetails = "3333444455556666&4&11&333&222222222&4568";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsTrue(res != -1);
            string paymentDetails2 = "3333444455556666&4&11&333&222222222&4568";
            PaymentHandler.Instance.mock = true;
            PaymentHandler.Instance.work = false;
            int res2 = PaymentHandler.Instance.pay(paymentDetails2,true);
            Assert.IsTrue(res2 == -1);
            PaymentHandler.Instance.mock = false;
            PaymentHandler.Instance.work = true;
        }
    }
}
