using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class DemoteRequest : Message
    {
        public string Appointer { get; set; }
        public string Appointed { get; set; }
        public int StoreId { get; set; }

        public DemoteRequest(string appointer, string appointed, int storeID) : base(Opcode.DEMOTE)
        {
            Appointer = appointer;
            Appointed = appointed;
            StoreId = storeID;
        }
    }
}
