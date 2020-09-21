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


        private static async Task<string> SendPostRequestAsyncTimeOut(Dictionary<string, string> request)
        {
            int timeout = 20000;
            var task = SendPostRequestAsync(request);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return task.Result;
            }
            else
            {
                return "BAD";
            }
        }
        private static async Task<string> SendPostRequestAsync(Dictionary<string, string> request)
        {
        

            var content = new FormUrlEncodedContent(request);
            var response = await httpClient.PostAsync(Url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        /// <test> TestingSystem.UnitTests.DeliverySystemTests</test>
        public static bool IsAlive(bool Failed = false)
        {
            if (Failed)
                return false;
            var handshake = new Dictionary<string, string>
            {
                { "action_type", "handshake" }
            };

            string response = SendPostRequestAsyncTimeOut(handshake).Result;
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

            string response = SendPostRequestAsyncTimeOut(supply).Result;
            if (response == "BAD")
            {
               return -1;
            }
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

            string response = SendPostRequestAsyncTimeOut(cancelSupply).Result;
            if (response == "BAD")
            {
                return -1;
            }
            return Int32.Parse(response);
        }

    }
}
