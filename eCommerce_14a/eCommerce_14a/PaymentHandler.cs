﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class PaymentHandler
    {
        bool connected;
        int checker;
        public PaymentHandler()
        {
            Console.WriteLine("PaymentHandler Created\n");
            checker = 3;
            connected = true;
        }
        public int getchecker()
        {
            return this.checker;
        }

        public bool checkconnection()
        {
            return this.connected;
        }
        public Tuple<bool,string> pay()
        {
            return new Tuple<bool,string>(true,"OK");
        }
        public void setConnections(bool con)
        {
            this.connected = con;
        }
    }
}