using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    class RegisterResponse : Message
    {
        bool Success { get; set; }

        public RegisterResponse(bool success) : base(Opcode.RESPONSE)
        {
            this.Success = Success;
        }
    }
}
