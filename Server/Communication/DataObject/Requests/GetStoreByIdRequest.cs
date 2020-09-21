using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetStoreByIdRequest : Message
    {
        public int StoreId { get; set; }

        public GetStoreByIdRequest(int storeId) : base(Opcode.STORE_BY_ID)
        {
            StoreId = storeId;
        }

    }
}
