using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class GetStoresResponse : Message
    {
        List<Store> Stores { get; set; }

        public GetStoresResponse(List<Store> stores): base(Opcode.RESPONSE)
        {
            Stores = stores;
        }

        public GetStoresResponse() : base()
        {

        }

    }
}
