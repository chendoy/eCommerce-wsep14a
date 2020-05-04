using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    class PurchaseResponse : Message
    {
        int _itemsPurchased { get; set; } // how many items were actually purchased
                                          // additional fields here?
        public PurchaseResponse(int itemsPurchased) : base(Opcode.RESPONSE)
        {
            _itemsPurchased = itemsPurchased;
        }
    }
}
