using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    class SuccessFailResponse : Message
    {
        bool Success { get; set; }
        string Error { get; set; }
        public SuccessFailResponse(bool success, string error) : base(Opcode.RESPONSE)
        {
            Success = success;
            Error = error;
        }

        public SuccessFailResponse() : base() { }
    }
}
