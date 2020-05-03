using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.Utils;
using Newtonsoft.Json;

namespace eCommerce_14a.Communication
{
    public class CommunicationHandler
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        System_Service sysService; //sundy
        StoreService StoreService; //liav
        PurchaseService purchService; //naor
        InfoExtractor extract;
        NetworkSecurity security;
        public CommunicationHandler()
        {
            appointService = new Appoitment_Service();
            userService = new UserService();
            sysService = new System_Service("Admin","Admin");
            StoreService = new StoreService();
            purchService = new PurchaseService();
            extract = new InfoExtractor();
            security = new NetworkSecurity();
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

        public Dictionary<string, object> GetDictFromMsg(byte[] msg)
        {
            string json = security.Decrypt(msg);
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dict;
        }
        public int GetOpCode(byte[] msg) 
        {
            object opcodeObj;
            string dec = security.Decrypt(msg); // decrypt the msg and convert it into string
            Dictionary<string, object> msgDict = Deseralize(dec); // desarilize the decrypted string and convert it into dict
            if (!msgDict.TryGetValue("OpCode", out opcodeObj))
                return -1;
            return (int)opcodeObj;
        }

        public byte[] HandleLogin(Dictionary<string, object> msgDict)
        {
            string username = extract.GetUsername(msgDict);
            string password = extract.GetPassword(msgDict);
            Tuple<bool, string> ans = userService.Login(username, password);
            string jsonAns = Seralize(new ResponseData(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleLogout(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleRegister(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleGetAllStores(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleGetProductsOfStore(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleGetProductDetails(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandlePurchase(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleGetCart(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleSearchProduct(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }

        internal byte[] HandleOpenStore(Dictionary<string, object> msgDict)
        {
            throw new NotImplementedException();
        }
    }
}
