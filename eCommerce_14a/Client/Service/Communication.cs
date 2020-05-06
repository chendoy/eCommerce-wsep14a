using eCommerce_14a.Communication;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;

namespace Client.Service
{
    public class Communication
    {
        private ClientWebSocket client;
        private NetworkSecurity security;
        private const string PORT = "443";

        public Communication()
        {
            client = new ClientWebSocket();
            client.Options.AddSubProtocol("Tls");
            client.ConnectAsync(new Uri("wss://localhost:"+PORT), new CancellationToken());
            

            security = new NetworkSecurity();
        }

        public void SendRequest(Object obj)
        {
            string json = JsonSerializer.Serialize(obj); // seralize this object into json string
            Console.WriteLine(json);
            byte[] arr = security.Encrypt(json); // encrypt the string using aes algorithm and convert it to byte array
            ArraySegment<byte> msg = new ArraySegment<byte>(arr); // init client msg
            client.SendAsync(msg, WebSocketMessageType.Binary, true, new CancellationToken()); // send async the msg above to the server
        }

        public Object Get()
        {
            return null;
        }

        //public async System.Threading.Tasks.Task<object> GetAsync()
        //{
        //    byte[] buffer = new byte[256];
        //    ArraySegment<byte> msg = new ArraySegment<byte>(buffer); // init client msg
        //    await client.ReceiveAsync(msg, new CancellationToken());
        //    SecureDataFormatsdf
            

        //}
    }
}
