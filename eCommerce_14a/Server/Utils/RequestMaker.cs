using eCommerce_14a.Communication;
using Newtonsoft.Json;
using Server.Communication.DataObject;
using Server.Communication.DataObject.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    class RequestMaker
    {
        public NetworkSecurity sec;
        public string[] usernames;
        public string[] passwords;
        public const int REQ_NUM = 500;

        public RequestMaker() 
        {
            sec = new NetworkSecurity();
            usernames = new string[REQ_NUM];
            passwords = new string[REQ_NUM];
            InitUsers();
        }

        public void GenerateBinReq()
        {
            //generate register requests
            for (int i = 0; i < REQ_NUM; i++)
            {
                SaveData(MakeRegisterRequest(usernames[i], passwords[i]),"register" + i);
            }
            //generate login requests
            for (int i = 0; i < REQ_NUM; i++)
            {
                SaveData(MakeLoginRequest(usernames[i], passwords[i]), "login" + i);
            }
            //generate logout requests
            for (int i = 0; i < REQ_NUM; i++)
            {
                SaveData(MakeLogoutRequest(usernames[i]), "logout" + i);
            }
        }

        public void InitUsers() 
        {
            for (int i = 0; i < REQ_NUM; i++) 
            {
                usernames[i] = "Guy" + i;
                passwords[i] = "Guy" + i;
            }
        }

        public byte[] MakeRegisterRequest(string username, string password) 
        {
            RegisterRequest req = new RegisterRequest(username, password);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakeLoginRequest(string username, string password)
        {
            LoginRequest req = new LoginRequest(username, password);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakeLogoutRequest(string username)
        {
            LogoutRequest req = new LogoutRequest(username);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public bool SaveData(byte[] ReqData, string reqName)
        {
            BinaryWriter Writer = null;
            string Name = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\BinaryRequests\" + reqName + @".bin";

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(new FileStream(Name, FileMode.Create));

                // Writer raw data                
                Writer.Write(ReqData);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                Console.WriteLine("cannot create file!");
                return false;
            }

            return true;
        }
    }
}
