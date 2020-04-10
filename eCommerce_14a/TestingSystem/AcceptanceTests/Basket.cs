using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Basket
    {
        List<Product> products;

        public Basket() 
        {
            products = new List<Product>();
        }

        public bool AddProduct(Product product) 
        {
            products.Add(product);
            return true;
        }

        public bool Contains(Product product) 
        {
            return products.Contains(product);
        }
    }
}
