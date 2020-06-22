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
            if(!DeliverySystem.IsAlive())
                return new Tuple<bool, string>(false, "Not Connected Delivery System");
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            int delivery_res = DeliverySystem.Supply("dummy", "dummy", "dummy", "dummy", "dummy");
            if(delivery_res < 0)
                return new Tuple<bool, string>(false, "Delivery Failed");

            return new Tuple<bool, string>(true, "FineByNow");
        }

        public virtual Tuple<bool, string> ProvideDeliveryForUser(string deliveryDetails)
        {
            if (!DeliverySystem.IsAlive())
                return new Tuple<bool, string>(false, "Not Connected Delivery System");
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            string[] parsedData = deliveryDetails.Split('&');
            int delivery_res = DeliverySystem.Supply(parsedData[0], parsedData[1], parsedData[3], parsedData[2], parsedData[4]);
            if (delivery_res < 0)
                return new Tuple<bool, string>(false, "Delivery Failed");

            return new Tuple<bool, string>(true, "FineByNow");
        }
        public void setConnection(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.connected = con;
        }
    }
}