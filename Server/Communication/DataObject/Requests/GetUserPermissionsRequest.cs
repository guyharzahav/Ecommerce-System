using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetUserPermissionsRequest : Message
    {

        public GetUserPermissionsRequest() : base(Opcode.GET_USER_PERMISSIONS){ }
        public GetUserPermissionsRequest(string username) : base(Opcode.GET_USER_PERMISSIONS)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
