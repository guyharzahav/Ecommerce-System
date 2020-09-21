using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class IncreaseProductAmountRequest : Message
    {
        public int StoreId { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public IncreaseProductAmountRequest(int storeId, string username, int productId, int amount) : base(Opcode.INCREASE_PRODUCT_AMOUNT)
        {
            StoreId = storeId;
            Username = username;
            ProductId = productId;
            Amount = amount;
        }
    }
}
