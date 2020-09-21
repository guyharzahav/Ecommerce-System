using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class SearchProductRequest : Message
    {

        public Dictionary<string, object> Filters { get; set; }

        public SearchProductRequest(Dictionary<string, object> filters) : base(Opcode.SEARCH_PROD)
        {
            Filters = filters;
        }
    }
}
