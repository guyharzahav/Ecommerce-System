using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetStoreHistoryRequest : Message
    {
        public string Username { get; set; }
        public int StoreId { get; set; }

        public GetStoreHistoryRequest(string username, int storeId) : base(Opcode.STORE_HISTORY)
        {
            Username = username;
            StoreId = storeId;
        }

    }
}
