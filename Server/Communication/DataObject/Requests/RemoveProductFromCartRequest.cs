using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class RemoveProductFromCartRequest : Message
    {
        public string Username { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public RemoveProductFromCartRequest(string username, int storeId, int productId) : base(Opcode.REMOVE_PRODUCT_FROM_CART)
        {
            Username = username;
            StoreId = storeId;
            ProductId = productId;
        }


    }
}
