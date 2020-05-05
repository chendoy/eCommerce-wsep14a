using eCommerce_14a.Communication;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            Console.WriteLine("sent: " + json);
            byte[] arr = security.Encrypt(json); // encrypt the string using aes algorithm and convert it to byte array
            ArraySegment<byte> msg = new ArraySegment<byte>(arr); // init client msg
            client.SendAsync(msg, WebSocketMessageType.Binary, true, new CancellationToken()); // send async the msg above to the server
        }

        public async Task<T> Get<T>()
        {
            byte[] byteArr = new byte[1024];
            ArraySegment<byte> buffer = new ArraySegment<byte>(byteArr); // init client msg
            await client.ReceiveAsync(buffer, new CancellationToken());
            byte[] ans = byteArr.TakeWhile((v, index) => byteArr.Skip(index).Any(w => w != 0x00)).ToArray();
            string json = security.Decrypt(ans);
            T response = JsonSerializer.Deserialize<T>(json);
            return response;
        }


    }
}
