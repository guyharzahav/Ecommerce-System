using eCommerce_14a.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class SearchProductResponse : Message
    {
        public Dictionary<int, List<ProductData>> SearchResults { get; set; }
        
        public SearchProductResponse(Dictionary<int, List<ProductData>> searchResults) : base(Opcode.RESPONSE)
        {
            SearchResults = searchResults;
        }

        public SearchProductResponse() : base()
        {

        }
    }
}
