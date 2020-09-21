using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;

namespace eCommerce_14a.PurchaseComponent.ServiceLayer
{
    public class PurchaseService
    {
        private PurchaseManagement purchaseManagement = PurchaseManagement.Instance;

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProductToShoppingCart(string user, int store, int product, int amount)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, amount, false);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Cart, string> GetCartDetails(string user)
        {
            return purchaseManagement.GetCartDetails(user);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> ChangeProductAmoountInShoppingCart(string user, int store, int product, int amount)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, amount, true);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, 0, true);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28 </req>
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address, bool Failed = false)
        {
            return purchaseManagement.PerformPurchase(user, paymentDetails, address,Failed);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--history-37 </req>
        public Tuple<List<Purchase>, string> GetBuyerHistory(string user)
        {
            return purchaseManagement.GetBuyerHistory(user);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system</param>
        public Tuple<List<PurchaseBasket>, string> GetStoreHistory(string manager, int storeId)
        {
            return purchaseManagement.GetStoreHistory(manager, storeId);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            return purchaseManagement.GetAllStoresHistory(admin);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            return purchaseManagement.GetAllUsersHistory(admin);
        }


        public void ClearAll()
        {
            purchaseManagement.ClearAll();
        }
    }
}
