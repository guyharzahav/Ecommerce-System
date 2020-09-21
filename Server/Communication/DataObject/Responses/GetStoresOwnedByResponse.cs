using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetStoresOwnedByResponse : Message
    {
        public List<StoreData> Stores { get; set; }
        public string Error { get; set; }

        public GetStoresOwnedByResponse(List<StoreData> stores, string error) : base(Opcode.RESPONSE)
        {
            this.Stores = stores;
            this.Error = error;
        }
        public GetStoresOwnedByResponse() : base()
        {

        }
    }
}
