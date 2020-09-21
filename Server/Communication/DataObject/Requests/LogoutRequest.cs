using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class LogoutRequest : Message
    {
        public string Username { get; set; }

        public LogoutRequest(string username) : base(Opcode.LOGOUT)
        {
            Username = username;
        }
    }
}
