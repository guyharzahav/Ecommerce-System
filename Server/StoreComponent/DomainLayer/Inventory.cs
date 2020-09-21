using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using eCommerce_14a.Utils;
using System.ComponentModel.DataAnnotations;
using Server.DAL;
using Server.DAL.StoreDb;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    
    /// <testclass cref ="TestingSystem.UnitTests.InventroyTest/>
    
    public class Inventory
    {
        public Dictionary<int, Tuple<Product, int>> InvProducts { get; set; }
        

        public Inventory()
        {
            this.InvProducts = new Dictionary<int, Tuple<Product, int>>();
        }

        public Inventory(Dictionary<int, Tuple<Product, int>> invProducts)
        {
            InvProducts = invProducts;
        }

        public Dictionary<int, Tuple<Product, int>> Inv
        {
            get { return InvProducts; }
        }

        public Tuple<bool, string> appendProduct(Dictionary<string, object> productParams, int amount, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (amount < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg);
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg);
            }

            double pPrice = (double)productParams[CommonStr.ProductParams.ProductPrice];
            if (pPrice < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.ProductPriceErrMsg);
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);
            }

            string pDetails = (string)productParams[CommonStr.ProductParams.ProductDetails];
            string pName = (string)productParams[CommonStr.ProductParams.ProductName];
            string pCategory = (string)productParams[CommonStr.ProductParams.ProductCategory];
            string imgUrl = (string)productParams[CommonStr.ProductParams.ProductImgUrl];
            Product product;
            // DB Addition
            if(productParams.ContainsKey(CommonStr.ProductParams.ProductId))
            {
                product = new Product(pid:(int)productParams[CommonStr.ProductParams.ProductId] ,sid:storeId, details: pDetails, price:pPrice, name: pName, category: pCategory, imgUrl: imgUrl);

            }
            else
            {
                product = new Product(sid: storeId, details: pDetails, price: pPrice, name: pName, category: pCategory, imgUrl: imgUrl);

            }

            InvProducts.Add(product.Id, new Tuple<Product, int>(product, amount));
            try
            {
               DbManager.Instance.AppendProductTransaction(product, amount, storeId, true);
            }
            catch(Exception ex)
            {
                Logger.logError("append product db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);

            }
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(int productId, int storeid)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!InvProducts.ContainsKey(productId))
            {
                Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            //Remove product and inventory item from db!
            try
            {
                DbManager.Instance.RemoveProductTransaction(InvProducts[productId].Item1);

            }
            catch (Exception ex)
            {
                Logger.logError("remove product db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            if (InvProducts.Remove(productId))
            {
            
            }
            else
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.GeneralErrMessage.UnKnownErr);
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.UnKnownErr);
            }
            return new Tuple<bool, string>(true, "");

        }


        public Tuple<bool, string> UpdateProduct(Dictionary<string, object> productParams)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            int productId = (int)productParams[CommonStr.ProductParams.ProductId];
            if (!InvProducts.ContainsKey(productId))
            {
                Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            double price = (double)productParams[CommonStr.ProductParams.ProductPrice];
            if (price < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.ProductPriceErrMsg);
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);
            }

            Product product = InvProducts[productId].Item1;
            product.Details = (string)productParams[CommonStr.ProductParams.ProductDetails];
            product.Price = price;
            product.Name = (string)productParams[CommonStr.ProductParams.ProductName];
            product.Category = (string)productParams[CommonStr.ProductParams.ProductCategory];
            product.ImgUrl = (string)productParams[CommonStr.ProductParams.ProductImgUrl];
            int amount = InvProducts[productId].Item2;

            InvProducts[productId] = new Tuple<Product, int>(product, amount);
            try
            {
                DbManager.Instance.UpdateProductTransaction(product);
            }
            catch (Exception ex)
            {
                Logger.logError("updateproduct db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            return new Tuple<bool, string>(true, "");
        }



        public Tuple<bool, string> IncreaseProductAmount(int productId, int amount, int storeid, bool saveCahnges)
        {
            // purpose: add amount to the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (amount < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.productAmountErrMsg);
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);
            }

            if (!InvProducts.ContainsKey(productId))
            {
                Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }


            int currentAmount = InvProducts[productId].Item2;
            try
            {
                //DB Update InventoryItem amount
                DbManager.Instance.UpdateInventoryItem(DbManager.Instance.GetDbInventoryItem(productId, storeid), currentAmount + amount, saveCahnges);
            }
            catch (Exception ex)
            {
                Logger.logError("IncreaseProductAmount db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            InvProducts[productId] = new Tuple<Product, int>(InvProducts[productId].Item1, currentAmount + amount);
            return new Tuple<bool, string>(true, "");
        }


     

        public Tuple<bool, string> DecraseProductAmount(int productId, int amount, int storeid, bool saveChanges)
        {
            // purpose: decrase amount from the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (amount < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.productAmountErrMsg);
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);
            }

            if (!InvProducts.ContainsKey(productId))
            {
                Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            int currentAmount = InvProducts[productId].Item2;
            int newAmount = currentAmount - amount;
            if (newAmount < 0)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.productAmountErrMsg + " amount after decrase is less than 0");
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);
            }

            try
            {
                //DB updating inventoryItem
                DbManager.Instance.UpdateInventoryItem(DbManager.Instance.GetDbInventoryItem(productId, storeid), newAmount, saveChanges);
            }
            catch (Exception ex)
            {
                Logger.logError("decreaseProductAmount db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            InvProducts[productId] = new Tuple<Product, int>(InvProducts[productId].Item1, newAmount);
            return new Tuple<bool, string>(true, "");
        }


        //return the product and it's amount in the inventory
        public Tuple<Product, int> getProductDetails(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!InvProducts.ContainsKey(productId))
            {
                Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
            else
                return InvProducts[productId];
        }


        public static Tuple<bool, string> isValidInventory(Dictionary<int, Tuple<Product, int>> inv)
        {

            if (inv == null)
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NullInventroyErrMsg);
            }
            //checking the amount of each product is not negative and each key matches the product id
            foreach (KeyValuePair<int, Tuple<Product, int>> entry in inv)
            {
                int productId = entry.Value.Item1.Id;
                int productAmount = entry.Value.Item2;
                if (productAmount < 0)
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg);

                }
                if (productId != entry.Key)
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.UnmatchedProductAnKeyErrMsg);
                }
            }

            return new Tuple<bool, string>(true, "");
        }



        public Tuple<bool, string> loadInventory(Dictionary<int, Tuple<Product, int>> otherInv)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Tuple<bool, string> isValidAns = isValidInventory(otherInv);
            bool isValid = isValidAns.Item1;
            if (isValid)
            {
                InvProducts = otherInv;
                return new Tuple<bool, string>(true, null);
            }
            else
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.InvalidInventory);
                return new Tuple<bool, string>(false, isValidAns.Item2);
            }
        }
        
     
        public Tuple<bool, string> isValidBasket(Dictionary<int, int> basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            foreach (KeyValuePair<int, int> entry in basket)
            {
                int id = entry.Key;
                int amount = entry.Value;
                if (!InvProducts.ContainsKey(id))
                {
                    Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " - PID : " + id.ToString(), this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " - PID : " + id.ToString());
                }

                int invProductAmount = InvProducts[id].Item2;
                if (invProductAmount < amount)
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.InventoryErrorMessage.ProductShortageErrMsg + " - PID : " + id.ToString());
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductShortageErrMsg + " - PID : " + id.ToString());

                }
            }

            return new Tuple<bool, string>(true, "");
        }

        public double getBasketPrice(Dictionary<int, int> products)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            double basketPrice = 0;
            foreach(KeyValuePair<int, int> entry in products)
            {
                int prod_id = entry.Key;
                int amount = entry.Value;
                if (!this.InvProducts.ContainsKey(prod_id))
                {
                    Logger.logError(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " - PID : " + prod_id, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return -1;

                }
                else
                    basketPrice += this.InvProducts[prod_id].Item1.Price * amount;
            }

            return basketPrice;
        }

        public bool productExist(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return InvProducts.ContainsKey(productId);
        }
    }
}