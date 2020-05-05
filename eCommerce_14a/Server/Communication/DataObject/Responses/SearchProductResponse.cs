using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class SearchProductResponse : Message
    {
        List<Product> _searchResults { get; set; }
        
        public SearchProductResponse(List<Product> searchResults) : base(Opcode.RESPONSE)
        {
            _searchResults = searchResults;
        }
    }
}
