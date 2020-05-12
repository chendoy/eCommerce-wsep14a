﻿using System;
using CertificateConfig = SuperSocket.SocketBase.Config.CertificateConfig;
using SuperWebSocket;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.IO;
using Server.UserComponent.Communication;
using Newtonsoft.Json;
using Server.Communication.DataObject.Requests;
using System.Reflection.Emit;
using Server.Communication.DataObject;

namespace eCommerce_14a.Communication
{
    public class WssServer
    {
        public CommunicationHandler handler;
        private static WebSocketServer wsServer;
        private int port;
        

        public WssServer()
        {
            //Start all services 
            //Services will start the domain layer
            handler = new CommunicationHandler();
            wsServer = new WebSocketServer();
            Publisher.Instance.setServer(this);
            LoadData();
        }
        private void LoadData()
        {
            handler.loaddata();
        }
        private void InitServer()
        {
            port = 443;
            var config1 = new ServerConfig();
            config1.Port = port;
            config1.Security = "Tls";
            config1.Certificate = new CertificateConfig
            {
                FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Communication\cert.pfx",
                Password = "GuyTheKing!",
            };
            wsServer.Setup(config1);
            wsServer.NewSessionConnected += StartSession;
            wsServer.SessionClosed += EndSession;
            wsServer.NewMessageReceived += ReceiveMessage;
            wsServer.NewDataReceived += ReceiveData;
            wsServer.Start();
            Console.WriteLine("Server is running on port " + ". Press ENTER to exit....");
            Console.ReadKey();
            wsServer.Stop();
        }


        private void EndSession(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("SessionClosed");
        }

        private void StartSession(WebSocketSession session)
        {
            checkFunc(); // for tests
            Console.WriteLine("NewSessionConnected");
        }

        private void ReceiveData(WebSocketSession session, byte[] value)
        {
            HandleMessage(session, value);
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            //Console.WriteLine("Receive Msg:" + value);
        }

        public void notify(string username, NotifyData msg)
        {
            byte[] response;
            //get_session:
            WebSocketSession session = handler.GetSession(username);
            if (session == null)
                //goto get_session;
                return;
            response = handler.HandleNotification(msg);
            session.Send(response, 0, response.Length);
        }

        private void HandleMessage(WebSocketSession session, byte[] msg)
        {
            byte[] response;
            string json = handler.Decrypt(msg);
            Console.WriteLine("received: " + json);
            Opcode opcode = handler.GetOpCode(json);

            switch (opcode)
            {
                case Opcode.LOGIN:
                    response = handler.HandleLogin(json, session);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.LOGOUT:
                    response = handler.HandleLogout(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.REGISTER:
                    response = handler.HandleRegister(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.ALL_STORES:
                    response = handler.HandleGetAllStores(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PRODUCTS_OF_STORE:
                    response = handler.HandleGetProductsOfStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PROD_INFO:
                    response = handler.HandleGetProductDetails(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PURCHASE:
                    response = handler.HandlePurchase(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.USER_CART:
                    response = handler.HandleGetCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.SEARCH_PROD:
                    response = handler.HandleSearchProduct(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.OPEN_STORE:
                    response = handler.HandleOpenStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.BUYER_HISTORY:
                    response = handler.HandleBuyerHistory(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.APPOINT_MANAGER:
                    response = handler.HandleAppointManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.APPOINT_OWNER:
                    response = handler.HandleAppointOwner(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.DEMOTE:
                    response = handler.HandleDemoteManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.LOGIN_AS_GUEST:
                    response = handler.HandleLoginAsGuest(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.REMOVE_PRODUCT_FROM_CART:
                    response = handler.HandleRemovePoductFromCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.UPDATE_DISCOUNT_POLICY:
                    response = handler.HandleUpdateDiscountPolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.UPDATE_PURCHASE_POLICY:
                    response = handler.HandleUpdatePurchasePolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.STORES_OWNED_BY:
                    response = handler.HandleGetStoresOwnedBy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_ALL_ACTIVE_USERS:
                    response = handler.HandleGetAllActiveUsers(json);
                    session.Send(response, 0, response.Length);
                    break;


                default:
                    break;
            }
        }

        public void checkFunc() 
        {
            handler.HandleRegister(JsonConvert.SerializeObject(new RegisterRequest("admin", "admin")));
            for (int i = 0; i < 10; i++)
            {
                notify("admin", new NotifyData("Notify Number : " + i.ToString()));
            }

        }


        public static void Main(string[] argv)
        {
            CommunicationHandler hand = new CommunicationHandler();
            WssServer notifier = new WssServer();
            notifier.InitServer();
        }
    }
}
