using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class NoConnectionPurchaseRequest : Message
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public string PaymentDetails { get; set; }

        public NoConnectionPurchaseRequest(string username, string address, string paymentDetails) : base(Opcode.PURCHASE_NO_CONNECTION)
        {
            Username = username;
            Address = address;
            PaymentDetails = paymentDetails;
        }
    }
}
