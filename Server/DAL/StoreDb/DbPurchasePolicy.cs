using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
   public class DbPurchasePolicy
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
        public int? PolicyProductId { set; get; }
        public DbProduct Product { set; get; }

        [ForeignKey("Buyer")]
        public string BuyerUserName { set; get; }
        public virtual DbUser Buyer { set; get; }

        public int PurchasePolicyType { set; get; }

        public int? MaxProductIdUnits { set; get; }
        
        public int? MinProductIdUnits { set; get; }
        public int? MaxItemsAtBasket { set; get; }
        public int? MinItemsAtBasket { set; get; }
        public double? MinBasketPrice { set; get; }

        public double? MaxBasketPrice { set; get; }


        public DbPurchasePolicy(int storeId, int? mergetype, int? parentid, int? preconditionnumber, int? policyproductid, string buyerusername, int purchasepolictype, int? maxproductidunits, int ? minproductidsunits, int? maxitemsatbasket, int? minitemsatbasket, double? minbasketprice, double? maxbaskeptrice)
        {
            StoreId = storeId;
            MergeType = mergetype;
            ParentId = parentid;
            PreConditionNumber = preconditionnumber;
            PolicyProductId = policyproductid;
            BuyerUserName = buyerusername;
            PurchasePolicyType = purchasepolictype;
            MaxProductIdUnits = maxproductidunits;
            MinProductIdUnits = minproductidsunits;
            MaxItemsAtBasket = maxitemsatbasket;
            MinItemsAtBasket = minitemsatbasket;
            MinBasketPrice = minbasketprice;
            MaxBasketPrice = maxbaskeptrice;
        }

        public DbPurchasePolicy()
        {

        }

    }
}
