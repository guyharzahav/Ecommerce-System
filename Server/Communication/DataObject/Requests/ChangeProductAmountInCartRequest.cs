using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class ChangeProductAmountInCartRequest : Message
    {
        public string User { get; set; }
        public int Store { get; set; }
        public int Product { get; set; }
        public int Amount { get; set; }

        public ChangeProductAmountInCartRequest(string user, int store, int product, int amount) : base(Opcode.CHANGE_PRODUCT_AMOUNT_CART)
        {
            User = user;
            Store = store;
            Product = product;
            Amount = amount;
        }

        public ChangeProductAmountInCartRequest() : base() { }
    }
}
