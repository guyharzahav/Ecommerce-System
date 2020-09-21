using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class CartData
    {
        public string Username { get; set; }
        public List<PurchaseBasketData> baskets { get; set; }

        public CartData() { }

        public CartData(string username, List<PurchaseBasketData> baskets)
        {
            Username = username;
            this.baskets = baskets;
        }
    }
}
