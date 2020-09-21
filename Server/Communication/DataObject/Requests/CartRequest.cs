using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class CartRequest : Message
    {
        public string Username { get; set; }

        public CartRequest(string username) : base(Opcode.USER_CART)
        {
            Username = username;
        }
    }
}
