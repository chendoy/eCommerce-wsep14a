using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;

namespace TestingSystem
{
    public class ProductGenerator
    {
        public static Product GetValidProduct() { return new Product(); }
        public static Product GetInvalidProduct() { return new Product(); }
    }
}
