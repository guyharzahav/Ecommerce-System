using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetAllUsersHistoryRequest : Message
    {
        public string Admin { get; set; }

        public GetAllUsersHistoryRequest(string admin) : base(Opcode.ALL_BUYERS_HISTORY)
        {
            Admin = admin;
        }
    }
}
