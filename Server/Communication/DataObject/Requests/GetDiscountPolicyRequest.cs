using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetDiscountPolicyRequest : Message
    {
        public GetDiscountPolicyRequest() : base(Opcode.GET_DISCOUNT_POLICY) { }
        public GetDiscountPolicyRequest(int storeID) : base(Opcode.GET_DISCOUNT_POLICY)
        {
            StoreID = storeID;
        }

        public int StoreID { get; set; }
    }
}
