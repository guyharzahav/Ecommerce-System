using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests.Stubs
{
    class PurchaseBasketStub : PurchaseBasket
    {
        public PurchaseBasketStub(string user,  StoreStub store): base(user, store)
        {

        }
        override
        public Tuple<bool, string> AddProduct(int productId, int wantedAmount, bool exist)
        {
            base.Products.Add(productId, wantedAmount);
            return new Tuple<bool, string>(true, null);
        }
    }
}
