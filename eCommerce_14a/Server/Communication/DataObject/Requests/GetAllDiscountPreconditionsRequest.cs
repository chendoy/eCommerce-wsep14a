using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetAllDiscountPreconditionsRequest : Message
    {

        public GetAllDiscountPreconditionsRequest() : base(Opcode.GET_ALL_PRECONDITIONS)
        {
        }
    }
}
