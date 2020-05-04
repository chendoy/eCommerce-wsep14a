using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.Utils;
using Newtonsoft.Json;
using Server.Communication.DataObject;
using Server.UserComponent.Communication;
using SuperWebSocket;

namespace eCommerce_14a.Communication
{
    public class CommunicationHandler
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        System_Service sysService; //sundy
        StoreService storeService; //liav
        PurchaseService purchService; //naor
        NetworkSecurity security;
        private Dictionary<string, WebSocketSession> usersSessions;
        public CommunicationHandler()
        {
            appointService = new Appoitment_Service();
            userService = new UserService();
            sysService = new System_Service("Admin","Admin");
            storeService = new StoreService();
            purchService = new PurchaseService();
            security = new NetworkSecurity();
            usersSessions = new Dictionary<string, WebSocketSession>();
        }

        public string Seralize(object obj)
        {
            string jsonString;
            jsonString = JsonConvert.SerializeObject(obj);
            return jsonString;
        }

        public Dictionary<string, object> Deseralize(string json) 
        {
            Dictionary<string,object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dict;
        }

        public int GetOpCode(string msg) 
        {
            object opcodeObj;
            Dictionary<string, object> msgDict = Deseralize(msg); // desarilize the decrypted string and convert it into dict
            if (!msgDict.TryGetValue("Opcode", out opcodeObj))
                return -1;
            return Convert.ToInt32(opcodeObj);
        }

        public string Decrypt(byte[] cipher) 
        {
            return security.Decrypt(cipher);
        }

        public WebSocketSession GetSession(string username)
        {
            WebSocketSession session;
            if (!usersSessions.TryGetValue(username, out session))
                return null;
            return session;
        }

        public byte[] HandleLogin(string json, WebSocketSession session)
        {
            LoginRequest res = JsonConvert.DeserializeObject<LoginRequest>(json);
            Tuple<bool, string> ans = userService.Login(res.Username, res.Password);
            if (ans.Item1)
                usersSessions.Add(res.Username, session);
            string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleLogout(string json)
        {
            LogoutRequest res = JsonConvert.DeserializeObject<LogoutRequest>(json);
            Tuple<bool, string> ans = userService.Logout(res.Username);
            string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleRegister(string json)
        {
            //RegisterRequest res = JsonConvert.DeserializeObject<RegisterRequest>(json);
            //Tuple<bool, string> ans = userService.Registration(res.Username, res.Password);
            //string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            //return security.Encrypt(jsonAns);
            return new byte[20];
        }

        public byte[] HandleGetAllStores(string json)
        {
            GetAllStoresRequest res = JsonConvert.DeserializeObject<GetAllStoresRequest>(json);
            //Tuple<bool, string> ans = storeService.GetAllStores();
            //string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            //return security.Encrypt(jsonAns);
            return new byte[20];
        }

        internal byte[] HandleNotification(NotifyData msg)
        {
            string json = Seralize(msg);
            return security.Encrypt(json);
        }

        public byte[] HandleGetProductsOfStore(string json)
        {
            StoresProductsRequest res = JsonConvert.DeserializeObject<StoresProductsRequest>(json);
            //Tuple<bool, string> ans = storeService.SearchProducts(new Dictionary<string, object>(CommonStr.SearcherKeys.StoreId,res.StoreId));
            //string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            //return security.Encrypt(jsonAns);
            return new byte[20];
        }

        public byte[] HandleGetProductDetails(string json)
        {
            ProductInfoRequest res = JsonConvert.DeserializeObject<ProductInfoRequest>(json);
            //Tuple<bool, string> ans = storeService.SearchProducts(new Dictionary<string, object>(CommonStr.SearcherKeys.));
            //string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            //return security.Encrypt(jsonAns);
            return new byte[20];
        }

        public byte[] HandlePurchase(string json)
        {
            PurchaseRequest res = JsonConvert.DeserializeObject<PurchaseRequest>(json);
            Tuple<bool, string> ans = purchService.PerformPurchase(res.Username, res.PaymentDetails, res.Address);
            string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetCart(string json)
        {
            CartRequest res = JsonConvert.DeserializeObject<CartRequest>(json);
            Tuple<Cart, string> ans = purchService.GetCartDetails(res.Username);
            string jsonAns = Seralize(ans);
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleSearchProduct(string json)
        {
            throw new NotImplementedException();
        }

        public byte[] HandleOpenStore(string json)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleBuyerHistory(string json)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleAppointManager(string json)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleAppointOwner(string json)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleDemoteManager(string json)
        {
            throw new NotImplementedException();
        }
    }
}
