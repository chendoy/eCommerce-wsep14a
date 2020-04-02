using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    //from here comes all the functionality of the system
    class SystemTrackTest
    {
        BridgeInterface sys = Driver.GetBridge();

        public Boolean Login(String username, String password) { return sys.Login(username, password); }

        public Boolean Register(String username, String password) { return sys.Register(username, password); }

    }
}
