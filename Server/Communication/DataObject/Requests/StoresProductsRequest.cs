using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class StoresProductsRequest : Message
    {
        public int StoreId { get; set; }
        public StoresProductsRequest(int storeID) : base(Opcode.PRODUCTS_OF_STORE)
        {
            StoreId = storeID;
        }
    }
}
