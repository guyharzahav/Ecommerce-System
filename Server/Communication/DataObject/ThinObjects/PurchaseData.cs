using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class PurchaseData
    {
        public string Username { get; set; }
        public CartData Cart { get; set; }

        public PurchaseData() { }

        public PurchaseData(string username, CartData cart)
        {
            Username = username;
            Cart = cart;
        }
    }
}
