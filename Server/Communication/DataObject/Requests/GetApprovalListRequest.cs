using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetApprovalListRequest : Message
    {
        public GetApprovalListRequest() : base(Opcode.GET_APPROVAL_LIST) { }
        public GetApprovalListRequest(int storeID, string username) : base(Opcode.GET_APPROVAL_LIST)
        {
            StoreID = storeID;
            Username = username;
        }

        public int StoreID { get; set; }
        public string Username { get; set; }
    }
}
