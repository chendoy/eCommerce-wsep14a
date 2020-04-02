using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class ProxyBridge : BridgeInterface
    {
        public Boolean Login(String username, String password) { return true; }

        public Boolean Register(String username, String password) { return true; }
    }
}
