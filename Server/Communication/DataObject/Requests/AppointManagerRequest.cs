using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class AppointManagerRequest : Message
    {
        public string Appointer { get; set; }
        public string Appointed { get; set; }
        public int StoreId { get; set; }

        public AppointManagerRequest(string appointer, string appointed, int storeID) : base(Opcode.APPOINT_MANAGER)
        {
            Appointer = appointer;
            Appointed = appointed;
            StoreId = storeID;
        }
    }
}
