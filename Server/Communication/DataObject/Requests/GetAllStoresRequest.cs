using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class GetAllStoresRequest : Message
    {
        public GetAllStoresRequest() : base(Opcode.ALL_STORES)
        {

        }
    }
}
