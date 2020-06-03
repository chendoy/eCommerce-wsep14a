using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using eCommerce_14a.Communication;
using Server.Communication.DataObject;
using Server.Communication.DataObject.Requests;
using Newtonsoft.Json;
using eCommerce_14a.StoreComponent.ServiceLayer;

namespace Server.Utils
{
    class StateInitiator
    {
        CommunicationHandler handler;
        List<string> lines;
        StoreService storeService;
        

        public StateInitiator() 
        {
            handler = new CommunicationHandler();
            lines = new List<string>();
        }

        public void CreateScenario() 
        {
            MakeRegisterLine("Guy", "Guy");
            MakeLoginLine("Guy", "Guy");
            MakeRegisterLine("Liav", "Liav");
            MakeLoginLine("Liav", "Liav");
            int storeID = MakeOpenStoreLine("Guy");
            MakeAddProductToStoreLine(storeID, "Guy", "bananot", 1.7, "banana", "fruits", 3);
            MakeAddProductToCartLine(storeID, "Liav", 1, 2);
            MakePerformPurchaseLine("Liav", "BeerSheva Gimel", "111223334");
            WriteScenarioToFile();
        }
        public void WriteScenarioToFile()
        {
            using (StreamWriter file =
            new StreamWriter(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Utils\State.txt"))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
            lines.Clear();
        }

        public void MakeRegisterLine(string username, string password) 
        {
            RegisterRequest req = new RegisterRequest(username, password);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public void MakeLoginLine(string username, string password)
        {
            LoginRequest req = new LoginRequest(username, password);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public void MakeLogoutLine(string username)
        {
            LogoutRequest req = new LogoutRequest(username);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public int MakeOpenStoreLine(string username)
        {
            OpenStoreRequest req = new OpenStoreRequest(username);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
            return handler.SpecialHandleOpenStore(username);
        }

        public void MakeAddProductToStoreLine(int storeID, string username, string pDetails, double pPrice, string pName, string pCategory, int amount)
        {
            AddProductToStoreRequest req = new AddProductToStoreRequest(storeID, username, pDetails, pPrice, pName, pCategory, amount);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public void MakeAddProductToCartLine(int storeID, string username, int productID, int amount)
        {
            AddProductToCartRequest req = new AddProductToCartRequest(username, storeID, productID, amount);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public void MakePerformPurchaseLine(string username, string address, string paymentDetails)
        {
            PurchaseRequest req = new PurchaseRequest(username, address, paymentDetails);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }

        public void InitSystemFromFile() 
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Utils\State.txt";
            CreateScenario();
            string[] operations = File.ReadAllLines(path);
            foreach (string operation in operations)
            {
                HandleState(operation);
            }
            File.WriteAllText(path, String.Empty);
        }

        private void HandleState(string json)
        {
            
            Opcode opcode = handler.GetOpCode(json);

            switch (opcode)
            {
                case Opcode.LOGIN:
                    handler.HandleLogin(json);
                    break;

                case Opcode.LOGOUT:
                    handler.HandleLogout(json);
                    break;

                case Opcode.REGISTER:
                    handler.HandleRegister(json);
                    break;

                case Opcode.ALL_STORES:
                    handler.HandleGetAllStores(json);
                    break;

                case Opcode.PRODUCTS_OF_STORE:
                    handler.HandleGetProductsOfStore(json);
                    break;

                case Opcode.PROD_INFO:
                    handler.HandleGetProductDetails(json);
                    break;

                case Opcode.PURCHASE:
                    handler.HandlePurchase(json);
                    break;

                case Opcode.USER_CART:
                    handler.HandleGetCart(json);
                    break;

                case Opcode.SEARCH_PROD:
                    handler.HandleSearchProduct(json);
                    break;

                case Opcode.OPEN_STORE:
                    //handler.HandleOpenStore(json);
                    break;

                case Opcode.BUYER_HISTORY:
                    handler.HandleBuyerHistory(json);
                    break;

                case Opcode.APPOINT_MANAGER:
                    handler.HandleAppointManager(json);
                    break;

                case Opcode.APPOINT_OWNER:
                    handler.HandleAppointOwner(json);
                    break;

                case Opcode.DEMOTE_MANAGER:
                    handler.HandleDemoteManager(json);
                    break;

                case Opcode.DEMOTE_OWNER:
                    handler.HandleDemoteOwner(json);
                    break;

                case Opcode.LOGIN_AS_GUEST:
                    handler.HandleLoginAsGuest(json);
                    break;

                case Opcode.REMOVE_PRODUCT_FROM_CART:
                    handler.HandleRemovePoductFromCart(json);
                    break;

                case Opcode.UPDATE_DISCOUNT_POLICY:
                    handler.HandleUpdateDiscountPolicy(json);
                    break;

                case Opcode.UPDATE_PURCHASE_POLICY:
                    handler.HandleUpdatePurchasePolicy(json);
                    break;

                case Opcode.STORES_OWNED_BY:
                    handler.HandleGetStoresOwnedBy(json);
                    break;

                case Opcode.GET_ALL_REGISTERED_USERS:
                    handler.HandleGetAllRegisteredUsers(json);
                    break;

                case Opcode.ADD_PRODUCT_TO_CART:
                    handler.HandleAddProductToCart(json);
                    break;

                case Opcode.CHANGE_PRODUCT_AMOUNT_CART:
                    handler.HandleChangeProductAmountInCart(json);
                    break;
                
                case Opcode.GET_STAFF_OF_STORE:
                    handler.HandleGetStaffOfStore(json);
                    break;

                case Opcode.GET_AVAILABLE_DISCOUNTS:
                    handler.HandleGetAvailableDiscounts(json);
                    break;

                case Opcode.ADD_PRODUCT_TO_STORE:
                    handler.HandleAddProductToStore(json);
                    break;

                case Opcode.REMOVE_PRODUCT_FROM_STORE:
                    handler.HandleRemoveProductFromStore(json);
                    break;

                case Opcode.UPDATE_PRODUCT_OF_STORE:
                    handler.HandleUpdateProductOfStore(json);
                    break;

                case Opcode.STORE_HISTORY:
                    handler.HandleGetStoreHistory(json);
                    break;

                case Opcode.ALL_STORE_HISTORY:
                    handler.HandleGetAllStoresHistory(json);
                    break;


                case Opcode.ALL_BUYERS_HISTORY:
                    handler.HandleGetAllUsersHistory(json);
                    break;

                case Opcode.STORE_BY_ID:
                    handler.HandleGetStoreById(json);
                    break;


                case Opcode.INCREASE_PRODUCT_AMOUNT:
                    handler.HandleIncreaseProductAmount(json);
                    break;

                case Opcode.DECREASE_PRODUCT_AMOUNT:
                    handler.HandleDecreaseProductAmount(json);
                    break;

                default:
                    break;
            }
        }

        //public static void Main(string[] argv)
        //{
        //    StateInitiator init = new StateInitiator();
        //    init.InitSystemFromFile();
        //}
    }
}
