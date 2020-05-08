using Server.DomainLayer.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class ProductInfoResponse : Message
    {
        Product Product { get; set; }
        public ProductInfoResponse(Product p) : base(Opcode.RESPONSE)
        {
            Product = p;
        }

        public ProductInfoResponse() : base()
        {

        }
    }
}
