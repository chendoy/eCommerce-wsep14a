using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class RealBridge : BridgeInterface
    {
        public RealBridge(){}

        public new Boolean Login(String username, String password)
        {
            return true;//sys.login(username, password)
        }

        public new Boolean Register(String username, String password)
        {
            return true;//sys.register(username, password)
        }
        public new bool ViewShopDetails()
        {
            return true;
        }
        public new bool ViewProductsByCategory(String InvalidCategory)
        {
            return true;
        }
    }
}
