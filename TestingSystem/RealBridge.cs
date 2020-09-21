using System;
using System.Collections.Generic;

using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace TestingSystem
{
    public class RealBridge : BridgeInterface
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        System_Service sysService; //sundy
        StoreService StoreService; //liav
        PurchaseService purchService; //naor

        public RealBridge()
        {
            appointService = new Appoitment_Service();
            userService = new UserService();
            sysService = new System_Service("Admin", "Admin");
            StoreService = new StoreService();
            purchService = new PurchaseService();
        }

        /// PurchaseService(Naor):

        public override void ClearAllPurchase() 
        {
            purchService.ClearAll();
        }
        
        public override Tuple<Cart, string> ViewCartDetails(string cartID)
        {
            return purchService.GetCartDetails(cartID);
        }

        public override bool CartIsEmpty(string userID) 
        {
            Tuple<Cart,string> res = purchService.GetCartDetails(userID);
            Cart resCart = res.Item1;
            return resCart.IsEmpty();
        }

        public override Tuple<bool, string> updateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            return StoreService.updateDiscountPolicy(storeId, userName, discountPolicy);
        }

        public override Tuple<bool, string> updatePurchasePolicy(int storeId, string userName, string purchasePolicy)
        {
            return StoreService.updatePurchasePolicy(storeId, userName, purchasePolicy);
        }


        public override Tuple<bool, string> AddProductToBasket(string userID, int storeID, int productID, int amount)
        {
            return purchService.AddProductToShoppingCart(userID, storeID, productID, amount);
        }

        public override Tuple<bool, string> PayForProduct(string userID, string paymentDetails, string address)
        {
            return purchService.PerformPurchase(userID, paymentDetails, address);
        }

        public override Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address, bool Failed = false)
        {
            return purchService.PerformPurchase(user, paymentDetails, address,Failed);
        }
        
        public override Tuple<List<Purchase>, string> ViewPurchaseUserHistory(string userName)
        {
            return purchService.GetBuyerHistory(userName);
        }

        
        public override Tuple<List<PurchaseBasket>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return purchService.GetStoreHistory(userName, storeID);
        }


        public override Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return purchService.RemoveProductFromShoppingCart(user, store, product);
        }

        public override Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            return purchService.GetAllUsersHistory(admin);
        }
        public override Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            return purchService.GetAllStoresHistory(admin);
        }


        /// StoreService(Liav):

        public override Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return StoreService.IncreaseProductAmount(storeId, userName, productId, amount);
        }

        public override Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return StoreService.decraseProduct(storeId, userName, productId, amount);
        }

        public override Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return StoreService.appendProduct(storeID, username, productDetails, productPrice, productName, productCategory, amount);
        }

        public override Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return StoreService.getStoreInfo(storeID);
        }

       public override Dictionary<int, List<Product>> ViewProductsByCategory(String category)
        {
            return StoreService.SearchProducts(new Dictionary<string, object> { { CommonStr.SearcherKeys.ProductCategory, category } });
        }

        public override Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return StoreService.removeStore(username, storeID);
        }


        public override Dictionary<int, List<Product>> ViewProductByName(String productName)
        {
            return StoreService.SearchProducts(new Dictionary<string, object> { { CommonStr.SearcherKeys.ProductName, productName } });
        }

        public override Dictionary<int, List<Product>> ViewProductByStoreID(int storeID)
        {
            return StoreService.SearchProducts(new Dictionary<string, object> { { CommonStr.SearcherKeys.StoreId, storeID } });
        }

        public override Tuple<int, string> OpenStore(string userName)
        {
            return StoreService.createStore(userName); // without discount or buying policy
        }

        public override Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return StoreService.UpdateProduct(userId, storeId, productId, newDetails, price, name, category, "");
        }

        public override Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return StoreService.removeProduct(storeID, username, productID);
        }

        public override Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, string>(flag,"");// stub
        }

        public override Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID, bool flag)
        {
            return new Tuple<bool, string>(flag, "");//  stub
        }

        public override void ClearAllShops()
        {
            StoreService.cleanup();
        }


        /// UserService + AppointService + SysService(Sundy):

        public override void SetSupplySystemConnection(bool v)
        {
            sysService.SetDeliveryConnection(v);
        }

        public override void SetPaymentSystemConnection(bool v)
        {
            sysService.SetPaymentConnection(v);
        }

        public override Tuple<bool, string> Logout(string userID)
        {
            return userService.Logout(userID);
        }

        public override Tuple<bool, string> Login(String username, String password)
        {
            return userService.Login(username, password);
        }

        public override Tuple<bool, string> Register(String username, String password)
        {
            return userService.Registration(username, password);
        }

        public override Tuple<bool, string> Init(bool flag = true)
        {
            return sysService.initSystem("Admin","Admin", flag);
        }

        public override Tuple<bool, string> enterSystem()
        {
            return userService.LoginAsGuest();
        }

        public override Tuple<bool, string> ProvideDeliveryForUser(string details, bool paymentFlag)
        {
            return sysService.ProvideDeliveryForUser(details, paymentFlag);
        }

        public override Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return appointService.AppointStoreOwner(owner, appoint, store);
        }

        public override Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return appointService.AppointStoreManage(owner, appoint, store);
        }

        public override Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return appointService.ChangePermissions(owner, appoint, store, permissions);
        }

        public override Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return appointService.RemoveStoreManager(owner, appoint, store);
        }

        public override void ClearAllUsers()
        {
            userService.cleanup();
        }
    }
}
