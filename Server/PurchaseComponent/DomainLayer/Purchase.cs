using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class Purchase
    {
        public string User { get; private set; }

        public Cart UserCart { get; private set; }

        public Purchase(string user, Cart userCart)
        {
            User = user;
            UserCart = userCart;
        }
    }
}
