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
using Server.Communication.DataObject.Requests;
using Server.Communication.DataObject.Responses;
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
            if (!msgDict.TryGetValue("_Opcode", out opcodeObj))
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
            string jsonAns = Seralize(new LoginResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleLogout(string json)
        {
            LogoutRequest res = JsonConvert.DeserializeObject<LogoutRequest>(json);
            Tuple<bool, string> ans = userService.Logout(res.Username);
            string jsonAns = Seralize(new LogoutResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleRegister(string json)
        {
            RegisterRequest res = JsonConvert.DeserializeObject<RegisterRequest>(json);
            Tuple<bool, string> ans = userService.Registration(res.Username, res.Password);
            string jsonAns = Seralize(new RegisterResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAllStores(string json)
        {
            GetAllStoresRequest res = JsonConvert.DeserializeObject<GetAllStoresRequest>(json);
            List<Store> ans = storeService.GetAllStores();
            string jsonAns = Seralize(new GetStoresResponse(ans));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleNotification(NotifyData msg)
        {
            string json = Seralize(msg);
            return security.Encrypt(json);
        }

        public byte[] HandleGetProductsOfStore(string json)
        {
            StoresProductsRequest res = JsonConvert.DeserializeObject<StoresProductsRequest>(json);
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.StoreId, res.StoreId);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(filters);
            List<Product> prodList = ans[res.StoreId];
            string jsonAns = Seralize(new SearchProductResponse(prodList));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetProductDetails(string json)
        {
            Product prod = null;
            ProductInfoRequest res = JsonConvert.DeserializeObject<ProductInfoRequest>(json);
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.StoreId, res.StoreId);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(filters);
            List<Product> prodList = ans[res.StoreId];
            foreach (Product product in prodList) 
            {
                if (product.ProductID == res.ProductId)
                {
                    prod = product;
                    break;
                }
            }
            string jsonAns = Seralize(new ProductInfoResponse(prod));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandlePurchase(string json)
        {
            PurchaseRequest res = JsonConvert.DeserializeObject<PurchaseRequest>(json);
            Tuple<bool, string> ans = purchService.PerformPurchase(res.Username, res.PaymentDetails, res.Address);
            string jsonAns = Seralize(new PurchaseResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetCart(string json)
        {
            CartRequest res = JsonConvert.DeserializeObject<CartRequest>(json);
            Tuple<Cart, string> ans = purchService.GetCartDetails(res.Username);
            string jsonAns = Seralize(new GetUsersCartResponse(ans.Item1));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleSearchProduct(string json) //deal with doytsh
        {
            SearchProductRequest res = JsonConvert.DeserializeObject<SearchProductRequest>(json);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(res.Filters);
            List<Product> prodList = ans.Values.ToList().SelectMany(x => x).ToList();
            string jsonAns = Seralize(new SearchProductResponse(prodList));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleOpenStore(string json)
        {
            OpenStoreRequest res = JsonConvert.DeserializeObject<OpenStoreRequest>(json);
            Tuple<int, string> ans = storeService.createStore(res.Username, 0, 0);
            bool success = ans.Item1 == -1 ? false : true;
            string jsonAns = Seralize(new OpenStoreResponse(success,ans.Item2, ans.Item1));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleBuyerHistory(string json)
        {
            BuyerHistoryRequest res = JsonConvert.DeserializeObject<BuyerHistoryRequest>(json);
            Tuple<List<Purchase>, string> ans = purchService.GetBuyerHistory(res.Username);
            string jsonAns = Seralize(new HistoryResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleAppointManager(string json)
        {
            AppointManagerRequest res = JsonConvert.DeserializeObject<AppointManagerRequest>(json);
            Tuple<bool, string> ans = appointService.AppointStoreManage(res.Appointer,res.Appointed,res.StoreId);
            string jsonAns = Seralize(new AppointManagerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleAppointOwner(string json)
        {
            AppointOwnerRequest res = JsonConvert.DeserializeObject<AppointOwnerRequest>(json);
            Tuple<bool, string> ans = appointService.AppointStoreOwner(res.Appointer, res.Appointed, res.StoreId);
            string jsonAns = Seralize(new AppointOwnerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleDemoteManager(string json)
        {
            DemoteRequest res = JsonConvert.DeserializeObject<DemoteRequest>(json);
            Tuple<bool, string> ans = appointService.RemoveStoreManager(res.Appointer, res.Appointed, res.StoreId);
            string jsonAns = Seralize(new DemoteManagerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }
    }
}
