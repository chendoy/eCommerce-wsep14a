using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Utils
{
    class InfoExtractor
    {
        public InfoExtractor() { }

        public string GetUsername(Dictionary<string, object> msgDict)
        {
            object usernameObj;
            msgDict.TryGetValue("Username", out usernameObj);
            string username = (string)usernameObj;
            return username;
        }
        public string GetPassword(Dictionary<string, object> msgDict)
        {
            object passwordObj;
            msgDict.TryGetValue("Password", out passwordObj);
            string password = (string)passwordObj;
            return password;
        }
    }
}
