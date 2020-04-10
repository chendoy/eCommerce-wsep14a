using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class DeliveryHandler
    {
        int checker;
        public DeliveryHandler()
        {
            Console.WriteLine("DeliveryHandler Created\n");
            checker = 2;
        }
        public int getchecker()
        {
            return this.checker;
        }

        public bool checkconnection(bool isAlive = true)
        {
            return isAlive;
        }
    }
}
