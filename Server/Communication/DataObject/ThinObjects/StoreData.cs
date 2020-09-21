using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class StoreData
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<string> Owners { get; set; }
        public List<string> Mangers { get; set; }
        public InventoryData Products { get; set; }
        public string StoreThumbnail { get; set; }


        public StoreData() { }

        public StoreData(int storeId, List<string> owners, List<string> managers, InventoryData products, string storeName, string storeThumbnail = "")
        {
            StoreId = storeId;
            StoreName = storeName;
            Owners = owners;
            Mangers = managers;
            Products = products;
            StoreThumbnail = storeThumbnail;
        }
    }
}
