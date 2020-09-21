using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class LoginRequest : Message
    {
        public string Username { get; set; }
        public string Password { get; set; }
        

        public LoginRequest(string username = "", string password = "") : base(Opcode.LOGIN)
        {
            if (username == null || password == null)
            {
                Username = "";
                Password = "";
            }
            Username = username;
            Password = password;
        }
    }
}
