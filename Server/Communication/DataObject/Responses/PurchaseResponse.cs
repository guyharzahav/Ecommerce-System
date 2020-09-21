using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class PurchaseResponse : Message
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public PurchaseResponse(bool success, string error) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
        }
        public PurchaseResponse() : base()
        {

        }
    }
}
