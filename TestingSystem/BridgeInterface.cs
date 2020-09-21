using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    //this is the empty bridge for abstraction
    public class BridgeInterface
    {
        public BridgeInterface() { }



        public virtual Tuple<bool, string> updateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> updatePurchasePolicy(int storeId, string userName, string purchasePolicy)
        {
            return new Tuple<bool, String>(true, "");
        }


        public virtual Tuple<bool, string> Login(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> Register(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> Init(bool flag = true)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return new Dictionary<string, object>();
        }

        public virtual Dictionary<int, List<Product>> ViewProductsByCategory(String category)
        {
            return new Dictionary<int, List<Product>>();
        }

        public virtual Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Dictionary<int, List<Product>> ViewProductByName(String productName)
        {
            return new Dictionary<int, List<Product>>();
        }

        public virtual Tuple<Cart, string> ViewCartDetails(string id) 
        {
            return new Tuple<Cart,string>(new Cart(id), "");
        }

        public virtual Tuple<bool, string> enterSystem() 
        {
            return new Tuple<bool, string>(true, "");
        }

        public virtual Tuple<bool, string> PayForProduct(string userID, string paymentDetails, string address)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual void SetPaymentSystemConnection(bool v)
        {
            return;
        }

        public virtual Tuple<bool, string> Logout(string userID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag) 
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual void SetSupplySystemConnection(bool v) 
        {
            return;
        }

        public virtual void ClearAllUsers() 
        {
            return;
        }

        public virtual Tuple<int, string> OpenStore(string userName)
        {
            return new Tuple<int, String>(1, "");
        }

        public virtual void ClearAllShops()
        {
            return;
        }

        public virtual Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address, bool Failed)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<List<Purchase>, string> ViewPurchaseUserHistory(string userName)
        {
            return new Tuple<List<Purchase>, string>(new List<Purchase>(), "");
        }

        public virtual Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<List<PurchaseBasket>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return new Tuple<List<PurchaseBasket>, string>(new List<PurchaseBasket>(), "");
        }

        public virtual Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }
        public virtual Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public virtual bool CartIsEmpty(string userID)
        {
            return true;
        }

        public virtual Dictionary<int, List<Product>> ViewProductByStoreID(int storeID)
        {
            return new Dictionary<int, List<Product>>();
        }

        public virtual void ClearAllPurchase()
        {
            return;
        }

        public virtual Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            return new Tuple<Dictionary<string,List<Purchase>>,string>(new Dictionary<string, List<Purchase>>(), "");
        }
        public virtual Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(new Dictionary<Store, List<PurchaseBasket>>(), "");
        }
    }
}
