using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;
using Server.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using Server.DAL;
using Server.Utils;
using System.Data.Entity.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using Newtonsoft.Json.Linq;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreManagmentTest/>
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;
        private UserManager userManager = UserManager.Instance;
        private static StoreManagment instance = null;
        private static readonly object padlock = new object();

        /// <summary>
        /// Public ONLY For generatin Stubs
        /// </summary>
        private StoreManagment()
        {
            stores = new Dictionary<int, Store>();
        }

        public static StoreManagment Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StoreManagment();
                    }
                    return instance;
                }
            }
        }


        public List<Store> GetAllStores() 
        {
            List<Store> retList = new List<Store>();
            foreach (Store store in stores.Values.ToList()) 
            {
                if (store.ActiveStore)
                    retList.Add(store);
            }
            return retList;
        }
      
        public void LoadFromDb()
        {
            List<Store> allstores = DbManager.Instance.LoadAllStores();
            foreach(Store store in allstores)
            {
                stores.Add(store.Id, store);
                if(store.owners.Count > 0)
                {
                    Publisher.Instance.subscribe(store.owners[0], store.Id);
                }
            }
          
        }

        public Dictionary<int, string> GetAvilableRawDiscount()
        {
            Dictionary<int, string> avilableDiscount = new Dictionary<int, string>();
            avilableDiscount.Add(CommonStr.DiscountPreConditions.NoDiscount, "no discount at all");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.BasketPriceAboveX, "Basket Price Above 'X'");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX, "Num Of Items At Basket Above 'X'");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.BasketProductPriceAboveEqX, "Discount 'X' On All Products Their Price Above 'Y'");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX, "Discount 'X' On Product 'Y' If Num Units Of Product 'Y' Bigger Than 'Z'");
            return avilableDiscount;
        }

        public Dictionary<int, string> GetAvilableRawPurchasePolicy()
        {
            Dictionary<int, string> avilablePurchasePolicy = new Dictionary<int, string>();
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.allwaysTrue, "No Condition");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.OwnerCantBuy, "Strore Owner Can't buy from the store");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.StoreMustBeActive, "in order to buy store must be active");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MaxItemsAtBasket, "Max 'X' Items Per Basket");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MinItemsAtBasket, "Min 'X' Items Per Basket");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MaxUnitsOfProductType, "Max 'X' Units Of Product 'Y' In Basket");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MinUnitsOfProductType, "Min 'X' Units Of Product 'Y' In Basket");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MaxBasketPrice, "Basket Price Is 'X' At Most");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.MinBasketPrice, "Basket Price Is 'X' At Least");
            return avilablePurchasePolicy;
        }



        public  Dictionary<string, string> GetStaffStroe(int storeID)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!stores.ContainsKey(storeID))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Dictionary<string, string>();
            }
            return stores[storeID].getStaff();
        }

        public Tuple<bool, string> appendProduct(int storeId, string userName, string pDetails, double pPrice, string pName, string pCategory, int amount, string imgUrl)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            productParams.Add(CommonStr.ProductParams.ProductImgUrl, imgUrl);
            return stores[storeId].appendProduct(user, productParams, amount);
        }


        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].removeProduct(user, productId);
        }

        internal string GetDiscountPolicy(int storeID)
        {
            Store store;
            if (!stores.TryGetValue(storeID, out store))
                return "";
            return store.DiscountPolicy.Describe(0);
        }

        internal string GetPurchasePolicy(int storeID)
        {
            Store store;
            if (!stores.TryGetValue(storeID, out store))
                return "";
            return store.PurchasePolicy.Describe(0);
        }

        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory, string imgUrl)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductId, productId);
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            productParams.Add(CommonStr.ProductParams.ProductImgUrl, imgUrl);


            return stores[storeId].UpdateProduct(user, productParams);
        }

        internal List<Store> GetStoresOwnedBy(string username)
        {
            List<Store> retList = new List<Store>();
            List<Store> allStores = stores.Values.ToList();
            foreach (Store store in allStores) 
            {
                List<string> owners = store.owners;
                foreach (string user in owners) 
                {
                    if (user.Equals(username))
                        retList.Add(store);
                }

                foreach (string user in store.managers)
                {
                    if (user.Equals(username))
                        retList.Add(store);
                }
            }
            return retList;
        }

        public Tuple<bool, string> addProductAmount(int storeId, string userName, int productId, int amount, bool saveChanges=false)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].IncreaseProductAmount(user: user, productId: productId, amount: amount, saveCahnges: saveChanges);
        }

        public Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount, bool saveCahnges=false)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].decrasePrdouctAmount(user: user, productId: productId, amount: amount, saveCahnges: saveCahnges);
        }


        public Tuple<bool, string> UpdateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            //throw new NotImplementedException();

            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }
            DiscountPolicy parsedDiscount = DiscountParser.Parse(discountPolicy);
            if (!DiscountParser.checkDiscount(parsedDiscount))
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.DiscountPolicyParsedFailed);
            }
            return stores[storeId].UpdateDiscountPolicy(user, parsedDiscount);

        }


        public Tuple<bool, string> UpdatePurchasePolicy(int storeId, string userName, string purchasePolicy)
        {
            //throw new NotImplementedException();
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }
            PurchasePolicy parsedPurchase = PurchasePolicyParser.Parse(purchasePolicy);
            if (!PurchasePolicyParser.CheckPurchasePolicy(parsedPurchase))
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.PurchasePolicyParsedFailed);

            }
            return stores[storeId].UpdatePurchasePolicy(user, parsedPurchase);

        }

     

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Dictionary<string, object>();
            }

            return stores[storeId].getSotoreInfo();
        }


     
        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].changeStoreStatus(user, status);

        }
        
       


        public virtual Store getStore(int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (stores.ContainsKey(storeId))
                return stores[storeId];
            return null;
        }

        public Dictionary<int, Store> getActiveSotres()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<int, Store> activeStores = new Dictionary<int, Store>();
            foreach (KeyValuePair<int, Store> storeEntry in stores)
            {
                if (storeEntry.Value.ActiveStore)
                {
                    activeStores.Add(storeEntry.Key, storeEntry.Value);
                }

            }
            return activeStores;
        }





        public Tuple<int, string> createStore(string userName, string storename)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if(storename == null || storename.Equals(""))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.illegalStoreName);
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.illegalStoreName);
            }


            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            lock (this)
            {
                int next_id = DbManager.Instance.GetNextStoreId();
                storeParam.Add(CommonStr.StoreParams.StoreId, next_id);
                storeParam.Add(CommonStr.StoreParams.StoreName, storename);
                storeParam.Add(CommonStr.StoreParams.mainOwner, user.Name);
                Store store = new Store(storeParam);
                //DB Insert Store
                Tuple<int, string> transactionRes = DbManager.Instance.InsertStoreTranscation(store, user, userName, next_id);
                if (transactionRes.Item1 >= 0)
                {
                    stores.Add(next_id, store);
                }
                return transactionRes;
            }
        }


        // impl on next version only!
        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }


            if (!isMainOwner(user, storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.notMainOwnerErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.notMainOwnerErrMessage);
            }


            if (!user.isStoreOwner(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.notStoreOwnerErrMessage + "- user: " + userName + " store id:" + storeId.ToString());
                return new Tuple<bool, string>(false, "user" + userName + " not store owner of " + storeId.ToString());
            }

            Tuple<bool, string> ownerShipedRemoved = userManager.removeAllFromStore(storeId);
            if (!ownerShipedRemoved.Item1)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), ownerShipedRemoved.Item2);
                return ownerShipedRemoved;
            }
            //Version 2 Addition
            Tuple<bool,string> ans =  Publisher.Instance.Notify(storeId, new NotifyData("Store Closed by Main Owner - "+userName));
            if (!ans.Item1)
                return ans;
            if (!Publisher.Instance.RemoveSubscriptionStore(storeId))
                return new Tuple<bool, string>(false,"Cannot Remove Subscription Store");


            try
            {
                //DB addition 
                DbManager.Instance.DeleteFullStoreTransaction(stores[storeId]);
            }
            catch(Exception ex)
            {
                Logger.logError("RemoveStore db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            stores.Remove(storeId);
            return new Tuple<bool, string>(true, "");
        }


        // impl on next version only!
        private bool isMainOwner(User user, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return stores[storeId].IsMainOwner(user);
        }


        public void setStores(Dictionary<int, Store> stores)
        {
            this.stores = stores;
        }

        public void cleanup()
        {
            this.stores = new Dictionary<int, Store>();
            userManager = UserManager.Instance;
        }
    }
}
