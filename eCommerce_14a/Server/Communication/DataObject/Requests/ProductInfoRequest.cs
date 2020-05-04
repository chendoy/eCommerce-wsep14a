using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class ProductInfoRequest : Message
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public ProductInfoRequest(int storeID, int productID) : base(6)
        {
            StoreId = storeID;
            ProductId = productID;
        }
    }
}
