using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.Communication
{
    public class Message
    {
        private string context;
        public Message(string context)
        {
            this.context = context;
        }
        string getContext()
        {
            return context;
        }
    }
}
