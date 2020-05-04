using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public enum Opcode
    {
        LOGIN = 1,
        REGISTER,
        RESPONSE
    }
    public class Message
    {
        public Opcode _Opcode { get; set; }

        public Message(Opcode opcode)
        {
            _Opcode = opcode;
        }
    }
}
