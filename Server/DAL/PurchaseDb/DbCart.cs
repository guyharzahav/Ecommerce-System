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
    public class DbCart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User"), Column(Order = 1)]
        public string UserName { get; set; }
        public DbUser User { get; set; }

        public double Price { get; set; }

        public bool IsPurchased { get; set; }

   

        public DbCart(int id, string username, double price, bool isPurchased)
        {
            Id = id;
            UserName = username;
            Price = price;
            IsPurchased = isPurchased;
        }

        public DbCart()
        {

        }
    }
}
