using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class BuyerHistoryRequest : Message
    {
        public string Username { get; set; }

        public BuyerHistoryRequest(string username) : base(Opcode.BUYER_HISTORY)
        {
            Username = username;
        }
    }
}
