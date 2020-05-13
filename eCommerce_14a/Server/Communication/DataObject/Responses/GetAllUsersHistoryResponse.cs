using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllUsersHistoryResponse : Message
    {
        public Dictionary<string, List<PurchaseData>> Purchases { get; set; }
        public string Error { get; set; }

        public GetAllUsersHistoryResponse(Dictionary<string, List<PurchaseData>> purchases, string error) : base(Opcode.RESPONSE)
        {
            Purchases = purchases;
            Error = error;
        }
    }
}
