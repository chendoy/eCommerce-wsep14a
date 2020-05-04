using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
        class AppointOwnerResponse : Message
        {
            bool Success { get; set; }

            public AppointOwnerResponse(bool success) : base(Opcode.RESPONSE)
            {
                this.Success = Success;
            }
        }
}
