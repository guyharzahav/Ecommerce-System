using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class LoginAsGuestResponse : Message
    {
        public bool Success { get; set; }
        public string Username { get; set; }
        public LoginAsGuestResponse(bool success, string username): base(Opcode.RESPONSE)
        {
            Success = success;
            Username = username;
        }

       
    }
}
