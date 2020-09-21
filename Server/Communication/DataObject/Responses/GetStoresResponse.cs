using eCommerce_14a.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class GetStoresResponse : Message
    {
        public List<StoreData> Stores { get; set; }

        public GetStoresResponse(List<StoreData> stores): base(Opcode.RESPONSE)
        {
            Stores = stores;
        }

        public GetStoresResponse() : base()
        {

        }

    }
}
