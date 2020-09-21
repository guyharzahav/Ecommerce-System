using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    //public static int NoDiscount = 0;
    //public static int ProductPriceAbove100 = 1;
    //public static int ProductPriceAbove200 = 2;
    //public static int Above1Unit = 3;
    //public static int Above2Units = 4;
    //public static int basketPriceAbove1000 = 5;

    public class DiscountPolicyData
    {
    }

    public class CompoundDiscountPolicyData : DiscountPolicyData
    {
        public CompoundDiscountPolicyData(int mergeType, List<DiscountPolicyData> discountChildren)
        {
            MergeType = mergeType;
            DiscountChildren = discountChildren;
        }

        public int MergeType { get; set; }
        public List<DiscountPolicyData> DiscountChildren { get; set; }
        //{{{1 xor 2 } or 3} xor 4}
    }

    public class Discount : DiscountPolicyData
    {
        public int PreCondition { get; set; }

        public double DiscountPercent { get; set; }

        public Discount()
        {
        }

        public Discount(int discountType, double discountPrecent)
        {
            PreCondition = discountType;
            DiscountPercent = discountPrecent;
        }
    }

    public class DiscountConditionalProductData : Discount
    {
        public int ProductId { get; set; }

        public DiscountConditionalProductData() : base()
        {
        }

        public DiscountConditionalProductData(int productId, int discountType, double discountPrecent) : base(discountType, discountPrecent)
        {
            ProductId = productId;
        }
    }

    public class DiscountConditionalBasketData : Discount
    {

        public DiscountConditionalBasketData() : base()
        {
        }
        public DiscountConditionalBasketData(int preCondition, double discount) : base(preCondition, discount)
        {
        }
    }

   public class DiscountRevealdData : DiscountPolicyData
    {
        public int ProductId { get; set; }
        public double DiscountPrecent { get; set; }

        public DiscountRevealdData() : base()
        {
        }

        public DiscountRevealdData(int productId, double discountPrecent) : base()
        {
            ProductId = productId;
            DiscountPrecent = discountPrecent;
        }
    }



}
