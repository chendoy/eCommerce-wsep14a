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
        List<Store> _stores { get; set; }

        public GetStoresResponse(List<Store> Stores): base(Opcode.RESPONSE)
        {
            _stores = Stores;
        }

    }
}
