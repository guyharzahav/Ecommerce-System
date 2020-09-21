using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class DemoteOwnerRequest : Message
    {
        public string Appointer { get; set; }
        public string Appointed { get; set; }
        public int StoreId { get; set; }

        public DemoteOwnerRequest(string appointer, string appointed, int storeID) : base(Opcode.DEMOTE_OWNER)
        {
            Appointer = appointer;
            Appointed = appointed;
            StoreId = storeID;
        }
    }
}
