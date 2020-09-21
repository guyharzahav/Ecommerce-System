using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class PurchasePolicyData
    {
    }

    public class CompoundPurchasePolicyData : PurchasePolicyData
    {
        public CompoundPurchasePolicyData(int mergeType, List<PurchasePolicyData> purchaseChildren)
        {
            MergeType = mergeType;
            PurchaseChildren = purchaseChildren;
        }

        public int MergeType { get; set; }
        public List<PurchasePolicyData> PurchaseChildren { get; set; }
    }


    public class ThinPurchasePolicy : PurchasePolicyData
    {
        public int PreCondition { get; set; }

        public ThinPurchasePolicy()
        {
        }

        public ThinPurchasePolicy(int preCondition)
        {
            PreCondition = preCondition;
        }
    }

    public class PurchasePolicySimple : ThinPurchasePolicy
    {

        public PurchasePolicySimple(int preCondition) : base(preCondition)
        {
        }
    }

    public class PurchasePolicyProductData : ThinPurchasePolicy
    {
        public int ProductId { get; set; }

        public PurchasePolicyProductData() : base()
        {
        }

        public PurchasePolicyProductData(int preCondition, int productId) : base(preCondition)
        {
            ProductId = productId;
        }

    }

    public class PurchasePolicyBasketData : ThinPurchasePolicy
    {

        public PurchasePolicyBasketData() : base()
        {
        }

        public PurchasePolicyBasketData(int preCondition) : base(preCondition)
        {
        }

    }

    public class PurchasePolicySystemData : ThinPurchasePolicy
    {
        public int StoreId { get; set; }
        public PurchasePolicySystemData() : base()
        {
        }

        public PurchasePolicySystemData(int preCondition, int storeId) : base(preCondition)
        {
            StoreId = storeId;
        }

    }

    public class PurchasePolicyUserData : ThinPurchasePolicy
    {
        public PurchasePolicyUserData() : base()
        {
        }

        public PurchasePolicyUserData(int preCondition) : base(preCondition)
        {

        }

    }









}



