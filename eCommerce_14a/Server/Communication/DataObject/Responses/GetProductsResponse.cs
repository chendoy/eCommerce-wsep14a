using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 

namespace Server.Communication.DataObject
{
    public class GetProductsResponse : Message
    {
        List<Product> _products { get; set; }

        public GetProductsResponse(List<Product> products) : base(Opcode.RESPONSE)
        {
            _products = products;
        }

        public GetProductsResponse() : base()
        {

        }
    }
}
