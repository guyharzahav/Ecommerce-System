using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class ApproveAppointmentRequest : Message
    {
        public ApproveAppointmentRequest() : base(Opcode.APPROVE_APPOINTMENT) { }
        public ApproveAppointmentRequest(string owner, string appointed, int storeID, bool approval) : base(Opcode.APPROVE_APPOINTMENT)
        {
            Owner = owner;
            Appointed = appointed;
            StoreID = storeID;
            Approval = approval;
        }

        public string Owner { get; set; }
        public string Appointed { get; set; }
        public int StoreID { get; set; }
        public bool Approval { get; set; }

    }
}
