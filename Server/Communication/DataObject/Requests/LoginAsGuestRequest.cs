using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class LoginAsGuestRequest : Message
    {
        public LoginAsGuestRequest() : base(Opcode.LOGIN_AS_GUEST) { }
    }
}
