using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    class LogoutResponse : Message
    {
        bool Success { get; set; }
        string Error { get; set; }

        public LogoutResponse(bool success, string error) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
        }
    }
}
