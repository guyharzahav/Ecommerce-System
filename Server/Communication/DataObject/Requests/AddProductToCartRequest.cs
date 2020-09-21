using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class AddProductToCartRequest : Message
    {
        public string User { get; set; }
        public int Store { get; set; }
        public int Product { get; set; }
        public int Amount { get; set; }

        public AddProductToCartRequest(string user, int store, int product, int amount) : base(Opcode.ADD_PRODUCT_TO_CART)
        {
            User = user;
            Store = store;
            Product = product;
            Amount = amount;
        }

        public AddProductToCartRequest() : base() { }

    }
}
