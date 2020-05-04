using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{
    public class SearchProductRequest : Message
    {
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string KeyWord { get; set; }

        public SearchProductRequest(string prodName = null, string prodCategory = null, string keyword = null) : base(9)
        {
            ProductName = prodName;
            ProductCategory = prodCategory;
            KeyWord = keyword;
        }
    }
}
