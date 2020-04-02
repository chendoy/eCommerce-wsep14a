using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class RealBridge : BridgeInterface
    {
        public Boolean Login(String username, String password) { return true; }//sys.login(username, password)

        public Boolean Register(String username, String password) { return true; }//sys.register(username, password)
    }
}
