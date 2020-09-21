using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class ErrorResponse : Message
    {
        string _errorMessage { get; set; }

        public ErrorResponse(string errorMessage) : base(Opcode.RESPONSE)
        {
            _errorMessage = errorMessage;
        }

        public ErrorResponse() : base()
        {

        }
    }
}
