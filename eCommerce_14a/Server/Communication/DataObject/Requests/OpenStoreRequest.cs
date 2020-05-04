using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class OpenStoreRequest : Message
    {
        public string Username { get; set; }

        public OpenStoreRequest(string username) : base(10)
        {
            Username = username;
        }
    }
}
