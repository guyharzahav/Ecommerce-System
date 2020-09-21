using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class RegisterRequest : Message
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterRequest(string username, string password) : base(Opcode.REGISTER)
        {
            Username = username;
            Password = password;
        }
    }
}
