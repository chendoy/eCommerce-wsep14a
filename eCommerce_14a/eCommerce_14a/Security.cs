using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Security
    {
        public Security()
        {
            Console.WriteLine("Security Created\n");
        }
        public string CalcSha1(string pass)
        {
            if (pass == null || pass == "")
                return null;
            var data = Encoding.ASCII.GetBytes(pass);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
    }
}
