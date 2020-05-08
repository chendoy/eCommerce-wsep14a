using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<User> Owners { get; set; }
        public List<User> Mangers { get; set; }
        public List<Product> Products { get; set; }
        public string StoreThumbnail { get; set; }


        public Store() { }

    }
}
