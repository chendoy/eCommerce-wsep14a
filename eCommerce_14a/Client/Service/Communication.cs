﻿using eCommerce_14a.Communication;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Collections.Concurrent;
using Server.Communication.DataObject;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server.UserComponent.Communication;

namespace Client.Service
{
    public class Communication
    {
        private WebSocket client;
        private NetworkSecurity security;
        private const string PORT = "443";
        private const string URL = "wss://localhost:" + PORT;
        private BlockingCollection<string> responses; // store as json

        public Communication()
        {
            //client = new ClientWebSocket();
            //client.Options.AddSubProtocol("Tls");
            //client.ConnectAsync(new Uri("wss://localhost:"+PORT), new CancellationToken());
            
            security = new NetworkSecurity();
            responses = new BlockingCollection<string>();
            NotifierService = new NotifierService();

            client = new WebSocket(URL);
            //client.cre
            client.Connect();
            client.OnMessage += Client_OnMessage;
        }

        public NotifierService NotifierService { get; set; }

        private async void Client_OnMessage(object sender, MessageEventArgs e)
        {
            byte[] byteMsg = e.RawData;
            string json = security.Decrypt(byteMsg);
            Dictionary<string, object> resDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (resDict.TryGetValue("_Opcode", out object opcodeObj))
            {
                int opcode = Convert.ToInt32(opcodeObj);
                if (opcode == (int)Opcode.NOTIFICATION)
                {
                    NotifyData notifyData = JsonConvert.DeserializeObject<NotifyData>(json);
                    await NotifierService.Update(notifyData.Context);
                }
                else
                {
                    responses.Add(json);
                }
            }
        }

        public void SendRequest(Object obj)
        {
            string json = JsonConvert.SerializeObject(obj); // seralize this object into json string
            Console.WriteLine("sent: " + json);
            byte[] arr = security.Encrypt(json); // encrypt the string using aes algorithm and convert it to byte array
            //ArraySegment<byte> msg = new ArraySegment<byte>(arr); // init client msg
            client.Send(arr);
            //client.SendAsync(msg, WebSocketMessageType.Binary, true, new CancellationToken()); // send async the msg above to the server
        }

        public async Task<T> Get<T>()
        {
            string json = responses.Take();
            T response = JsonConvert.DeserializeObject<T>(json);
            return response;
        }


    }
}
