using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Owner { get; set; }

        public List<Product> Products { get; set; }
    }
}
