using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    class OpenStoreResponse : Message
    {
        bool Success { get; set; }
        string Error { get; set; }
        int StoreId { get; set; }

        public OpenStoreResponse(bool success, string error, int storeID) : base(Opcode.RESPONSE)
        {
            this.Success = success;
            this.Error = error;
            this.StoreId = storeID;
        }
    }
}
