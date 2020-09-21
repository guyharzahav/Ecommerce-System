using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class LogoutResponse : Message
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public LogoutResponse(bool success, string error) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
        }

        public LogoutResponse() : base()
        {

        }
    }
}
