using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class MakeAdminRequest : Message
    {
        public MakeAdminRequest() : base(Opcode.MAKE_ADMIN) { }
        public MakeAdminRequest(string username) : base(Opcode.MAKE_ADMIN)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
