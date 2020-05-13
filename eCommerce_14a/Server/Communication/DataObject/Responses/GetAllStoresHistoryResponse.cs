using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class GetAllStoresHistoryResponse : Message
    {
        public Dictionary<StoreData, List<PurchaseBasketData>> Purchases { get; set; } 
        public string Error { get; set; }

        public GetAllStoresHistoryResponse(Dictionary<StoreData, List<PurchaseBasketData>> purchases, string error) : base(Opcode.RESPONSE)
        {
            Purchases = purchases;
            Error = error;
        }

    }
}
