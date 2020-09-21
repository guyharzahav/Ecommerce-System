using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbInventoryItem
    {

        [Key, ForeignKey("Store")]
        [Column(Order = 1)]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }

        [Key, ForeignKey("Product")]
        [Column(Order = 2)]
        public int ProductId { set; get; }
        public DbProduct Product { set; get; }
        
        public int Amount { set; get; }

        public DbInventoryItem(int storeId, int productId, int amount)
        {
            StoreId = storeId;
            ProductId = productId;
            Amount = amount;
        }

        public DbInventoryItem()
        {

        }

    }
}
