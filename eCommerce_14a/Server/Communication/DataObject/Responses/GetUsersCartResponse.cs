using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    class GetUsersCartResponse : Message
    {
        List<Product> _cart { get; set; }
        public GetUsersCartResponse(List<Product> cart) : base(Opcode.RESPONSE)
        {
            _cart = cart;
        }
    }
}
