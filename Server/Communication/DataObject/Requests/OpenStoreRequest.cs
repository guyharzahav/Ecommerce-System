using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class OpenStoreRequest : Message
    {
        public string Username { get; set; }
        public string StoreName { get; set; }

        public OpenStoreRequest(string username, string storename = "Store") : base(Opcode.OPEN_STORE)
        {
            Username = username;
            StoreName = storename;
        }
    }
}
