using Server.DomainLayer.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class SearchProductResponse : Message
    {
        List<Product> SearchResults { get; set; }
        
        public SearchProductResponse(List<Product> searchResults) : base(Opcode.RESPONSE)
        {
            SearchResults = searchResults;
        }

        public SearchProductResponse() : base()
        {

        }
    }
}
