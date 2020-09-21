using Server.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class ChangePermissionsRequest : Message
    {
        public string Owner { get; set; }
        public string Appoint { get; set; }
        public int StoreId { get; set; }
        public Permission Permissions { get; set; }

        public ChangePermissionsRequest() : base(Opcode.CHANGE_PERMISSIONS)
        {

        }

        public ChangePermissionsRequest(string owner, string appoint, int storeId, Permission permissions) : base(Opcode.CHANGE_PERMISSIONS)
        {
            Owner = owner;
            Appoint = appoint;
            StoreId = storeId;
            Permissions = permissions;
        }
    }
}
