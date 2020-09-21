using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class PurchaseRequest : Message
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public string PaymentDetails { get; set; }

        public PurchaseRequest(string username, string address, string paymentDetails) : base(Opcode.PURCHASE)
        {
            Username = username;
            Address = address;
            PaymentDetails = paymentDetails;
        }

        public void Clear()
        {
            Username = "";
            Address = "";
            PaymentDetails = "";
        }
    }
}
