using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class ProductInfoResponse : Message
    {
        Product _product { get; set; }
        public ProductInfoResponse(Product p) : base(Opcode.RESPONSE)
        {
            _product = p;
        }
    }
}
