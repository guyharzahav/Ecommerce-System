using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.StoreComponent.DomainLayer;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace eCommerce_14a.StoreComponent.DomainLayer
{  
    public interface PurchasePolicy
    {
        bool IsEligiblePurchase(PurchaseBasket basket);
        string Describe(int depth);
    }
    public class CompundPurchasePolicy : PurchasePolicy
    {

        private List<PurchasePolicy> children;

        public int mergeType { get; set; }

        public CompundPurchasePolicy(int mergeType, List<PurchasePolicy> children)
        {
            if (children != null)
                this.children = children;
            else
                this.children = new List<PurchasePolicy>();
            this.mergeType = mergeType;
        }

        public bool IsEligiblePurchase(PurchaseBasket basket)
        {
            if (mergeType == CommonStr.PurchaseMergeTypes.AND)
            {
                foreach (PurchasePolicy child in children)
                    if (!child.IsEligiblePurchase(basket))
                        return false;
                return true;
            }
            else if (mergeType == CommonStr.PurchaseMergeTypes.OR)
            {
                foreach (PurchasePolicy child in children)
                    if (child.IsEligiblePurchase(basket))
                        return true;
                return false;
            }
            else if (mergeType == CommonStr.PurchaseMergeTypes.XOR)
            {
                // assuming xor accept only 2 inputs at most!
                if (children.Count == 0)
                    return true;
                if (children.Count > 2)
                    return false;
                if (children.Count == 1)
                    return children[0].IsEligiblePurchase(basket);
                if(children.Count == 2)
                {
                    bool firstRes = children[0].IsEligiblePurchase(basket);
                    bool secondRes = children[1].IsEligiblePurchase(basket);
                    return firstRes & !secondRes || secondRes & !firstRes;
                }
                return false;   
            }
            else
            {
                return false;
            }
        }
        public void add(PurchasePolicy purchasePolicy)
        {
            children.Add(purchasePolicy);
        }
        public void remove(PurchasePolicy purchasePolicy)
        {
            children.Remove(purchasePolicy);
        }

        public List<PurchasePolicy> getChildren()
        {
            return children;
        }

        public string Describe(int depth)
        {
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            string ret = pad + "(";
            ret += "    ";
            ret += mergeType == 0 ? "XOR " : mergeType == 1 ? "OR " : mergeType == 2 ? "AND " : "UNKNOWN ";
            foreach (PurchasePolicy policy in children)
                ret += "\n" + policy.Describe(depth + 1) + ",";
            ret += "\n" + pad + ")";
            return ret;
        }
    }

    abstract 
    public class SimplePurchasePolicy : PurchasePolicy
    {
        private PreCondition preCondition;
        public SimplePurchasePolicy(PreCondition preCondition)
        {
            this.preCondition = preCondition;
        }

        abstract
        public bool IsEligiblePurchase(PurchaseBasket basket);
        public abstract string Describe(int depth);

        public PreCondition PreCondition
        {
            get { return preCondition; }
        }
    }

    public class ProductPurchasePolicy : SimplePurchasePolicy
    {
        public int ProductId { get; set; }
        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }
        public ProductPurchasePolicy(PreCondition pre, int productId, int minAmount) : base(pre)
        {
            ProductId = productId;
            MinAmount = minAmount;
            MaxAmount = int.MinValue;
        }

        public ProductPurchasePolicy(int maxAmount, PreCondition pre , int productId) : base(pre)
        {
            ProductId = productId;
            MaxAmount = maxAmount;
            MinAmount = int.MaxValue;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket)
        {
            if(PreCondition.preCondNumber == CommonStr.PurchasePreCondition.MinUnitsOfProductType)
            {
                return PreCondition.IsFulfilledMinUnitsOfProductTypePurchase(basket, ProductId, MinAmount);   
            }
            else if(PreCondition.preCondNumber == CommonStr.PurchasePreCondition.MaxUnitsOfProductType)
            {
                return PreCondition.IsFulfilledMaxUnitsOfProductPurchase(basket, ProductId, MaxAmount);
            }
            else
            {
                return false;
            }
        }

        public override string Describe(int depth)
        {
            //Inv6entory inv = new Inventory();
            //string productStr = inv.getProductDetails(policyProductId).Item1.Name;
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            string amount = "";
            if (MinAmount != int.MaxValue)
                amount = "min amount " + MinAmount + ",";
            if (MaxAmount != int.MinValue)
                amount = "max amount " + MaxAmount + ",";
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Product Purchase Policy: " + amount + " product #" + ProductId + " - " + preStr + " ]";
        }
    }

    public class BasketPurchasePolicy : SimplePurchasePolicy
    {
        public int MaxItems { set; get; }
        public int MinItems { set; get; }
        public double MinBasketPrice { set; get; }
        public double MaxBasketPrice { set; get; }

        public BasketPurchasePolicy(PreCondition pre, int maxItems) : base(pre)
        {
            MaxItems = maxItems;
            MinItems = int.MaxValue;
            MinBasketPrice = int.MaxValue;
            MaxBasketPrice = int.MinValue;
        }

        public BasketPurchasePolicy(int minItems, PreCondition pre) : base(pre)
        {
            MinItems = minItems;
            MaxItems = int.MinValue;
            MinBasketPrice = int.MaxValue;
            MaxBasketPrice = int.MinValue;
        }

        public BasketPurchasePolicy(double minBasketPrice, PreCondition pre) : base(pre)
        {
            MinItems = int.MaxValue;
            MaxItems = int.MinValue;
            MinBasketPrice = minBasketPrice;
            MaxBasketPrice = int.MinValue;
        }
        public BasketPurchasePolicy(PreCondition pre, double maxBasketPrice) : base(pre)
        {
            MinItems = int.MaxValue;
            MaxItems = int.MinValue;
            MinBasketPrice = int.MaxValue;
            MaxBasketPrice = maxBasketPrice;
        }
        public BasketPurchasePolicy(PreCondition pre) : base(pre)
        {
            MinItems = int.MaxValue;
            MaxItems = int.MinValue;
            MinBasketPrice = int.MaxValue;
            MaxBasketPrice = int.MinValue;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket)
        {
            if(PreCondition.PreConditionNumber == CommonStr.PurchasePreCondition.allwaysTrue)
            {
                return true;
            }
            else if(PreCondition.PreConditionNumber == CommonStr.PurchasePreCondition.MaxItemsAtBasket)
            {
                return PreCondition.IsFulfilledMaxItemAtBasketPurchase(basket, MaxItems);
            }
            else if(PreCondition.preCondNumber == CommonStr.PurchasePreCondition.MinItemsAtBasket)
            {
                return PreCondition.IsFulfilledMinItemAtBasketPurchase(basket, MinItems);
            }
            else if(PreCondition.PreConditionNumber == CommonStr.PurchasePreCondition.MinBasketPrice)
            {
                return PreCondition.IsFulfilledMinBasketPricePurchase(basket, MinBasketPrice);
            }
            else if(PreCondition.PreConditionNumber == CommonStr.PurchasePreCondition.MaxBasketPrice)
            {
                return PreCondition.IsFulfilledMaxBasketPricePurchase(basket, MaxBasketPrice);
            }
            return false;
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            string amount = "";
            if (MinBasketPrice != int.MaxValue)
                amount = "min basket price - " + MinBasketPrice + ",";
            if (MaxBasketPrice != int.MinValue)
                amount = "max basket price - " + MaxBasketPrice + ",";
            if (MinItems != int.MaxValue)
                amount = "min items - " + MinItems + ",";
            if (MaxItems != int.MinValue)
                amount = "max items - " + MaxItems + ",";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Basket Purchase Policy: " + amount + " " + preStr + "]";
        }
    }

    public class SystemPurchasePolicy : SimplePurchasePolicy
    {
        public  int  StoreId { get; set; }
        public SystemPurchasePolicy(PreCondition pre,int storeId) : base(pre)
        {
            StoreId = storeId;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket)
        {
           if(PreCondition.preCondNumber == CommonStr.PurchasePreCondition.StoreMustBeActive)
            {
                return PreCondition.IsFulfilledStoreMustBeActivePurchase(StoreId);
            }
            else
            {
                return false;
            }
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[System Purchase Policy: " + preStr + " in store #" + StoreId + "]";
        }
    }

    public class UserPurchasePolicy : SimplePurchasePolicy
    {
        public UserPurchasePolicy(PreCondition pre) : base(pre)
        {
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket)
        { 
            if(PreCondition.preCondNumber == CommonStr.PurchasePreCondition.OwnerCantBuy)
            {
                return PreCondition.IsFulfilledOwnerCantBuyPurchase(basket.User, basket.Store.Id);
            }
            else
            {
                return false;
            }
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[User Purchase Policy: " + preStr + "]";
        }
    }
}