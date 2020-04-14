using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    class Program
    {
        public static void Main()
        {
            Program p = new Program();
            p.testLogger();
        }

        private void testLogger()
        {
            Logger.logError("Error message.", this);
            Logger.logEvent("Event message.", this);
            Console.ReadLine();
        }

    }
}
