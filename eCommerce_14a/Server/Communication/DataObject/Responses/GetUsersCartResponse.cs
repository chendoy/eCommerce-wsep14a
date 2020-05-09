using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetUsersCartResponse : Message
    {
        public CartData Cart { get; set; }
        public GetUsersCartResponse(CartData cart) : base(Opcode.RESPONSE)
        {
            Cart = cart;
        }

        public GetUsersCartResponse() : base()
        {

        }
    }
}
