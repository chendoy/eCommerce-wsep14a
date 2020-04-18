using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;


namespace eCommerce_14a.UserComponent.ServiceLayer
{
    public class UserService
    {
        UserManager UM;
        public UserService()
        {
            UM = UserManager.Instance;
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
        public Tuple<bool,string> Registration(string username, string password)
        {
            //Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return  UM.Register(username, password);

        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
        public Tuple<bool,string> Login(string username, string password)
        {
            //Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Login(username, password);

        }
        public Tuple<bool,string> LoginAsGuest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Login("", "", true);

        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer-logout-31</req>
        public Tuple<bool,string> Logout(string user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Logout(user);
        
        }
        //For Admin Usage
        public void cleanup()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            UM.cleanup();
        }
    }
}