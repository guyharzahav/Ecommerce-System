using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.DAL;
using Server.DAL.StoreDb;
using eCommerce_14a.UserComponent.DomainLayer;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{

    public class Cart
    {

        public int Id { get; set; }
        public string user { get; set; }
        public Dictionary<Store, PurchaseBasket> baskets { get; set; }
        public double Price { get; private set; }

        public bool IsPurchased { get; set; }

        public Cart(string user)
        {
            Id = DbManager.Instance.GetnextCartId();
            this.user = user;
            this.baskets = new Dictionary<Store, PurchaseBasket>();
            Price = 0;
            IsPurchased = false;
        }
        public Cart(int id, string username, double price, Dictionary<Store, PurchaseBasket> purchasebaskets, bool ispurchased)
        {
            user = username;
            Id = id;
            Price = price;
            baskets = purchasebaskets;
            IsPurchased = ispurchased;
        }

        internal Dictionary<Store, PurchaseBasket> GetBaskets()
        {
            return this.baskets;
        }
        
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(Store store, int productId, int wantedAmount, bool exist)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!store.ProductExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            if (!baskets.TryGetValue(store, out PurchaseBasket basket))
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProducAlreadyExistInCartErrMsg);
                }

                basket = new PurchaseBasket(this.user, store);

                //Inserting new basket To db
                if(UserManager.Instance.GetAtiveUser(this.user)!=null && !UserManager.Instance.GetAtiveUser(this.user).IsGuest)
                {
                    if (!UserManager.Instance.GetAtiveUser(this.user).IsGuest)
                    {
                        try
                        {
                            DbManager.Instance.InsertPurchaseBasket(basket, this.Id, true);
                        }
                        catch(Exception ex)
                        {
                            Logger.logError("Cart_AddProduct db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                            return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
                        }
                    }

                }

                baskets.Add(store, basket);
            }

            Tuple<bool, string> res = basket.AddProduct(productId, wantedAmount, exist);

            if (basket.IsEmpty())
            {

                if (!UserManager.Instance.GetAtiveUser(this.user).IsGuest)
                {
                    try
                    {
                        DbManager.Instance.DeletePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(basket.Id), true);
                    }
                    catch(Exception ex)
                    {
                        Logger.logError("Cart_AddProduct db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
                    }
                }

                baskets.Remove(store);
            }
            
            Tuple<bool, string> updateCartPriceRes =  UpdateCartPrice(true);
            if(!updateCartPriceRes.Item1)
            {
                return updateCartPriceRes;
            }

            return res;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        public Tuple<bool, string> UpdateCartPrice(bool saveChanges)
        {
            double oldPrice = Price;
            Price = 0;
            
            foreach (var basket in baskets.Values)
            {
                Price += basket.UpdateCartPrice(saveChanges);
            }


            //Update CART PRICE AT DB
            if (UserManager.Instance.GetAtiveUser(user)!=null && !UserManager.Instance.GetAtiveUser(this.user).IsGuest)
            {
                try
                {
                    DbManager.Instance.UpdateDbCart(DbManager.Instance.GetDbCart(Id), this, IsPurchased, saveChanges);
                }
                catch (Exception ex)
                {
                    Price = oldPrice;
                    Logger.logError("Cart_UpdateCartPrice db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
                }
            }
            
            return new Tuple<bool, string>(true, "");

        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            foreach (var basket in baskets.Values)
            {
                Tuple<bool, string> res = basket.CheckProductsValidity();
                if (!res.Item1)
                {
                    return res;
                }
            }

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> SetPurchaseTime(DateTime purchaseTime, bool saveCahnges)
        {
            foreach (var basket in baskets.Values)
            { 
                Tuple<bool, string> res = basket.SetPurchaseTime(purchaseTime, saveCahnges);
                if(!res.Item1)
                {
                    return res;
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        public bool IsEmpty()
        {
            return baskets.Count == 0;
        }

        public Tuple<bool, string> RemoveFromStoresStock(bool saveChanges)
        {
            foreach (var basket in baskets.Values)
            {
                Tuple<bool, string> removedRes = basket.RemoveFromStoreStock(saveChanges);
                if(!removedRes.Item1)
                {
                    return removedRes;
                }

            }
            return new Tuple<bool, string>(true, "");

        }

        public void RestoreItemsToStores(bool saveCahnges)
        {
            foreach (var basket in baskets.Values)
            {
                basket.RestoreItemsToStore(saveCahnges);
            }
        }

        // For tests
        public int GetAmountOfUniqueProducts()
        {
            int res = 0;
            foreach (var basket in baskets.Values)
            {
                res += basket.GetAmountOfUniqueProducts();
            }

            return res;
        }

        public PurchaseBasket GetBasket(Store store)
        {
            if (baskets.ContainsKey(store))
                return baskets[store];
            else
                return null;
        }
    }
}
