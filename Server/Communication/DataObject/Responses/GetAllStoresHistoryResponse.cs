using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllStoresHistoryResponse : Message
    {
        public List<StoreData> Purchases { get; set; } 
        public string Error { get; set; }

        public GetAllStoresHistoryResponse(List<StoreData> purchases, string error) : base(Opcode.RESPONSE)
        {
            Purchases = purchases;
            Error = error;
        }

    }
}
