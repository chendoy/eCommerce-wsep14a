using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Communication.DataObject.ThinObjects;

namespace Server.Communication.DataObject
{
    public class GetProductsResponse : Message
    {
        List<ProductData> Products { get; set; }

        public GetProductsResponse(List<ProductData> products) : base(Opcode.RESPONSE)
        {
            Products = products;
        }

        public GetProductsResponse() : base()
        {

        }
    }
}
