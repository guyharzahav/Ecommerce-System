using Server.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetManagerPermissionResponse : Message
    {
        public GetManagerPermissionResponse() : base(Opcode.RESPONSE)
        {

        }

        public GetManagerPermissionResponse(List<Tuple<string, Permission>> managerPermissions) : base(Opcode.RESPONSE)
        {
            ManagerPermissions = managerPermissions;
        }

        public List<Tuple<string, Permission>> ManagerPermissions { get; set; }
    }
}
