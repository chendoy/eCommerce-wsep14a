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
        public eSystem(string admin = "",string password = "")
        {
            UManagment = new UserManager();
            bodyguard = new Security();
            DH = new DeliveryHandler();
            PH = new PaymentHandler();
            AM = new AppoitmentManager();
            stores = new Dictionary<int, Store>();
            if (!DH.checkconnection() || !PH.checkconnection())
            {
                for(int i = 0;i<4;i++)
                {
                    if(DH.checkconnection() && !PH.checkconnection())
                    {
                        break;
                    }
                }
                string Exit = "Queiting the System\n";
                Console.WriteLine(Exit);
                Environment.Exit(1);
            }
            if(admin == "" || password == "")
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
            if (user_hash == null || !UManagment.RegisterMaster(admin, user_hash))
            {
                Console.WriteLine("Failed to Assign Administrator\n");
                string Exit = "Queiting the System\n";
                Console.WriteLine(Exit);
                Environment.Exit(1);
            }
            Console.WriteLine("System Administrator Created:\n");
        }
        public bool Registration()
        {
            Console.WriteLine("Please insert Username:\n");
            string username = Console.ReadLine();
            while (UManagment.isUserExist(username))
            {
                Console.WriteLine("User Already Exist Try another option:\n");
                Console.WriteLine("User:\n");
                username = Console.ReadLine();
            }
            Console.WriteLine("Please Enter Password:\n");
            string password = Console.ReadLine();
            string user_hash = bodyguard.CalcSha1(password);
            if (!UManagment.Register(username, user_hash))
                Console.WriteLine("Registration Failed\n");
            Console.WriteLine("Registration was Successful\n");
            return true;
        }
        public bool Login(bool isAguest = false)
        {
            if(isAguest)
            {
                UManagment.Login("","",true);
            }
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("User:\n");
                string username = Console.ReadLine();
                Console.WriteLine("pass:\n");
                string pass = Console.ReadLine();
                string user_hash = bodyguard.CalcSha1(pass);
                if (!UManagment.Login(username, user_hash))
                    Console.WriteLine("Wrong Cred" + (2 - i) + "Tries Left:\n");
                else
                {
                    Console.WriteLine("Worked Cred:\n");
                    return true;
                }
            }
            Console.WriteLine("Failed to Login Wrong Cred:\n");
            return false;
        }
        public bool Logout(string name)
        {
            return UManagment.Logout(name);
        }
        //Open store
        public bool OpenStore()
        {
            return UManagment.OpenStore();
        }
        //Manage Store(User user)
    }
}
