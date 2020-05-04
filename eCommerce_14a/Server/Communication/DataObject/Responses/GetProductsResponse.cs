using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    class GetProductsResponse : Message
    {
        List<Product> _products { get; set; }

        public GetProductsResponse(List<Product> products) : base(Opcode.RESPONSE)
        {
            _products = products;
        }
    }
}
