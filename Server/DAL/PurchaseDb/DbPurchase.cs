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
    public class DbPurchase
    {
        [Key, ForeignKey("Cart"),  Column(Order = 1)]
        public int CartId { get; set; }
        public DbCart Cart { get; set; }

        [ForeignKey("User")]
        public string UserName { get; set; }
        public DbUser User { get; set; }

        public DbPurchase(int cartid, string username)
        {
            CartId = cartid;
            UserName = username;
        }

        public DbPurchase()
        {

        }

    }
}
