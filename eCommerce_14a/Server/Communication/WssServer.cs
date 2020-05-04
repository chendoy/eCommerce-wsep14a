using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using CertificateConfig = SuperSocket.SocketBase.Config.CertificateConfig;
using SuperWebSocket;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.Text;
using System.IO;
using Server.Communication.DataObject;
using Newtonsoft.Json;
using Server.UserComponent.Communication;

namespace eCommerce_14a.Communication
{
    public class WssServer
    {


        public CommunicationHandler handler;
        private static WebSocketServer wsServer;
        private ClientWebSocket client;
        private int port;
        private Dictionary<string, WebSocketSession> usersSessions;

        public WssServer()
        {
            client = new ClientWebSocket();
            usersSessions = new Dictionary<string, WebSocketSession>();
            handler = new CommunicationHandler();
            wsServer = new WebSocketServer();
        }

        private void InitServer()
        {
            Console.WriteLine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName);
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
            //client.Options.UseDefaultCredentials = true;
            wsServer.NewSessionConnected += StartSession;
            wsServer.SessionClosed += EndSession;
            wsServer.NewMessageReceived += ReceiveMessage;
            wsServer.NewDataReceived += ReceiveData;
            wsServer.Start();
            //client.Options.AddSubProtocol("Tls");
            //client.ConnectAsync(new Uri("wss://localhost:443"), new CancellationToken());
            Console.WriteLine("Server is running on port " + port + ". Press ENTER to exit....");
            Console.ReadKey();
            wsServer.Stop();
        }


        private void EndSession(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("SessionClosed");
        }

        private void StartSession(WebSocketSession session)
        {
            Console.WriteLine("NewSessionConnected");
        }

        private void ReceiveData(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("NewDataReceived");
            HandleMessage(session, value);
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            Console.WriteLine("Receive Msg:" + value);
        }


        private void StoreUsernameAndSession(WebSocketSession session, string value)
        {
            usersSessions.Add(value, session);
        }

        public void notify(string username, NotifyData msg)
        {
            WebSocketSession session;
            if (!usersSessions.TryGetValue(username, out session))
                return; // user isn't found.
            //session.Send(msg);
        }


        private void HandleMessage(WebSocketSession session, byte[] msg)
        {
            byte[] response;
            string json = handler.Decrypt(msg);
            int opcode = handler.GetOpCode(json);

            switch (opcode)
            {
                case 0:
                    //StoreUsernameAndSession(session, msg);
                    break;

                case 1:
                    response = handler.HandleLogin(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 2:
                    response = handler.HandleLogout(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 3:
                    response = handler.HandleRegister(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 4:
                    response = handler.HandleGetAllStores(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 5:
                    response = handler.HandleGetProductsOfStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 6:
                    response = handler.HandleGetProductDetails(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 7:
                    response = handler.HandlePurchase(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 8:
                    response = handler.HandleGetCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 9:
                    response = handler.HandleSearchProduct(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 10:
                    response = handler.HandleOpenStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 11:
                    response = handler.HandleBuyerHistory(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 12:
                    response = handler.HandleAppointManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 13:
                    response = handler.HandleAppointOwner(json);
                    session.Send(response, 0, response.Length);
                    break;

                case 14:
                    response = handler.HandleDemoteManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                default:
                    break;

            }
        }


        public static void Main(string[] argv)
        {
            //CommunicationHandler hand = new CommunicationHandler();
            //WssServer notifier = new WssServer();
            //notifier.InitServer();
            LoginRequest req = new LoginRequest("bla", "bla");
            string json = JsonConvert.SerializeObject(req);
            Console.WriteLine(json);
            LoginRequest res = JsonConvert.DeserializeObject<LoginRequest>(json);
            Console.WriteLine(res.Username);
            Console.WriteLine(res.Password);
            Console.WriteLine(res.Opcode);
            Console.ReadLine();
        }
    }
}
