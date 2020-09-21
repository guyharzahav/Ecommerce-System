using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAvailableRawDiscountsResponse : Message
    {
        public Dictionary<int, string> DiscountPolicies { get; set; }

        public GetAvailableRawDiscountsResponse(Dictionary<int, string> discountPolicies) : base(Opcode.RESPONSE)
        {
            DiscountPolicies = discountPolicies;
        }
    }
}
