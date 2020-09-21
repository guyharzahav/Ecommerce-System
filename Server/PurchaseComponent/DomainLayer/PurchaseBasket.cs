using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.DAL.StoreDb;
using eCommerce_14a.UserComponent.DomainLayer;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseBasket
    {

        public int Id { get; set; }
        public string User { get; set; }

        public  Store store { get; set; }
        public Dictionary<int, int> products { get; set; }
        public double Price { get; set; }
        public DateTime? PurchaseTime { get; private set; }

        public PurchaseBasket(string user, Store store)
        {
            Id = DbManager.Instance.GetNextPurchBasketId();
            this.User = user;
            this.store = store;
            this.products = new Dictionary<int, int>();
            Price = 0;
            PurchaseTime = null;
        }

        public PurchaseBasket(int id, string user, Store store, Dictionary<int, int> products, double price, DateTime? purchaseTime)
        {
            Id = id;
            User = user;
            this.store = store;
            this.products = products;
            Price = price;
            PurchaseTime = purchaseTime;
        }

        public override string ToString()
        {
            string products_name = "";
            foreach (var product in products.Keys)
            {
                products_name += "Product ID - " + product + " Product Amount - " + products[product];
            }
            return products_name;
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        /// This method Add/Change/Remove product from this basket
        /// <param name="exist">state if it should be already in the basket</param>
        virtual
        public Tuple<bool, string> AddProduct(int productId, int wantedAmount, bool exist)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!this.Store.ProductExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            Dictionary<int, int> existingProducts = new Dictionary<int, int>(products);
            if (products.ContainsKey(productId))
            {
                if (!exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProductExistInCartErrMsg);
                }

                if (wantedAmount == 0)
                {

                    if (!UserManager.Instance.GetAtiveUser(this.User).IsGuest)
                    {
                        try
                        {
                            DbManager.Instance.DeletePrdocutAtBasket(DbManager.Instance.GetProductAtBasket(this.Id, productId), true);
                        }
                        catch(Exception ex)
                        {
                            Logger.logError("Cart_AddprProduct_DeleteProductAtBasket db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                            return new Tuple<bool, string>(false, "there was err in Cart_AddprProduct_DeleteProductAtBasket  update db " + ex.Message);
                        }
                        products.Remove(productId);
                    }
                    else

                    {
                        products.Remove(productId);
                    }

                }
                else
                {

                    if (!UserManager.Instance.GetAtiveUser(this.User).IsGuest)
                    {
                        try
                        {
                            DbManager.Instance.UpdateProductAtBasket(DbManager.Instance.GetProductAtBasket(this.Id, productId), wantedAmount, true);

                        }
                        catch(Exception ex)
                        {
                            Logger.logError("Cart_AddprProduct_UpdateProductAtBasket db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                            return new Tuple<bool, string>(false, "there was err in Cart_AddprProduct_UpdateProductAtBasket  update db " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProducAlreadyExistInCartErrMsg);
                }


                if (UserManager.Instance.GetAtiveUser(this.User) !=null && !UserManager.Instance.GetAtiveUser(this.User).IsGuest)
                {
                    try
                    {
                        DbManager.Instance.InsertProductAtBasket(StoreAdapter.Instance.ToProductAtBasket(this.Id, productId, wantedAmount, this.Store.Id), true);

                    }
                    catch (Exception ex)
                    {
                        Logger.logError("Cart_AddprProduct_InsertProductAtBasket db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, "there was err in Cart_AddprProduct_InsertProductAtBasket  update db " + ex.Message);
                    }
                    products.Add(productId, wantedAmount);
                    
                }
                else
                {
                    products.Add(productId, wantedAmount);
                    
                }
            }

            Tuple<bool, string> isValidBasket = Store.CheckIsValidBasket(this);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                if (!UserManager.Instance.GetAtiveUser(this.User).IsGuest)
                {
                    try
                    {
                        DbManager.Instance.DeletePrdocutAtBasket(DbManager.Instance.GetProductAtBasket(this.Id, productId), true);
                    }
                    catch (Exception ex)
                    {
                        Logger.logError("Cart_AddprProduct_DeletePrdocutAtBasket db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, "there was err in Cart_AddprProduct_DeletePrdocutAtBasket  update db " + ex.Message);
                    }

                }
                return isValidBasket;
            }

            Price = Store.GetBasketPriceWithDiscount(this);

            // DB Updating basket Price
            if (UserManager.Instance.GetAtiveUser(this.User)!= null && !UserManager.Instance.GetAtiveUser(this.User).IsGuest)
            {
                try
                {
                    DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this, true);
                }
                catch (Exception ex)
                {
                    Logger.logError("Cart_AddprProduct_UpdatePurchaseBasket db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "there was err in Cart_AddprProduct_UpdatePurchaseBasket  update db " + ex.Message);
                }
            }
            return new Tuple<bool, string>(true, null);
        }

        internal bool IsEmpty()
        {
            return products.Count == 0;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        public double UpdateCartPrice(bool saveCahnges)
        {
            Price = Store.GetBasketPriceWithDiscount(this);
            // DB update purchase BasketPrice
            if (UserManager.Instance.GetAtiveUser(User)!= null && !UserManager.Instance.GetAtiveUser(this.User).IsGuest)
            {
               DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this, saveCahnges);
            }
            return Price;
        }

        public Tuple<bool, string> SetPurchaseTime(DateTime purchaseTime, bool saveCahnges)
        {
            PurchaseTime = purchaseTime;

            //UPDATING purchase time in db
            if (!UserManager.Instance.GetAtiveUser(this.User).IsGuest)
            {
                try
                {
                    DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this, saveCahnges);
                }
                catch(Exception ex)
                {
                    Logger.logError("Cart_SetPurchaseTime db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "there was err in setpurchasetime update db " + ex.Message);
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> RemoveFromStoreStock(bool saveCahnges)
        {
            foreach (var product in products.Keys)
            {
                Tuple<bool, string> decreased = Store.DecraseProductAmountAfterPuarchse(product, products[product], saveCahnges);
                if (!decreased.Item1)
                {
                    return decreased;
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            if (Store == null || !Store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return Store.CheckIsValidBasket(this);
        }

        // For tests
        internal int GetAmountOfUniqueProducts()
        {
            return products.Keys.Count;
        }

        public Store Store
        {
            get { return store; }
        }
        public double GetBasketPriceWithDiscount()
        {
            return Store.GetBasketPriceWithDiscount(this);
        }
        public double GetBasketOrigPrice()
        {
            return Store.GetBasketOrigPrice(this);
        }

        public double getBasketDiscount()
        {
            return GetBasketOrigPrice() - GetBasketPriceWithDiscount();
        }

        public int GetNumProductsAtBasket()
        {
            int numProducts = 0;
            foreach(KeyValuePair<int, int> entry in products)
            {
                numProducts += entry.Value;
            }
            return numProducts;
        }
        public Dictionary<int, int> Products
        {
            get { return products; }
        }
        public void  RestoreItemsToStore(bool saveCahnges)
        {
            foreach (var product in products.Keys)
            {
                Store.IncreaseProductAmountAfterFailedPuarchse(product, products[product], saveCahnges);
            }
        }
    }
}
