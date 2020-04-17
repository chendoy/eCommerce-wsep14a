using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class DeliveryHandler
    {
        int checker;
        bool connected;
        public DeliveryHandler()
        {
            Console.WriteLine("DeliveryHandler Created\n");
            checker = 2;
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
        public bool Delivered(bool isAlive = true)
        {
            return isAlive;
        }
        public Tuple<bool, string> ProvideDeliveryForUser(string name, bool ispayed)
        {
            return new Tuple<bool, string>(true, "FineByNow");
        }
        public void setConnection(bool con)
        {
            this.connected = con;
        }
    }
}