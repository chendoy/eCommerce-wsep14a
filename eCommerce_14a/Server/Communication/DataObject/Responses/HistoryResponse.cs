using Server.DomainLayer.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
       public class HistoryResponse : Message
    {
        List<Purchase> HistoryItems { get; set; } // not sure about that
        string Error;

        public HistoryResponse(List<Purchase> historyItems, string error) : base(Opcode.RESPONSE)
        {
            HistoryItems = historyItems;
            this.Error = error;
        }

        public HistoryResponse() : base()
        {

        }
    }
}
