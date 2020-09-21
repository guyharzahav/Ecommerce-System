using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetAllStoresHistoryRequest : Message
    {
        public string Admin { get; set; }

        public GetAllStoresHistoryRequest(string admin) : base(Opcode.ALL_STORE_HISTORY)
        {
            Admin = admin;
        }
    }
}
