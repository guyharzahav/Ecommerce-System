using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAvailableRawPurchaseResponse : Message
    {
        public GetAvailableRawPurchaseResponse() : base(Opcode.RESPONSE) { }

        public GetAvailableRawPurchaseResponse(Dictionary<int, string> rawPurchases) : base(Opcode.RESPONSE)
        {
            RawPurchases = rawPurchases;
        }

        public Dictionary<int, string> RawPurchases { get; set; }
    }
}
