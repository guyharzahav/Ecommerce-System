using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class OpenStoreResponse : Message
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public int StoreId { get; set; }

        public OpenStoreResponse(bool success, string error, int storeID) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
            this.StoreId = storeID;
        }

        public OpenStoreResponse() : base()
        {

        }
    }
}
