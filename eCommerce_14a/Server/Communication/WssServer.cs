using System;
using System.Net.WebSockets;
using CertificateConfig = SuperSocket.SocketBase.Config.CertificateConfig;
using SuperWebSocket;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.IO;
using Server.UserComponent.Communication;

namespace eCommerce_14a.Communication
{
    public class WssServer
    {


        public CommunicationHandler handler;
        private static WebSocketServer wsServer;
        private int port;
        

        public WssServer()
        {
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
            wsServer.NewSessionConnected += StartSession;
            wsServer.SessionClosed += EndSession;
            wsServer.NewMessageReceived += ReceiveMessage;
            wsServer.NewDataReceived += ReceiveData;
            wsServer.Start();
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
            HandleMessage(session, value);
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            //Console.WriteLine("Receive Msg:" + value);
        }

        public void notify(string username, NotifyData msg)
        {
            byte[] response;
            WebSocketSession session = handler.GetSession(username);
            if (session == null)
                return;
            response = handler.HandleNotification(msg);
            session.Send(response, 0, response.Length);
        }

        private void HandleMessage(WebSocketSession session, byte[] msg)
        {
            byte[] response;
            string json = handler.Decrypt(msg);
            Console.WriteLine("received: " + json);
            int opcode = handler.GetOpCode(json);

            switch (opcode)
            {
                case 1:
                    response = handler.HandleLogin(json, session);
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
            CommunicationHandler hand = new CommunicationHandler();
            WssServer notifier = new WssServer();
            notifier.InitServer();
        //    LoginRequest req = new LoginRequest("bla", "bla");
        //    string json = JsonConvert.SerializeObject(req);
        //    Console.WriteLine(json);
        //    LoginRequest res = JsonConvert.DeserializeObject<LoginRequest>(json);
        //    Console.WriteLine(res.Username);
        //    Console.WriteLine(res.Password);
        //    Console.WriteLine(res._Opcode);
        //    Console.ReadLine();
        }
    }
}
