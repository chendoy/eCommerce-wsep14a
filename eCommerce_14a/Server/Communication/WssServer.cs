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

namespace eCommerce_14a.Communication
{
    public class WssServer
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
            client.Options.UseDefaultCredentials = true;
            wsServer.NewSessionConnected += StartSession;
            wsServer.SessionClosed += EndSession;
            wsServer.NewMessageReceived += ReceiveMessage;
            wsServer.NewDataReceived += ReceiveData;
            wsServer.Start();
            client.Options.AddSubProtocol("Tls");
            client.ConnectAsync(new Uri("wss://localhost:443"), new CancellationToken());
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
            UserData data = new UserData("blabla", "lala"); //init new user data
            string json = handler.Seralize(data); // seralize this object into json string
            byte[] arr = sec.Encrypt(json); // encrypt the string using aes algorithm and convert it to byte array
            ArraySegment<byte> msg = new ArraySegment<byte>(arr); // init client msg
            client.SendAsync(msg, WebSocketMessageType.Binary, true, new CancellationToken()); // send async the msg above to the server
            Console.WriteLine("NewSessionConnected");
        }

        private void ReceiveData(WebSocketSession session, byte[] value)
        {
            object usernameObj;
            Console.WriteLine("NewDataReceived");
            string dec = sec.Decrypt(value); // decrypt the msg and convert it into string
            Dictionary<string, object> msgDict = handler.Deseralize(dec); // desarilize the decrypted string and convert it into dict
            if (!msgDict.TryGetValue("Username", out usernameObj)) // get the username from dict
                return;


            Console.WriteLine("username:" + usernameObj.ToString());
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            Console.WriteLine("Receive Msg:" + value);
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


        private void HandleMessage(WebSocketSession session, byte[] msg)
        {
            byte[] response;
            int opcode = handler.GetOpCode(msg);
            Dictionary<string, object> msgDict = handler.GetDictFromMsg(msg);

            switch (opcode)
            {
                case 0:
                    //StoreUsernameAndSession(session, msg);
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
