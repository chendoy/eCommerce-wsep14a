using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class DeliveryHandler
    {
        bool connected;
        public DeliveryHandler()
        {
            Console.WriteLine("DeliveryHandler Created\n");
            connected = true;
        }

        public bool checkconnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.connected;
        }
        public bool Delivered(bool isAlive = true)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return isAlive;
        }
        public virtual Tuple<bool, string> ProvideDeliveryForUser(string name, bool ispayed)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return new Tuple<bool, string>(true, "FineByNow");
        }
        public void setConnection(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.connected = con;
        }
    }
}