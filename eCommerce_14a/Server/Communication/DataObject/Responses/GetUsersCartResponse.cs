using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetUsersCartResponse : Message
    {
        Cart _cart { get; set; }
        public GetUsersCartResponse(Cart cart) : base(Opcode.RESPONSE)
        {
            _cart = cart;
        }
    }
}
