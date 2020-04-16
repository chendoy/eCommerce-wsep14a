using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class System_Service
    {
        eSystem Commercial_System;
        public System_Service(string name, string pass)
        {
            Commercial_System = new eSystem();
        }

        public Tuple<bool, string> initSystem(string userName, string pass)
        {
            return Commercial_System.system_init(userName, pass);
        }
        public bool SetDeliveryConnection(bool con)
        {
            return Commercial_System.SetDeliveryConnection(con);
        }
        public bool SetPaymentConnection(bool con)
        {
            return Commercial_System.SetPaymentConnection(con);
        }
        public bool CheckDeliveryConnection()
        {
            return Commercial_System.CheckDeliveryConnection();
        }
        public bool CheckPaymentConnection()
        {
            return Commercial_System.CheckPaymentConnection();
        }
        public Tuple<bool,string> pay()
        {
            return Commercial_System.pay();
        }
        public Tuple<bool, string> ProvideDeliveryForUser(string username,bool paymentFlag)
        {
            return Commercial_System.ProvideDeliveryForUser(username, paymentFlag);
        }
        public void clean(string n, string p)
        {
            Commercial_System.clean(n,p);
        }
    }
}
