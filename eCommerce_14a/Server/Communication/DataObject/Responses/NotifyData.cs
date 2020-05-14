using Server.Communication.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.Communication
{
    public class NotifyData : Message
    {
        public string Context {get; set;}
        public NotifyData(string context) : base(Opcode.NOTIFICATION) 
        {
            this.Context = context;
        }

        public NotifyData() : base()
        {

        }
    }
}
