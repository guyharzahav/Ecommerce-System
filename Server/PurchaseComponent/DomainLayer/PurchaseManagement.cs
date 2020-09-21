using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.DAL.PurchaseDb;
using Server.DAL.StoreDb;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.PurchaseManagementTest/>
    public class PurchaseManagement
    {
        // Holding the carts and purchases by user
        private Dictionary<string, Cart> carts;
        private Dictionary<string, List<Purchase>> purchasesHistoryByUser;
        private Dictionary<Store, List<PurchaseBasket>> purchasesHistoryByStore;
        private StoreManagment storeManagment;
        private PaymentHandler paymentHandler;
        private DeliveryHandler deliveryHandler;
        private UserManager userManager;

        private static PurchaseManagement instance = null;
        private static readonly object padlock = new object();

        public static PurchaseManagement Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PurchaseManagement(PaymentHandler.Instance, DeliveryHandler.Instance);
                    }
                    return instance;
                }
            }
        }

        private PurchaseManagement(PaymentHandler paymentHandler, DeliveryHandler deliveryHandler)
        {
            ClearAll();
            this.storeManagment = StoreManagment.Instance;
            this.paymentHandler = paymentHandler;
            this.deliveryHandler = deliveryHandler;
            this.userManager = UserManager.Instance;
        }
        public PaymentHandler GetPaymentHandler()
        {
            return this.paymentHandler;
        }
        public DeliveryHandler GetDeliveryHandler()
        {
            return this.deliveryHandler;
        }

        public void LoadFromDb()
        {
            List<Cart> notPurchasedCarts = DbManager.Instance.LoadNotPurchasedCarts();
            foreach(Cart cart in notPurchasedCarts)
            {
                carts.Add(cart.user, cart);
            }

            List<Purchase> purchases = DbManager.Instance.LoadPurchases();
            List<Cart> purchasedCarts = new List<Cart>();
            foreach(Purchase purchase in purchases)
            {
                purchasedCarts.Add(purchase.UserCart);
                if (!purchasesHistoryByUser.ContainsKey(purchase.User))
                {
                    purchasesHistoryByUser.Add(purchase.User, new List<Purchase> { purchase });
                }
                else
                {
                    purchasesHistoryByUser[purchase.User].Add(purchase);
                }
            }

            foreach(Cart cart in purchasedCarts)
            {
                foreach(KeyValuePair<Store, PurchaseBasket> entry in cart.baskets)
                {
                    if(!purchasesHistoryByStore.ContainsKey(entry.Key))
                    {
                        purchasesHistoryByStore.Add(entry.Key, new List<PurchaseBasket> { entry.Value });
                    }
                    else
                    {
                        purchasesHistoryByStore[entry.Key].Add(entry.Value);
                    }
                }
            }

        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        /// Get the user ,store and product to add to the shopping cart
        /// Additionaly indicate how much items of the product should be in the cart
        /// <param name="exist">  means this product meant to be already in the cart (in case of change/remove existing product </param>
        public Tuple<bool, string> AddProductToShoppingCart(string userId, int storeId, int productId, int wantedAmount, bool exist)
        {

            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (String.IsNullOrWhiteSpace(userId))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            User user = userManager.GetAtiveUser(userId);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            Store store = storeManagment.getStore(storeId);

            if (store == null || !store.ActiveStore)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!store.ProductExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            if (wantedAmount < 0)
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NegativeProductAmountErrMsg);
            }

            if (!exist && wantedAmount == 0)
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ZeroProductAmountErrMsg);
            }

            int amount = store.GetProductDetails(productId).Item2;

            if (amount < wantedAmount)
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductShortageErrMsg);
            }

            Cart cart;
            if (!carts.TryGetValue(userId, out cart))
            {
                cart = CreateNewCart(userId);
                if(cart == null)
                {
                    return new Tuple<bool, string>(false, "there was error with CreateNewCart in the DB  in addProduct Function");
                }
                carts.Add(userId, cart);
            }

            return cart.AddProduct(store, productId, wantedAmount, exist);
        }

      

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Cart, string> GetCartDetails(string userName)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (String.IsNullOrWhiteSpace(userName))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<Cart, string>(null, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            User user = userManager.GetAtiveUser(userName);
            if (user is null)
            {
                return new Tuple<Cart, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            if (!carts.TryGetValue(userName, out Cart cart))
            {
                cart = CreateNewCart(userName);
                carts.Add(userName, cart);
            }

            return new Tuple<Cart, string>(cart, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28 </req>
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address, bool Failed = false)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (String.IsNullOrWhiteSpace(user))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            if (String.IsNullOrWhiteSpace(paymentDetails))
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NotValidPaymentErrMsg);
            }

            if (String.IsNullOrWhiteSpace(address))
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NotValidAddressErrMsg);
            }

            User userObject = userManager.GetAtiveUser(user);
            if (userObject is null)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            if (!carts.TryGetValue(user, out Cart userCart))
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.EmptyCartPurchaseErrMsg);
            }

            if (userCart.IsEmpty())
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.EmptyCartPurchaseErrMsg);
            }

            Tuple<bool, string> validCart = userCart.CheckProductsValidity();
            if (!validCart.Item1)
            {
                return validCart;
            }

            Tuple<bool, string> transactionres = DbManager.Instance.PerformPurchaseTransaction(userCart, paymentHandler, paymentDetails, address, deliveryHandler, purchasesHistoryByStore, purchasesHistoryByUser,Failed);
            if(!transactionres.Item1)
            {
                return transactionres;
            }

            carts[user] = CreateNewCart(user);
            return new Tuple<bool, string>(true, "");
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--history-37 </req>
        public Tuple<List<Purchase>, string> GetBuyerHistory(string user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            List<Purchase> res = new List<Purchase>();
            if (String.IsNullOrWhiteSpace(user))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<List<Purchase>, string>(res, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }
            
            User userObject = userManager.GetAtiveUser(user);
            if (userObject is null)
            {
                if (userManager.GetAtiveUser("Admin") is null)
                {
                    return new Tuple<List<Purchase>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
                }
            }
            
           
            if (!purchasesHistoryByUser.ContainsKey(user))
            {
                return new Tuple<List<Purchase>, string>(res, "");
            }

            return new Tuple<List<Purchase>, string>(purchasesHistoryByUser[user], "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system </param>
        public Tuple<List<PurchaseBasket>, string> GetStoreHistory(string manager, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            List<PurchaseBasket> res = new List<PurchaseBasket>();
            if (String.IsNullOrWhiteSpace(manager))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            User userObject = userManager.GetAtiveUser(manager);
            if (userObject is null)
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            Store store = storeManagment.getStore(storeId);
            if (store == null)
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!GetStoreHistoryAuthorization(userObject, storeId))
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }
            if (!purchasesHistoryByStore.TryGetValue(store, out List<PurchaseBasket> currHistory))
            {
                currHistory = new List<PurchaseBasket>();
            }

            return new Tuple<List<PurchaseBasket>, string>(currHistory, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system </param>
        private bool GetStoreHistoryAuthorization(User manager, int storeID)
        {
            if (manager.isSystemAdmin() || manager.isStoreOwner(storeID))
            {
                return true;
            }

            return manager.getUserPermission(storeID, CommonStr.MangerPermission.Purchase);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (String.IsNullOrWhiteSpace(admin))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            User userObject = userManager.GetAtiveUser(admin);
            if (userObject is null)
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }

            if (!userObject.isSystemAdmin())
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.NotAdminErrMsg);
            }
            return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(purchasesHistoryByStore, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (String.IsNullOrWhiteSpace(admin))
            {
                Logger.logError(CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.BlankOrNullInputErrMsg);
            }

            User userObject = userManager.GetAtiveUser(admin);
            if (userObject is null)
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }

            if (!userObject.isSystemAdmin())
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.NotAdminErrMsg);
            }
            return new Tuple<Dictionary<string, List<Purchase>>, string>(purchasesHistoryByUser, "");
        }

        /// <summary>
        ///  For clearing the data stored in this class for tests purposes
        /// </summary>
        public void ClearAll()
        {
            this.carts = new Dictionary<string, Cart>();
            this.purchasesHistoryByUser = new Dictionary<string, List<Purchase>>();
            this.purchasesHistoryByStore = new Dictionary<Store, List<PurchaseBasket>>();
        }

        /// <summary>
        /// Setup the dependencies of this class for tests purposes
        /// </summary>
        public void SetupDependencies(
            StoreManagment storeManagment,
            PaymentHandler paymentHandler,
            DeliveryHandler deliveryHandler,
            UserManager userManager)
        {
            this.storeManagment = storeManagment;
            this.userManager = userManager;
            this.paymentHandler = paymentHandler;
            this.deliveryHandler = deliveryHandler;
        }
        
        private Cart CreateNewCart(string userId)
        {
            Cart newCart = new Cart(userId);
            //DB Insert New Cart
            if (!UserManager.Instance.GetAtiveUser(userId).IsGuest)
            {
                try
                {
                    DbManager.Instance.InsertCart(StoreAdapter.Instance.ToDbCart(newCart), true);
                }
                catch(Exception ex)
                {
                    Logger.logError("Cart_CreateNewCartt db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return null;
                }
            }
            return newCart;
        }
    }
}
