using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class eSystem
    {
        private UserManager UManagment;
        private Security bodyguard;
        private DeliveryHandler DH;
        private PaymentHandler PH;
        private AppoitmentManager AM;
        private Dictionary<int, Store> stores;
        public eSystem(string admin = "", string password = "")
        {
            system_init(admin, password);
        }
        private void system_init(string admin, string password)
        {
            UManagment = UserManager.Instance;
            AM = AppoitmentManager.Instance;
            bodyguard = new Security();
            DH = new DeliveryHandler();
            PH = new PaymentHandler();
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
                string Exit = "Queiting the System\n";
                Console.WriteLine(Exit);
                Environment.Exit(1);
            }
            if (admin == "" || password == "")
            {
                Console.WriteLine("System is ready please register the system's Admin:\n");
                Console.WriteLine("User:\n");
                admin = Console.ReadLine();
                while (UManagment.isUserExist(admin))
                {
                    Console.WriteLine("User Already Exist Try Again:\n");
                    Console.WriteLine("User:\n");
                    admin = Console.ReadLine();
                }
                Console.WriteLine("Password:\n");
                password = Console.ReadLine();
            }
            string user_hash = bodyguard.CalcSha1(password);
            Tuple<bool, string> ans;
            ans = UManagment.RegisterMaster(admin, user_hash);
            if (user_hash == null || !ans.Item1)
            {
                Console.WriteLine("Failed to Assign Administrator\n");
                string Exit = "Queiting the System\n";
                Console.WriteLine(Exit);
                Environment.Exit(1);
            }
            Console.WriteLine("System Administrator Created:\n");
        }
        public bool SetDeliveryConnection(bool conn)
        {
            DH.setConnection(conn);
            return true;
        }
        public bool SetPaymentConnection(bool conn)
        {
            PH.setConnections(conn);
            return true;
        }
        public bool CheckDeliveryConnection()
        {
            return DH.checkconnection();
        }
        public bool CheckPaymentConnection()
        {
            return PH.checkconnection();
        }
        public Tuple<bool,string> pay()
        {
            return PH.pay();
        }
        public Tuple<bool, string>  ProvideDeliveryForUser(string name ,bool ispayed)
        {
            return DH.ProvideDeliveryForUser(name, ispayed);
        }
        public void clean(string name,string pass)
        {
            system_init(name, pass);
        }
    }
}