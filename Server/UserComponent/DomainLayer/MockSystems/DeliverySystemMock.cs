using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class DeliverySystemMock
    {
        public DeliverySystemMock()
        {

        }
        public static bool IsAlive(bool works)
        {
            return works;
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static int Supply(int res)
        {
            return res;
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static int CancelSupply(int transactionID)
        {
            return transactionID;
        }
    }
}
