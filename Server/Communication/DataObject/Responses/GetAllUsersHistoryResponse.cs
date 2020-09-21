using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllUsersHistoryResponse : Message
    {
        public List<string> Purchases { get; set; }
        public string Error { get; set; }

        public GetAllUsersHistoryResponse(List<string> purchases, string error) : base(Opcode.RESPONSE)
        {
            Purchases = purchases;
            Error = error;
        }
    }
}
