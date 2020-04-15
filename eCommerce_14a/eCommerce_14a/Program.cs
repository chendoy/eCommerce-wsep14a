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
            string str = "microsogt";

            string bestOffer = DidYouMean.suggest(str, "suggestions2.txt");
            Console.WriteLine("Did you mean: " + bestOffer + "?");
            Console.ReadLine();
        }

    }
}
