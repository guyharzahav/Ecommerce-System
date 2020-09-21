using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetUserPermissionsResponse : Message
    {
        public GetUserPermissionsResponse() : base(Opcode.RESPONSE){ }
        public GetUserPermissionsResponse(Dictionary<int, int[]> permissions) : base(Opcode.RESPONSE)
        {
            Permissions = permissions;
        }

        public Dictionary<int, int[]> Permissions { get; set; }
    }
}
