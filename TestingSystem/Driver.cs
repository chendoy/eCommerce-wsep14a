﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class Driver
    {
        public static BridgeInterface GetBridge() { return new ProxyBridge(); }//return new RealBridge();
    }
}
