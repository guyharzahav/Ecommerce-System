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
        public const int REQ_NUM = 1001;

        public RequestMaker() 
        {
            sec = new NetworkSecurity();
            usernames = new string[10001];
            passwords = new string[10001];
            InitUsers();
        }

        public void GenerateBinReq()
        {
            ////generate register requests
            //for (int i = 1; i < 10001; i++)
            //{
            //    SaveData(MakeRegisterRequest(usernames[i], passwords[i]),"register" + i);
            //}
            ////generate login requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeLoginRequest(usernames[i], passwords[i]), "login" + i);
            //}
            ////generate logout requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeLogoutRequest(usernames[i]), "logout" + i);
            //}
            ////generate open store requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeOpenStoreRequest(usernames[i], "Store" + i), "openstore" + i); //should insert legal payment details for users
            //}
            ////generate add to cart product requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeAddToCartRequest(usernames[i], 1, 1, 1), "addprodtocart" + i); //should exist store with id 1 and product id 1 with amount  > REQ_NUM
            //}
            //generate perform purchase requests
            for (int i = 1; i < REQ_NUM; i++)
            {
                SaveData(MakePerformPurchaseRequset(usernames[i], "Guy&Hanesher3&Eilat&Israel&88000", "12345698754&3&2021&Guy&104&20661314"), "purchase" + i); //should insert legal payment details for users
            }
            ////generate add product to store requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeAddProductToStoreRequset(i, usernames[i], "prodDetails", 10, "prodName", "prodCategory", 1000), "addprodtostore" + i); //should insert legal payment details for users
            //}
            ////generate login as guest requests
            //for (int i = 1; i < REQ_NUM; i++)
            //{
            //    SaveData(MakeLoginAsGuestRequest(), "loginasguest" + i);
            //}

            //generate no connection purchase requests
            //for (int i = 1; i < 101; i++)
            //{
            //    SaveData(MakePerformPurchaseNoConnectionRequset(usernames[i], "Guy&Hanesher3&Eilat&Israel&88000", "12345698754&3&2021&Guy&104&20661314"), "noconnectionpurchase" + i);
            //}

        }

        public void InitUsers() 
        {
            for (int i = 1; i < 10001; i++) 
            {
                usernames[i] = "Guy" + i;
                passwords[i] = "Guy" + i;
            }
        }


        public byte[] MakeLoginAsGuestRequest()
        {
            LoginAsGuestRequest req = new LoginAsGuestRequest();
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakeOpenStoreRequest(string username, string storeName = "Store")
        {
            OpenStoreRequest req = new OpenStoreRequest(username, storeName);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakeAddProductToStoreRequset(int storeId, string userName, string productDetails, double productPrice,
            string productName, string productCategory, int pamount)
        {
            AddProductToStoreRequest req = new AddProductToStoreRequest(storeId, userName, productDetails, productPrice, productName, productCategory, pamount);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakePerformPurchaseRequset(string username, string address, string paymentDetails)
        {
            PurchaseRequest req = new PurchaseRequest(username, address, paymentDetails);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakePerformPurchaseNoConnectionRequset(string username, string address, string paymentDetails)
        {
            NoConnectionPurchaseRequest req = new NoConnectionPurchaseRequest(username, address, paymentDetails);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
        }

        public byte[] MakeAddToCartRequest(string user, int store, int product, int amount) 
        {
            AddProductToCartRequest req = new AddProductToCartRequest(user, store, product, amount);
            string jsonString = JsonConvert.SerializeObject(req);
            return sec.Encrypt(jsonString);
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
            //string Name = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\BinaryRequests\" + reqName + @".bin";
            string Name = @"C: \Users\guyha\Desktop\BinaryRequests\" + reqName + @".bin";

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

        //public static void Main(String[] args)
        //{
        //    RequestMaker reqmaker = new RequestMaker();
        //    reqmaker.GenerateBinReq();
        //}

    }

}
