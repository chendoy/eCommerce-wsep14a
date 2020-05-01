using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.Utils;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketEngine.Configuration;
using CertificateConfig = SuperSocket.SocketBase.Config.CertificateConfig;

namespace eCommerce_14a.Communication
{
    class WssServer
    {
        public CommunicationHandler handler;
        private static WebSocketServer wsServer;
        private ClientWebSocket client;
        private int port;
        private Dictionary<string, WebSocketSession> usersSessions;
        private NetworkSecurity sec;

        public WssServer()
        {
            sec = new NetworkSecurity();
            client = new ClientWebSocket();
            usersSessions = new Dictionary<string, WebSocketSession>();
            handler = new CommunicationHandler();
            wsServer = new WebSocketServer();
        }

        private void InitServer() {
            port = 443;
            //var config1 = new ServerConfig();
            //config1.Port = port;
            //config1.Security = "Tls";
            //config1.Certificate = new CertificateConfig
            //{
            //    FilePath = @"D:\certificate\cert.pfx",
            //    Password = "GuyTheKing!",
            //    //StoreName = StoreName.My.ToString()
            //};

            //wsServer.Setup(config1);
            wsServer.Setup(port);
            wsServer.NewSessionConnected += StartSession;
            wsServer.SessionClosed += EndSession;
            wsServer.NewMessageReceived += ReceiveMessage;
            wsServer.NewDataReceived += ReceiveData;
            wsServer.Start();
            //X509Certificate certificate = new X509Certificate(@"D:\certificate\OutputCert.cer", "GuyTheKing!");
            //client.Options.ClientCertificates.Add(certificate);
            //client.Options.AddSubProtocol("Tls");
            //client.ConnectAsync(new Uri("ws://192.168.1.42:443"), new CancellationToken());
            Console.WriteLine(client.State.ToString());
            
            Console.WriteLine("Server is running on port " + port + ". Press ENTER to exit....");
            Console.ReadKey();
            wsServer.Stop();
        }

        private void ReceiveData(WebSocketSession session, byte[] value)
        {
            //
            Console.WriteLine("NewDataReceived");
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            //string dec = sec.Decrypt(Encoding.UTF8.GetBytes(value));
            Console.WriteLine("Receive Msg:" + value);
            //HandleMessage(session, value);
        }

        private void EndSession(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("SessionClosed");
            //Console.WriteLine(client.State.ToString());
        }

        private void StartSession(WebSocketSession session)
        {
            Console.WriteLine("NewSessionConnected");
        }

        private void StoreUsernameAndSession(WebSocketSession session, string value)
        {
            usersSessions.Add(value, session);
        }

        private void notify(string username, string msg) 
        {
            WebSocketSession session;
            if (!usersSessions.TryGetValue(username, out session))
                return; // user isn't found.
            session.Send(msg);
        }


        private void HandleMessage(WebSocketSession session, string msg)
        {
            byte[] response;
            object opObj;
            Dictionary<string, object> msgDict = handler.Deseralize(msg);
            if (!msgDict.TryGetValue("Opcode", out opObj))
                return;
            int opcode = (int)opObj;

            switch (opcode)
            {
                case 0:
                    StoreUsernameAndSession(session, msg);
                    break;

                case 1:
                    response = handler.HandleLogin(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 2:
                    response = handler.HandleLogout(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 3:
                    response = handler.HandleRegister(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 4:
                    response = handler.HandleGetAllStores(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 5:
                    response = handler.HandleGetProductsOfStore(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 6:
                    response = handler.HandleGetProductDetails(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 7:
                    response = handler.HandlePurchase(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 8:
                    response = handler.HandleGetCart(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 9:
                    response = handler.HandleSearchProduct(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                case 10:
                    response = handler.HandleOpenStore(msgDict);
                    session.Send(response, 0, response.Length);
                    break;

                default:
                    break;

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
