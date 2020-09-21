using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class SuccessFailResponse : Message
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public SuccessFailResponse(bool success, string error) : base(Opcode.RESPONSE)
        {
            Success = success;
            Error = error;
        }

        public SuccessFailResponse() : base() { }
    }
}
