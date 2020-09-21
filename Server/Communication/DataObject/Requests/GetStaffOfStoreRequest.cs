using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetStaffOfStoreRequest : Message
    {

        public int StoreId { get; set; }

        public GetStaffOfStoreRequest(int storeId) : base(Opcode.GET_STAFF_OF_STORE)
        {
            StoreId = storeId;
        }

        public GetStaffOfStoreRequest() : base() { }

    }
}
