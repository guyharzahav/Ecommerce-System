
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;


namespace TestingSystem
{
    //from here comes all the functionality of the system
    public class SystemTrackTest
    {
        public BridgeInterface sys = Driver.GetBridge();

        public SystemTrackTest() { }

        /// PurchaseService(Naor):
        
        public Tuple<Cart, string> ViewCartDetails(string cartID)
        {
            return sys.ViewCartDetails(cartID);
        }

        public void ClearAllPurchase()
        {
            sys.ClearAllPurchase();
        }

        public bool CartIsEmpty(string userID)
        {
            return sys.CartIsEmpty(userID);
        }



        public  Tuple<bool, string> updateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            return sys.updateDiscountPolicy(storeId, userName, discountPolicy);
        }

        public  Tuple<bool, string> updatePurchasePolicy(int storeId, string userName, string purchasePolicy)
        {
            return sys.updatePurchasePolicy(storeId, userName, purchasePolicy);
        }

        public Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return sys.AddProductToBasket(UserID, storeID, productID, amount);
        }
        
        public Tuple<bool, string> PayForProduct(string userID, string paymentDetails, string address)
        {
            return sys.PayForProduct(userID, paymentDetails, address);
        }
        
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address,bool Failed = false)
        {
            return sys.PerformPurchase(user, paymentDetails, address,Failed);
        }


        public Tuple<List<Purchase>, string> ViewPurchaseUserHistory(string userName)
        {
            return sys.ViewPurchaseUserHistory(userName);
        }

        public Tuple<List<PurchaseBasket>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return sys.ViewAllStorePurchase(userName, storeID);
        }

        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return sys.RemoveProductFromShoppingCart(user, store, product);
        }

        public Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            return sys.GetAllUsersHistory(admin);
        }

        public Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            return sys.GetAllStoresHistory(admin);
        }





        /// StoreService(Liav):

        public Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return sys.IncreaseProductAmount(storeId, userName, productId, amount);
        }

        public Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return sys.decraseProductAmount(storeId, userName, productId, amount);
        }

        public Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return sys.AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
        }

        public Dictionary<int, List<Product>> ViewProductByStoreID(int storeID)
        {
            return sys.ViewProductByStoreID(storeID);
        }

        public Dictionary<string,object> ViewStoreDetails(int storeID)
        {
            return sys.ViewStoreDetails(storeID);
        }
        
        public Dictionary<int, List<Product>> ViewProductsByCategory(String category) 
        {
            return sys.ViewProductsByCategory(category);
        }
        
        public Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return sys.CloseStore(username, storeID);
        }
        
        public Dictionary<int, List<Product>> ViewProductByName(String productName)
        {
            return sys.ViewProductByName(productName);
        }
        
        public Tuple<int, string> OpenStore(string userName)
        {
            return sys.OpenStore(userName);
        }

        public Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return sys.UpdateProductDetails(storeId, userId, productId, newDetails, price, name, category);
        }

        public Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return sys.RemoveProductFromStore(username, storeID, productID);
        }
        public Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID, bool flag)
        {
            return sys.CheckBuyingPolicy(userID, storeID, flag);
        }

        public Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID,bool flag)
        {
            return sys.CheckDiscountPolicy(userID, storeID, flag);
        }

        public void ClearAllShops()
        {
            sys.ClearAllShops();
        }



        /// UserService + SystemService + AppointService(Sundy):

        public void SetSupplySystemConnection(bool v)
        {
            sys.SetSupplySystemConnection(v);
        }
        
        public void SetPaymentSystemConnection(bool v)
        {
            sys.SetPaymentSystemConnection(v);
        }
        
        public Tuple<bool, string> Logout(string userID)
        {
            return sys.Logout(userID);
        }
        
        public Tuple<bool, string> Login(String username, String password)
        {
            return sys.Login(username, password);
        }
        
        public Tuple<bool, string> Register(String username, String password)
        {
            return sys.Register(username, password);
        }
     
        public Tuple<bool, string> Init(bool flag = true)
        {
            return sys.Init(flag);
        }

        public Tuple<bool, string> enterSystem()
        {
            return sys.enterSystem();
        }

        public Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag)
        {
            return sys.ProvideDeliveryForUser(UserID, paymentFlag);
        }

        public Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return sys.AppointStoreOwner(owner, appoint, store);
        }

        public Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return sys.AppointStoreManage(owner, appoint, store);
        }

        public Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return sys.ChangePermissions(owner, appoint, store, permissions);
        }

        public Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return sys.RemoveStoreManager(owner, appoint, store);
        }
        public void ClearAllUsers()
        {
            sys.ClearAllUsers();
        }
    }
}

