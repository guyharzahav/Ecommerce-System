using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class AppointOwnerRequest : Message
    {
        public string Appointer { get; set; }
        public string Appointed { get; set; }
        public int StoreId { get; set; }

        public AppointOwnerRequest(string appointer, string appointed, int storeId) : base(Opcode.APPOINT_OWNER)
        {
            Appointer = appointer;
            Appointed = appointed;
            StoreId = storeId;
        }
    }
}
