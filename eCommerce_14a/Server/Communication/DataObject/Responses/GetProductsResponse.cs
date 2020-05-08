using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DomainLayer.ThinObjects;

namespace Server.Communication.DataObject
{
    public class GetProductsResponse : Message
    {
        List<Product> Products { get; set; }

        public GetProductsResponse(List<Product> products) : base(Opcode.RESPONSE)
        {
            Products = products;
        }

        public GetProductsResponse() : base()
        {

        }
    }
}
