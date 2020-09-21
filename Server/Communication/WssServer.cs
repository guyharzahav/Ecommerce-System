using System;
using CertificateConfig = SuperSocket.SocketBase.Config.CertificateConfig;
using SuperWebSocket;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.IO;
using Server.UserComponent.Communication;
using Newtonsoft.Json;
using Server.Communication.DataObject.Requests;
using System.Reflection.Emit;
using Server.Communication.DataObject;
using Server.Communication.DataObject.Responses;
using System.Collections.Generic;
using Server.Communication.DataObject.ThinObjects;
using System.Linq;
using Server.Utils;
using System.Reflection;
using log4net;
using log4net.Config;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.Communication
{
    public class WssServer
    {
        public CommunicationHandler handler;
        private static WebSocketServer wsServer;
        private int port;

        public WssServer()
        {
            //Start all services 
            //Services will start the domain layer
            handler = new CommunicationHandler();
            wsServer = new WebSocketServer();
            Publisher.Instance.setServer(this);
            LoadData();
        }
        private void LoadData()
        {
            handler.loaddata();
        }
        private void InitServer()
        {
            port = 443;
            var config1 = new ServerConfig();
            config1.Port = port;
            config1.MaxConnectionNumber = 100000;
            config1.Security = "Tls";
            config1.LogAllSocketException = false;
            config1.LogBasicSessionActivity = false;
            config1.LogCommand = false;
            
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
            Console.WriteLine("Server is running on port " + ". Press ENTER to exit....");
            Console.ReadKey();
            wsServer.Stop();
        }


        private void EndSession(WebSocketSession session, CloseReason value)
        {
            handler.HandleSessionClosed(session);
            Console.WriteLine("SessionClosed");
        }

        private void StartSession(WebSocketSession session)
        {
            //checkFunc(); // for tests
            Console.WriteLine("NewSessionConnected");
        }

        private void ReceiveData(WebSocketSession session, byte[] value)
        {
            //Console.WriteLine("Receive Msg:" + value.ToString()) ;
            HandleMessage(session, value);
        }

        private void ReceiveMessage(WebSocketSession session, string value)
        {
            Console.WriteLine("Receive Msg:" + value);
        }

        public void notify(string username, NotifyData msg)
        {
            byte[] response;
            //get_session:
            WebSocketSession session = handler.GetSession(username);
            if (session == null)
                //goto get_session;
                return;
            response = handler.HandleNotification(msg);
            session.Send(response, 0, response.Length);
        }

        public void notifyStatistics(string admin, Statistic_View statistics)
        {
            byte[] response;
            WebSocketSession session = handler.GetSession(admin);
            if (session == null)
                return;
            response = handler.HandleStatisticsNotification(statistics);
            session.Send(response, 0, response.Length);
        }


        private void HandleMessage(WebSocketSession session, byte[] msg)
        {
            byte[] response;
            string json = handler.Decrypt(msg);
            Console.WriteLine("received: " + json);
            Opcode opcode = handler.GetOpCode(json);

            switch (opcode)
            {
                case Opcode.LOGIN:
                    response = handler.HandleLogin(json, session);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.LOGOUT:
                    response = handler.HandleLogout(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.REGISTER:
                    response = handler.HandleRegister(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.ALL_STORES:
                    response = handler.HandleGetAllStores(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PRODUCTS_OF_STORE:
                    response = handler.HandleGetProductsOfStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PROD_INFO:
                    response = handler.HandleGetProductDetails(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PURCHASE:
                    response = handler.HandlePurchase(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.USER_CART:
                    response = handler.HandleGetCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.SEARCH_PROD:
                    response = handler.HandleSearchProduct(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.OPEN_STORE:
                    response = handler.HandleOpenStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.BUYER_HISTORY:
                    response = handler.HandleBuyerHistory(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.APPOINT_MANAGER:
                    response = handler.HandleAppointManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.APPOINT_OWNER:
                    response = handler.HandleAppointOwner(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.DEMOTE_MANAGER:
                    response = handler.HandleDemoteManager(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.DEMOTE_OWNER:
                    response = handler.HandleDemoteOwner(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.LOGIN_AS_GUEST:
                    response = handler.HandleLoginAsGuest(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.REMOVE_PRODUCT_FROM_CART:
                    response = handler.HandleRemovePoductFromCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.UPDATE_DISCOUNT_POLICY:
                    response = handler.HandleUpdateDiscountPolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.UPDATE_PURCHASE_POLICY:
                    response = handler.HandleUpdatePurchasePolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.STORES_OWNED_BY:
                    response = handler.HandleGetStoresOwnedBy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_ALL_REGISTERED_USERS:
                    response = handler.HandleGetAllRegisteredUsers(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.ADD_PRODUCT_TO_CART:
                    response = handler.HandleAddProductToCart(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.CHANGE_PRODUCT_AMOUNT_CART:
                    response = handler.HandleChangeProductAmountInCart(json);
                    session.Send(response, 0, response.Length);
                    break;
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
                case Opcode.GET_STAFF_OF_STORE:
                    response = handler.HandleGetStaffOfStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_AVAILABLE_DISCOUNTS:
                    response = handler.HandleGetAvailableDiscounts(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_AVAILABLE_PURCHASES:
                    response = handler.HandleGetAvailablePurchases(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.ADD_PRODUCT_TO_STORE:
                    response = handler.HandleAddProductToStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.REMOVE_PRODUCT_FROM_STORE:
                    response = handler.HandleRemoveProductFromStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.UPDATE_PRODUCT_OF_STORE:
                    response = handler.HandleUpdateProductOfStore(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.STORE_HISTORY:
                    response = handler.HandleGetStoreHistory(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.ALL_STORE_HISTORY:
                    response = handler.HandleGetAllStoresHistory(json);
                    session.Send(response, 0, response.Length);
                    break;


                case Opcode.ALL_BUYERS_HISTORY:
                    response = handler.HandleGetAllUsersHistory(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.STORE_BY_ID:
                    response = handler.HandleGetStoreById(json);
                    session.Send(response, 0, response.Length);
                    break;


                case Opcode.INCREASE_PRODUCT_AMOUNT:
                    response = handler.HandleIncreaseProductAmount(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.DECREASE_PRODUCT_AMOUNT:
                    response = handler.HandleDecreaseProductAmount(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_MANAGER_PERMISSION:
                    response = handler.HandleGetManagersPermission(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.CHANGE_PERMISSIONS:
                    response = handler.HandleChangePermissions(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.APPROVE_APPOINTMENT:
                    response = handler.HandleApproveAppointment(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_APPROVAL_LIST:
                    response = handler.HandleApprovalList(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_USER_PERMISSIONS:
                    response = handler.HandleUserPermissions(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_DISCOUNT_POLICY:
                    response = handler.HandleDiscountPolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_PURCHASE_POLICY:
                    response = handler.HandlePurchasePolicy(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.MAKE_ADMIN:
                    response = handler.HandleMakeAdmin(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.GET_STATISTICS:
                    response = handler.HandleStatistics(json);
                    session.Send(response, 0, response.Length);
                    break;

                case Opcode.PURCHASE_NO_CONNECTION:
                    response = handler.HandleNoConnectionPurchase(json);
                    session.Send(response, 0, response.Length);
                    break;


                default:
                    break;
            }
        }


        public void checkFunc() 
        {
            handler.HandleRegister(JsonConvert.SerializeObject(new RegisterRequest("admin", "admin")));
            for (int i = 0; i < 10; i++)
            {
                notify("admin", new NotifyData("Notify Number : " + i.ToString()));
            }

        }


        public static void Main(string[] argv)
        {
            StateInitiator init = new StateInitiator();
            WssServer server = new WssServer();
            init.InitSystemFromFile();
            server.InitServer();
        }
    }
}
