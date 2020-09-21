using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetManagersPermissionRequest : Message
    {
        public GetManagersPermissionRequest(int storeID, string username) : base(Opcode.GET_MANAGER_PERMISSION)
        {
            StoreID = storeID;
            this.username = username;
        }

        public int StoreID { get; set; }
        public string username { get; set; }
    }
}
