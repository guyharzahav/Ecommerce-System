using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetStoreByIdResponse : Message
    {
        public StoreData Store { get; set; }

        public GetStoreByIdResponse(StoreData store) : base(Opcode.RESPONSE)
        {
            Store = store;
        }
    }
}
