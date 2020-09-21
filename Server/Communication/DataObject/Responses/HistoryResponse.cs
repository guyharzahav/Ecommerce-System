using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
       public class HistoryResponse : Message
    {
        public List<PurchaseData> HistoryItems { get; set; } // not sure about that
        public string Error;

        public HistoryResponse(List<PurchaseData> historyItems, string error) : base(Opcode.RESPONSE)
        {
            HistoryItems = historyItems;
            this.Error = error;
        }

        public HistoryResponse() : base()
        {

        }
    }
}
