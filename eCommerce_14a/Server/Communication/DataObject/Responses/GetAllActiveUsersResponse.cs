using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllActiveUsersResponse : Message
    {
        public List<string> Users { get; set; }
        public string Error { get; set; }

        public GetAllActiveUsersResponse(List<string> users, string error) : base(Opcode.RESPONSE)
        {
            this.Users = users;
            this.Error = error;
        }

        public GetAllActiveUsersResponse() : base()
        {

        }
    }
}
