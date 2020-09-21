using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class PaymentSystemMock
    {
        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static bool IsAlive(bool works)
        {
            return works;
        }

        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static int Pay(string cardNumber, int month, int year, string holder, string ccv, string id, int res)
        {
            return res;
        }

        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static int CancelPayment(int transactionID, int res)
        {
            return res;
        }
    }
}
