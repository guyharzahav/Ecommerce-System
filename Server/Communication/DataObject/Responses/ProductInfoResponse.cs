using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class ProductInfoResponse : Message
    {
        public ProductData Product { get; set; }
        public ProductInfoResponse(ProductData p) : base(Opcode.RESPONSE)
        {
            Product = p;
        }

        public ProductInfoResponse() : base()
        {

        }
    }
}
