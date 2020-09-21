using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetPurchasePolicyRequest : Message
    {
        public GetPurchasePolicyRequest() : base(Opcode.GET_PURCHASE_POLICY) { }
        public GetPurchasePolicyRequest(int storeID)  : base(Opcode.GET_PURCHASE_POLICY)
        {
            StoreID = storeID;
        }

        public int StoreID { get; set; }
    }
}
