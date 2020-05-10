using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class LoginResponse : Message
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public Dictionary<int, int[]> Permissions { get; set; }

        public LoginResponse(bool success, string error, Dictionary<int, int[]> permissions) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
            this.Permissions = permissions;
        }

        public LoginResponse() : base(){  }
    }
}
