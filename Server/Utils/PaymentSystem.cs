using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Communication;

namespace eCommerce_14a.Utils
{
    public static class PaymentSystem
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


        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static bool IsAlive(bool Failed = false)
        {
            if(Failed)
            {
                return false;
            }
            var handshake = new Dictionary<string, string>
            {
                { "action_type", "handshake" }
            };

            string response = SendPostRequestAsyncTimeOut(handshake).Result;
            return response == "OK" ? true : false;
        }

        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static int Pay(string cardNumber, int month, int year, string holder, string ccv, string id)
        {
            var pay = new Dictionary<string, string>
            {
                { "action_type", "pay" },
                { "card_number", cardNumber },
                { "month", month.ToString() },
                { "year", year.ToString() },
                { "holder", holder },
                { "ccv", ccv },
                { "id", id },
            };

            string response = SendPostRequestAsyncTimeOut(pay).Result;
            if(response == "BAD")
            {
                return -1;
            }
            return Int32.Parse(response);
        }

        /// <test> TestingSystem.UnitTests.PaymentSystemTests</test>
        public static int CancelPayment(int transactionID)
        {
            var cancelPay = new Dictionary<string, string>
            {
                { "action_type", "cancel_pay" },
                { "transaction_id", transactionID.ToString() }
            };


            string response = SendPostRequestAsyncTimeOut(cancelPay).Result;
            if (response == "BAD")
            {
                return -1;
            }

            return Int32.Parse(response);
        }
    }
}
