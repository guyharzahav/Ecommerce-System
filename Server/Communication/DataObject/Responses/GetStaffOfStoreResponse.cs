using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetStaffOfStoreResponse : Message
    {
        public Dictionary<string, string> Staff { get; set; }

        public GetStaffOfStoreResponse(Dictionary<string, string> staff) : base(Opcode.RESPONSE)
        {
            Staff = staff;
        }
    }
}
