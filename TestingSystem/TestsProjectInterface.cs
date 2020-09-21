using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    public interface TestsProjectInterface
    {
        /// ~~~~~~~~Naor~~~~~~~~:
        //Tuple<Cart, string> GetCartDetails(string user)@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@cant return cart
        List<object> ViewCartDetails(string cartID);
        //Tuple<bool, string> AddProductToShoppingCart(string user, int store, int product, int amount)
        Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID);
        Tuple<bool, string> PayForProduct(string userID, string paymentDetails);
        //Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)
        Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address);
        // Tuple<List<PurchaseBasket>, string> GetStoreHistory(string manager, int storeId)
        Tuple<List<object>, string> ViewAllStorePurchase(string userName, int storeID); // view all purchase of a certain store performed by username
        //Tuple<List<Purchase>, string> GetBuyerHistory(string user)
        Tuple<List<object>, string> ViewPurchaseUserHistory(string userName);


        /// ~~~~~~~~Liav~~~~~~~~:
        // Tuple<bool, String> decraseProduct(int storeId, int userId, Product p, int amount)
        // Tuple<bool, String> addProduct(int storeId, int userId, Product p, int amount)
        Tuple<bool, string> ChangeProductAmount(int storeID, string username, int productID, int newAmount);// changes the product amout in the store
        // Tuple<bool, string> appendProduct(int storeId, string userName, int productId, string productDetails, double productPrice, string productName, string productCategory, int amount)
        Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount); // add product to store with storeID.
        // Dictionary<string, object> getStoreInfo(int storeId)
        Dictionary<string, object> ViewStoreDetails(int storeID);// returns dict of the store details.
        // check with liav
        List<object> ViewStoreProductsByCategory(int storeID, String category);
        // Tuple<bool, string> removeStore(int userId, int storeId)
        Tuple<bool, string> CloseStore(string username, int storeID);// close the store with storeID.
        //check with liav
        Dictionary<int, List<object>> ViewProductByName(String productName);
        // Tuple<bool, string> createStore(int storeId, int userId) @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Tuple<int, string> OpenStore(string userName); // return -1 if fail else return unique storeID
        // Tuple<bool, string> updateProductDetails(int storeId, int userId, int productId, string newDetails)
        Tuple<bool, string> UpdateProductDetails(int storeID, string username, int productID, string productDetails, double productPrice, string newName, string productCategory); // changes the product's name.
        //liav should add this func
        Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID);// remove the product from the store.
        Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID);
        Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID);
        void ClearAllShops();


        /// ~~~~~~~~Sundy~~~~~~~~:
        void setSupplySystemConnection(bool v);
        void setPaymentSystemConnection(bool v);
        // bool Logout(string user)
        Tuple<bool, string> Logout(string userID);
        // bool Login(string username, string password)
        Tuple<bool, string> Login(String username, String password);
        // bool Registration(string username,string password)
        Tuple<bool, string> Register(String username, String password);
        Tuple<bool, string> Init();
        // bool LoginAsGuest()
        Tuple<string, string> enterSystem(); // returns "-1" if fail otherwise returns unique username
        Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag);
        // Tuple<bool,string> AppointStoreOwner(string owner,string appoint,Store store)
        Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store);
        // Tuple<bool, string> AppointStoreManage(string owner, string appoint, Store store)
        Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store);
        // Tuple<bool, string> ChangePermissions(string owner, string appoint, Store store,int[] permissions)
        Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions);
        // Tuple<bool, string> RemoveStoreManager(string owner, string appoint, Store store)
        Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store);
        void ClearAllUsers();
        

    }
}
