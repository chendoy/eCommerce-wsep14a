using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class PaymentHandler
    {
        int checker;
        public PaymentHandler()
        {
            Console.WriteLine("PaymentHandler Created\n");
            checker = 3;
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
