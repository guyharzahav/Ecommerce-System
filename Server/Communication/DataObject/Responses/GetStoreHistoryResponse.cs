using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetStoreHistoryResponse : Message
    {
        public List<PurchaseBasketData> Baskets { get; set; }

        public string Error { get; set; }

        public GetStoreHistoryResponse(List<PurchaseBasketData> baskets, string error)
        {
            Baskets = baskets;
            Error = error;
        }
    }
}
