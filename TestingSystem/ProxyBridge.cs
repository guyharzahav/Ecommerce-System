using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class ProxyBridge : BridgeInterface
    {
        public ProxyBridge() { }

        public override Tuple<bool, string> Login(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> Register(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> Init(bool flag = true)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return new Dictionary<string, object>();
        }

        public override Dictionary<int, List<Product>> ViewProductsByCategory(String category)
        {
            return new Dictionary<int, List<Product>>();
        }

        public override Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Dictionary<int, List<Product>> ViewProductByName(String productName)
        {
            return new Dictionary<int, List<Product>>();
        }

        public override Tuple<Cart, string> ViewCartDetails(string id)
        {
            return new Tuple<Cart, string>(new Cart(id), "");
        }

        public override Tuple<bool, string> enterSystem()
        {
            return new Tuple<bool, string>(true, "");
        }

        public override Tuple<bool, string> PayForProduct(string userID, string paymentDetails, string address)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override void SetPaymentSystemConnection(bool v)
        {
            return;
        }

        public override Tuple<bool, string> Logout(string userID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override void SetSupplySystemConnection(bool v)
        {
            return;
        }

        public override void ClearAllUsers()
        {
            return;
        }

        public override Tuple<int, string> OpenStore(string userName)
        {
            return new Tuple<int, String>(1, "");
        }

        public override void ClearAllShops()
        {
            return;
        }

        public override Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address, bool Failed = false)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<List<Purchase>, string> ViewPurchaseUserHistory(string userName)
        {
            return new Tuple<List<Purchase>, string>(new List<Purchase>(), "");
        }

        public override Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<List<PurchaseBasket>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return new Tuple<List<PurchaseBasket>, string>(new List<PurchaseBasket>(), "");
        }

        public override Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }
        public override Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public override bool CartIsEmpty(string userID)
        {
            return true;
        }

        public override Dictionary<int, List<Product>> ViewProductByStoreID(int storeID)
        {
            return new Dictionary<int, List<Product>>();
        }

        public override void ClearAllPurchase()
        {
            return;
        }

        public override Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            return new Tuple<Dictionary<string, List<Purchase>>, string>(new Dictionary<string, List<Purchase>>(), "");
        }

        public override Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(new Dictionary<Store, List<PurchaseBasket>>(), "");
        }

    }
}
