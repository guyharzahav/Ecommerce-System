using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetApprovalListResponse : Message
    {

        public GetApprovalListResponse()  : base(Opcode.RESPONSE) { }
        public GetApprovalListResponse(List<string> usersToApprove) : base(Opcode.RESPONSE)
        {
            UsersToApprove = usersToApprove;
        }

        public List<string> UsersToApprove { get; set; }
    }
}
