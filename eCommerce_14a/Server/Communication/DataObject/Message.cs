using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class Message
    {
        public int Opcode { get; set; }

        public Message(int opcode)
        {
            Opcode = opcode;
        }
    }
}
