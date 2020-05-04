using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    class HistoryResponse : Message
    {
        List<Purchase> _historyItems { get; set; } // not sure about that

        public HistoryResponse(List<Purchase> historyItems) : base(Opcode.RESPONSE)
        {
            _historyItems = historyItems;
        }
    }
}
