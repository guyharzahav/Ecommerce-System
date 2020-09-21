using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetStoresOwnedByRequest : Message
    {
        public string User { get; set; }

        public GetStoresOwnedByRequest(string user) : base(Opcode.STORES_OWNED_BY)
        {
            User = user;
        }
    }
}
