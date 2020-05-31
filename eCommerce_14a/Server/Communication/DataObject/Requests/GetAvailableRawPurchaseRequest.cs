using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    class GetAvailableRawPurchaseRequest : Message
    {
        public GetAvailableRawPurchaseRequest() : base(Opcode.GET_AVAILABLE_PURCHASES) {  }
    }
}
