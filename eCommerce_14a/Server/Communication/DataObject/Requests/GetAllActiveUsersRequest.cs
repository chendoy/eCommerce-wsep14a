using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetAllActiveUsersRequest : Message
    {

        public GetAllActiveUsersRequest() : base(Opcode.GET_ALL_ACTIVE_USERS)
        {
            
        }
    }
}
