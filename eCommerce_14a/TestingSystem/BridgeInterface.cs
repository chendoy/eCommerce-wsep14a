using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    //this is the empty bridge for abstraction
    class BridgeInterface
    {
        public Boolean Login(String username, String password) { return true; }

        public Boolean Register(String username, String password) { return true; }
    }
}
