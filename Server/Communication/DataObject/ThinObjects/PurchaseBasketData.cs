using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class PurchaseBasketData
    {

        public StoreData Store { get; set; }
        public string Username { get; set; }
        public double Price { get; set; }
        public DateTime? PurchaseTime { get; set; }
        public Dictionary<int, int> Product { get; set; }

        public PurchaseBasketData() { }

        public PurchaseBasketData(StoreData store, string username, double price, DateTime? purchaseTime, Dictionary<int, int> product)
        {
            Store = store;
            Username = username;
            Price = price;
            PurchaseTime = purchaseTime;
            Product = product;
        }
    }
}
