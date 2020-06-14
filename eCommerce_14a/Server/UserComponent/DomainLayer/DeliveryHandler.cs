using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Utils;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class DeliveryHandler
    {
        bool connected;
        //DeliverySystem deliverySystem;
        DeliveryHandler()
        {
            connected = true;
            //deliverySystem = new DeliverySystem();
        }

        private static readonly object padlock = new object();
        private static DeliveryHandler instance = null;
        public static DeliveryHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DeliveryHandler();
                        }
                    }
                }
                return instance;
            }
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
            if(!connected)
                return new Tuple<bool, string>(false, "NotConnected");
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