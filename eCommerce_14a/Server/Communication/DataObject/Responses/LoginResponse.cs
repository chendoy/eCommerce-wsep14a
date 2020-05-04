using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    class LoginResponse : Message
    {
        bool Success { get; set; }

        public LoginResponse(bool success) : base(Opcode.RESPONSE)
        {
            this.Success = Success;
        }
    }
}
