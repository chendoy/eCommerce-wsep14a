using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    class GetAvailableRawDiscountsRequest : Message
    {
        public GetAvailableRawDiscountsRequest() : base(Opcode.GET_AVAILABLE_DISCOUNTS)
        {
        }
    }
}
