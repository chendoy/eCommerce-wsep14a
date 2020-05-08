using Server.DomainLayer.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetUsersCartResponse : Message
    {
        Cart Cart { get; set; }
        public GetUsersCartResponse(Cart cart) : base(Opcode.RESPONSE)
        {
            Cart = cart;
        }

        public GetUsersCartResponse() : base()
        {

        }
    }
}
