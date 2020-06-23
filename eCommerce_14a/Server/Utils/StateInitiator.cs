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
using Server.DAL;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.UserComponent.DomainLayer;

namespace Server.Utils
{
    class StateInitiator
    {
        CommunicationHandler handler;
        List<string> lines;
        

        public StateInitiator() 
        {
            handler = new CommunicationHandler();
            lines = new List<string>();
        }

        public void CreateScenario() 
        {
            MakeRegisterLine("u11", "u11");
            MakeRegisterLine("u22", "u22");
            MakeRegisterLine("u33", "u33");
            MakeRegisterLine("u44", "u44");
            MakeRegisterLine("u55", "u55");
            MakeAdminLine("u11");
            MakeLoginLine("u22", "u22");
            MakeOpenStoreLine("u22");
            int storeID = DbManager.Instance.GetNextStoreId(); // (s1)
            MakeAddProductToStoreLine(storeID, "u22", "for age 0 - 2", 30, "diapers", "hygiene", 20);
            MakeAppointManagerLine("u22", "u33", storeID);
            MakeChangePermissionsLine("u22", "u33", storeID, new int[5] {1,1,1,0,0});
            MakeLoginLine("u33", "u33");
            MakeAppointManagerLine("u33", "u44", storeID);
            MakeLoginLine("u44", "u44");
            MakeAppointManagerLine("u44", "u55", storeID);
            MakeLogoutLine("u22");
            MakeLogoutLine("u33");
            MakeLogoutLine("u44");
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

        public void MakeAppointManagerLine(string appointer, string appointed, int storeID) 
        {
            AppointManagerRequest req = new AppointManagerRequest(appointer, appointed, storeID);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }
        public void MakeChangePermissionsLine(string appointer, string appointed, int storeID, int[] permissions)
        {
            Permission permission = new Permission(permissions);
            ChangePermissionsRequest req = new ChangePermissionsRequest(appointer, appointed, storeID, permission);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
        }
        public void MakeAdminLine(string username)
        {
            MakeAdminRequest req = new MakeAdminRequest(username);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
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

        public void MakeOpenStoreLine(string username)
        {
            OpenStoreRequest req = new OpenStoreRequest(username);
            string json = JsonConvert.SerializeObject(req);
            lines.Add(json);
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
            try
            {
                string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Utils\State.txt";
                string[] operations = File.ReadAllLines(path);

                foreach (string operation in operations)
                {
                    HandleState(operation);
                }
            }
            catch
            {
                Console.WriteLine("load system from init file failed - init the system from default init file..\n");
                //default path
                string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Utils\DefaultInitFile.txt";
                string[] operations = File.ReadAllLines(path);

                foreach (string operation in operations)
                {
                    HandleState(operation);
                }
            }
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
                    handler.HandleLogoutToInit(json);
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
                    handler.HandleOpenStore(json);
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

                case Opcode.MAKE_ADMIN:
                    handler.HandleMakeAdmin(json);
                    break;

                case Opcode.CHANGE_PERMISSIONS:
                    handler.HandleChangePermissions(json);
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
