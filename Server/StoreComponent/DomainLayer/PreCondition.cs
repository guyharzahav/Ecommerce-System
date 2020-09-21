using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System.ComponentModel.DataAnnotations;
using eCommerce_14a.UserComponent.DomainLayer;

namespace Server.StoreComponent.DomainLayer
{
    public class PreCondition
    {
        public int preCondNumber {set; get;}
        public PreCondition(int num)
        {
            this.preCondNumber = num;
        }


        public virtual bool IsFulfilledProductPriceAboveEqXDiscount(PurchaseBasket basket, int productId, double minPrice)
        {
            return false;
        }

        public virtual bool IsFufillledMinProductUnitDiscount(PurchaseBasket basket, int productId, int minUnits)
        {
            return false;
        }

        
        public virtual bool IsFulfilledMinBasketPriceDiscount(PurchaseBasket basket, double minPrice)
        {
            return false;
        }

        public virtual bool IsFulfilledMinUnitsAtBasketDiscount(PurchaseBasket basket, int minUnits)
        {
            return false;
        }

        
        public virtual bool IsFulfilledMaxUnitsOfProductPurchase(PurchaseBasket basket, int productId, int maxAmount)
        {
            return false;
        }

        
        public virtual bool IsFulfilledMinUnitsOfProductTypePurchase(PurchaseBasket basket, int productId, int minAmount)
        {
            return false;
        }

        public virtual bool IsFulfilledMaxItemAtBasketPurchase(PurchaseBasket basket, int maxitems)
        {
            return false;
        }

        
        public virtual bool IsFulfilledMinItemAtBasketPurchase(PurchaseBasket basket, int minItems)
        {
            return false;
        }


        
        public virtual bool IsFulfilledStoreMustBeActivePurchase(int sid)
        {
            return false;
        }
        
        public virtual bool IsFulfilledOwnerCantBuyPurchase(string username, int sid)
        {
            return false;
        }

        public virtual bool IsFulfilledMinBasketPricePurchase(PurchaseBasket basket, double minPrice)
        {
            return false;
        }

        
        public virtual bool IsFulfilledMaxBasketPricePurchase(PurchaseBasket basket, double maxPrice)
        {
            return false;
        }

        public int PreConditionNumber
        {
            get { return preCondNumber; }
        }

    }

    public class DiscountPreCondition : PreCondition
    {
        public DiscountPreCondition(int num) : base (num)
        {

        }
           
        override
        public bool IsFulfilledProductPriceAboveEqXDiscount(PurchaseBasket basket, int productId, double minPrice)
        {
            if (basket.Store.Inventory.InvProducts.ContainsKey(productId))
            {
                return basket.Store.Inventory.InvProducts[productId].Item1.Price >= minPrice;
            }
            return false;
        }

        override
        public bool IsFufillledMinProductUnitDiscount(PurchaseBasket basket, int productId, int minUnits)
        {
            if(basket.products.ContainsKey(productId))
            {
                return basket.products[productId] >= minUnits;
            }
            return false;
        }

        override
        public  bool IsFulfilledMinBasketPriceDiscount(PurchaseBasket basket, double minPrice)
        {
            return basket.GetBasketOrigPrice() >= minPrice;
        }

        override
        public  bool IsFulfilledMinUnitsAtBasketDiscount(PurchaseBasket basket, int minUnits)
        {
            int count = 0;
            foreach(int amount in basket.products.Values)
            {
                count += amount;
            }
            return count >= minUnits;
        }

    }

    public class PurchasePreCondition : PreCondition
    {

        public static int MinBasketPrice = 7;
        public static int MaxBasketPrice = 8;

        override
        public bool IsFulfilledMaxUnitsOfProductPurchase(PurchaseBasket basket, int productId, int maxAmount)
        {
            
            if(!basket.Products.ContainsKey(productId))
            {
                return true;
            }

            return basket.Products[productId] <= maxAmount;
        }

        override
        public bool IsFulfilledMinUnitsOfProductTypePurchase(PurchaseBasket basket, int productId, int minAmount)
        {
            if(minAmount == 0)
            {
                return true;
            }

            if(!basket.Products.ContainsKey(productId))
            {
                return false;
            }

            return basket.Products[productId] >= minAmount;
        }

        override
        public bool IsFulfilledMaxItemAtBasketPurchase(PurchaseBasket basket, int maxitems)
        {
            int totalamount = 0;
            foreach(int amount in basket.Products.Values)
            {
                totalamount += amount;
            }
            return totalamount <= maxitems;
        }

       override
       public bool IsFulfilledMinItemAtBasketPurchase(PurchaseBasket basket, int minItems)
        {
            int totalamount = 0;
            foreach (int amount in basket.Products.Values)
            {
                totalamount += amount;
            }
            return totalamount >= minItems;
        }


        override
        public bool IsFulfilledStoreMustBeActivePurchase(int sid)
        {
            return StoreManagment.Instance.getStore(sid).ActiveStore;
        }

        override
        public bool IsFulfilledOwnerCantBuyPurchase(string username, int sid)
        {
            if(StoreManagment.Instance.getStore(sid).owners.Contains(username))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        override
        public bool IsFulfilledMinBasketPricePurchase(PurchaseBasket basket, double minPrice)
        {
            return basket.GetBasketPriceWithDiscount() >= minPrice;
        }

        override
        public bool IsFulfilledMaxBasketPricePurchase(PurchaseBasket basket, double maxPrice)
        {
            return basket.GetBasketPriceWithDiscount() <= maxPrice;
        }

        public PurchasePreCondition(int num): base (num)
        {

        }

   
    }






 




}
