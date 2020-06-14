using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace eCommerce_14a.Utils
{
    public static class DeliverySystem
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string Url = "https://cs-bgu-wsep.herokuapp.com";

        private static async Task<string> SendPostRequestAsync(Dictionary<string, string> request)
        {
            var content = new FormUrlEncodedContent(request);
            var response = await httpClient.PostAsync(Url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static bool IsAlive()
        {
            var handshake = new Dictionary<string, string>
            {
                { "action_type", "handshake" }
            };

            string response = SendPostRequestAsync(handshake).Result;
            return response == "OK" ? true : false;
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static int Supply(string name, string address, string city, string country, string zip)
        {
            var supply = new Dictionary<string, string>
            {
                { "action_type", "supply" },
                { "name",  name},
                { "address",  address},
                { "city",  city},
                { "country",  country},
                { "zip",  zip},
            };

            string response = SendPostRequestAsync(supply).Result;
            return Int32.Parse(response);
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static int CancelSupply(int transactionID)
        {
            var cancelSupply = new Dictionary<string, string>
            {
                { "action_type", "cancel_supply" },
                { "transaction_id", transactionID.ToString() }
            };

            string response = SendPostRequestAsync(cancelSupply).Result;
            return Int32.Parse(response);
        }

    }
}
