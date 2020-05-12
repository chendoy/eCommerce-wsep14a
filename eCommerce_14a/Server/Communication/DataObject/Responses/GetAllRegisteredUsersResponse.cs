using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllRegisteredUsersResponse : Message
    {
        List<string> Users { get; set; }
        string Error { get; set; }

        public GetAllRegisteredUsersResponse(List<string> users, string error) : base(Opcode.RESPONSE)
        {
            this.Users = users;
            this.Error = error;
        }

        public GetAllRegisteredUsersResponse() : base()
        {

        }
    }
}
