using eCommerce_14a.StoreComponent.DomainLayer;
using Server.DAL.StoreDb;
using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.PurchaseDb
{
    public class ProductAtBasket
    {
        [Key, ForeignKey("Basket")]
        [Column(Order = 1)]
        public int BasketId { set; get; }
        public  DbPurchaseBasket Basket { set; get; }


        [Key, ForeignKey("Product")]
        [Column(Order = 2)]
        public int ProductId { set; get; }
        public DbProduct Product { set; get; }



        [ForeignKey("Store")]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }


        public int ProductAmount { set; get; }

        public ProductAtBasket (int basketid, int productid, int productamount, int storeId)
        {
            StoreId = storeId;
            BasketId = basketid;
            ProductId = productid;
            ProductAmount = productamount;
        }
        
        public ProductAtBasket()
        {

        }

    }
}
