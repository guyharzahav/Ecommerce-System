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
    public class DbPurchaseBasket
    {
        [Key, Column(Order =0)]
        public int Id { set; get; }

        [ForeignKey("User")]
        public string UserName { get; set; }
        public DbUser User { get; set; }


        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public DbCart Cart { get; set; }

        [ForeignKey("Store")]
        public int StoreId { set; get; }

        public DbStore Store { set; get; }

        public double Price { set; get; }

        public DateTime? PurchaseTime { get; set; }

        public DbPurchaseBasket(int id, string username, int storeid, double basketprice, DateTime? purchasetime, int cartid)
        {
            Id = id;
            UserName = username;
            StoreId = storeid;
            Price = basketprice;
            PurchaseTime = purchasetime;
            CartId = cartid;
            PurchaseTime = purchasetime;
        }


        public DbPurchaseBasket()
        {

        }

    }
}
