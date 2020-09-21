using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class RemoveProductFromStoreRequest : Message
    {
        public int storeId { get; set; }
        public string userName { get; set; }
        public int productId { get; set; }
        public RemoveProductFromStoreRequest(int storeId, string userName, int productId) : base(Opcode.REMOVE_PRODUCT_FROM_STORE)
        {
            this.storeId = storeId;
            this.userName = userName;
            this.productId = productId;
        }
    }
}
