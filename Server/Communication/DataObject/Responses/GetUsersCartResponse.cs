using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetUsersCartResponse : Message
    {
        public CartData Cart { get; set; }
        public string Error { get; set; }
        public GetUsersCartResponse(CartData cart, string error) : base(Opcode.RESPONSE)
        {
            Cart = cart;
            Error = error;
        }

        public GetUsersCartResponse() : base()
        {

        }
    }
}
