using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
       public class HistoryResponse : Message
    {
        List<Purchase> _historyItems { get; set; } // not sure about that
        string Error;

        public HistoryResponse(List<Purchase> historyItems, string error) : base(Opcode.RESPONSE)
        {
            _historyItems = historyItems;
            this.Error = error;
        }
    }
}
