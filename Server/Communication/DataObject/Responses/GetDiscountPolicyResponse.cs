using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetDiscountPolicyResponse : Message
    {
        public GetDiscountPolicyResponse() : base(Opcode.RESPONSE) { }
        public GetDiscountPolicyResponse(string discountPolicy) : base(Opcode.RESPONSE)
        {
            DiscountPolicy = discountPolicy;
        }

        public string DiscountPolicy { get; set; }
    }
}
