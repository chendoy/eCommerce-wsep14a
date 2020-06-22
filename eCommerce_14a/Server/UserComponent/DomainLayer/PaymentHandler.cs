using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class PaymentHandler
    {
        bool connected;
        //PaymentSystem paymentSystem;
        PaymentHandler()
        {
            connected = true;
            //paymentSystem = new PaymentSystem();
        }

        private static readonly object padlock = new object();
        private static PaymentHandler instance = null;
        public static PaymentHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PaymentHandler();
                        }
                    }
                }
                return instance;
            }
        }

        public bool checkconnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PaymentSystem.IsAlive();
        }
        public virtual Tuple<bool,string> pay(string paymentDetails, double amount)
        {
            if (!PaymentSystem.IsAlive())
                return new Tuple<bool, string>(false, "Not Connected");

            int transaction_num = PaymentSystem.Pay("1", 1, 1, "1", "1", "1");
            if(transaction_num < 0)
                return new Tuple<bool, string>(false, "Payment Failed");

            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return new Tuple<bool,string>(true,"OK");
        }
        public virtual Tuple<bool, string> refund(string paymentDetails, double amount)
        {
            if (paymentDetails is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (paymentDetails == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return new Tuple<bool, string>(true, "OK");
        }

        public virtual Tuple<bool, string> refund(int transactionId)
        {
            if(transactionId < 0 )
                return new Tuple<bool, string>(false, "Invalid TransactionId");


            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            int cancel_res = PaymentSystem.CancelPayment(transactionId);
            if(cancel_res < 0)
                return new Tuple<bool, string>(false, "refund failed");

            return new Tuple<bool, string>(true, "OK");
        }
        public void setConnections(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.connected = con;
        }
    }
}