using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class DemoteManagerRequest : Message
    {
        public string Appointer { get; set; }
        public string Appointed { get; set; }
        public int StoreId { get; set; }

        public DemoteManagerRequest(string appointer, string appointed, int storeID) : base(Opcode.DEMOTE_MANAGER)
        {
            Appointer = appointer;
            Appointed = appointed;
            StoreId = storeID;
        }
    }
}
