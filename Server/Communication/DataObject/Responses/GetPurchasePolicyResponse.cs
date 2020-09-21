using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetPurchasePolicyResponse : Message
    {
        public GetPurchasePolicyResponse() : base(Opcode.RESPONSE){ }
        public GetPurchasePolicyResponse(string purchasePolicy) : base(Opcode.RESPONSE)
        {
            PurchasePolicy = purchasePolicy;
        }

        public string PurchasePolicy { get; set; }
    }
}
