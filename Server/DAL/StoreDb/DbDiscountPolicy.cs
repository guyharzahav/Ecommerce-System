using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbDiscountPolicy
    {
        [Key]
        public int Id { set; get; }

        [ForeignKey("Store")]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }

        public int? MergeType { set; get; }
        
        public int? ParentId { set; get; }

        public int? PreConditionNumber { set; get; }

        [ForeignKey("Product")]
        public int? DiscountProductId { set; get; }
        public DbProduct Product { set; get; }

        public double? Discount { set; get; }

        public int DiscountType { set; get; }

        public int? MinProductUnits { set; get; }

        public double? MinBasketPrice { set; get; }

        public double? MinProductPrice { set; get; }

        public int? MinUnitsAtBasket { set; get; }



        public DbDiscountPolicy (int storeid, int? mergetype, int? parentId, int? preConditionnumber, int? discountproductid, double? discount, int discounttype, int? minproductunits, double? minbaskeptice, double? minproductprice, int? minunitsatbasket)
        {
            StoreId = storeid;
            MergeType = mergetype;
            ParentId = parentId;
            PreConditionNumber = preConditionnumber;
            DiscountProductId = discountproductid;
            Discount = discount;
            DiscountType = discounttype;
            MinProductUnits = minproductunits;
            MinBasketPrice = minbaskeptice;
            MinProductPrice = minproductprice;
            MinUnitsAtBasket = minunitsatbasket;
        }

        public DbDiscountPolicy()
        {

        }

    }
}
