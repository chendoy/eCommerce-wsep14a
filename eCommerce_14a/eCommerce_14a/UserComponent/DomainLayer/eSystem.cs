using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class eSystem
    {
        private UserManager UManagment;
        private Security bodyguard;
        private DeliveryHandler DH;
        private PaymentHandler PH;
        private AppoitmentManager AM;
        private Dictionary<int, Store> stores;
        
         
        public Tuple<bool, string> system_init(string admin, string password,bool paymmentconnection = true)
        {
            if (admin is null || password is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (admin == "" || password == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            UManagment = UserManager.Instance;
            AM = AppoitmentManager.Instance;
            bodyguard = new Security();
            DH = new DeliveryHandler();
            PH = new PaymentHandler();
            PH.setConnections(paymmentconnection);
            stores = new Dictionary<int, Store>();
            if (!DH.checkconnection() || !PH.checkconnection())
            {
                for (int i = 0; i < 4; i++)
                {
                    if (DH.checkconnection() && !PH.checkconnection())
                    {
                        break;
                    }
                }
                return new Tuple<bool, string>(false, "cann't connect to 3rd party system");
            }
            
            string user_hash = bodyguard.CalcSha1(password);
            Tuple<bool, string> ans;
            ans = UManagment.RegisterMaster(admin, user_hash);
            if (user_hash == null || !ans.Item1)
            {
                return new Tuple<bool, string>(true, "System Admin didn't register");

            }
            return new Tuple<bool, string>(true, "");
        }
        public bool SetDeliveryConnection(bool conn)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            DH.setConnection(conn);
            return true;
        }
        public bool SetPaymentConnection(bool conn)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            PH.setConnections(conn);
            return true;
        }
        public bool CheckDeliveryConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return DH.checkconnection();
        }
        public bool CheckPaymentConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PH.checkconnection();
        }
        public Tuple<bool,string> pay(string PaymentDetails, double amount)
        {
            if (PaymentDetails is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (PaymentDetails == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PH.pay(PaymentDetails, amount);
        }
        public Tuple<bool, string>  ProvideDeliveryForUser(string name ,bool ispayed)
        {
            if(name is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (name == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return DH.ProvideDeliveryForUser(name, ispayed);
        }
        public void clean(string name,string pass)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            system_init(name, pass);
        }
    }
}