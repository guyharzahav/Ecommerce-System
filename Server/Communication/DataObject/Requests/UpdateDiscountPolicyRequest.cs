using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class UpdateDiscountPolicyRequest : Message
    {
        public int storeId { get; set; }
        public string userName { get; set; }
        public string DiscountPolicy { get; set; }

        public UpdateDiscountPolicyRequest(int storeId, string userName, string discountPolicy) : base(Opcode.UPDATE_DISCOUNT_POLICY)
        {
            this.storeId = storeId;
            this.userName = userName;
            this.DiscountPolicy = discountPolicy;
        }

        public UpdateDiscountPolicyRequest() : base()  { }
        
    }
}
