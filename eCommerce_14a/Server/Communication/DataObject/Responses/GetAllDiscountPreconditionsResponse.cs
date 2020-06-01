using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllDiscountPreconditionsResponse : Message
    {
        public Dictionary<int, string> preconditions { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

        public GetAllDiscountPreconditionsResponse(bool success, string error, Dictionary<int, string> preconditions) : base(Opcode.RESPONSE)
        {
            this.preconditions = preconditions;
            this.Success = success;
            this.Error = error;
        }

        public GetAllDiscountPreconditionsResponse() : base()
        {

        }
    }
}
