using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.Utils;
using Newtonsoft.Json;
using Server.Communication;
using Server.Communication.DataObject;
using Server.Communication.DataObject.Requests;
using Server.Communication.DataObject.Responses;
using Server.Communication.DataObject.ThinObjects;
using Server.UserComponent.Communication;
using Server.UserComponent.DomainLayer;
using SuperWebSocket;


namespace eCommerce_14a.Communication
{
    public class CommunicationHandler
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        System_Service sysService; //sundy
        StoreService storeService; //liav
        PurchaseService purchService; //naor
        NetworkSecurity security;
        private Dictionary<string, WebSocketSession> usersSessions;
        DataConverter converter;
        public CommunicationHandler()
        {
            userService = new UserService();
            storeService = new StoreService();
            appointService = new Appoitment_Service();
            purchService = new PurchaseService();
            security = new NetworkSecurity();
            sysService = new System_Service("Admin","Admin");
            usersSessions = new Dictionary<string, WebSocketSession>();
            converter = new DataConverter();
        }
        public void loaddata()
        {
            sysService.loaddata();
            //userService.Login("user6", "Test6");
            //purchService.AddProductToShoppingCart("user6", 1, 1, 3);
            //purchService.AddProductToShoppingCart("user6", 2, 1, 1);
            //purchService.AddProductToShoppingCart("user6", 2, 3, 2);
            //userService.Logout("user6");
            //userService.Logout("user4");
            //userService.Logout("user5");
        }

        public string Seralize(object obj)
        {
            string jsonString;
            jsonString = JsonConvert.SerializeObject(obj);
            return jsonString;
        }

        public Dictionary<string, object> Deseralize(string json) 
        {
            Dictionary<string,object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dict;
        }

        public Opcode GetOpCode(string msg) 
        {
            object opcodeObj;
            Dictionary<string, object> msgDict = Deseralize(msg); // desarilize the decrypted string and convert it into dict
            if (!msgDict.TryGetValue("_Opcode", out opcodeObj))
                return Opcode.ERROR;
            return (Opcode)Convert.ToInt32(opcodeObj);
        }

        public string Decrypt(byte[] cipher) 
        {
            return security.Decrypt(cipher);
        }

        public WebSocketSession GetSession(string username)
        {
            WebSocketSession session;
            if (!usersSessions.TryGetValue(username, out session))
                return null;
            return session;
        }

        public byte[] HandleLogin(string json)
        {
            LoginRequest res = JsonConvert.DeserializeObject<LoginRequest>(json);
            userService.Login(res.Username, res.Password);
            return new byte[5];
        }

        public string GetUserNameBySocket(WebSocketSession session) 
        {
            string username = "";
            try
            {
                foreach (KeyValuePair<string, WebSocketSession> entry in usersSessions)
                {
                    if (entry.Value == session)
                        username = entry.Key;
                }
            }
            catch (Exception ex) 
            {
                Logger.logError("GetUserNameBySocket failed : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                Console.WriteLine("error occured during GetUserNameBySocket");
            }
            return username;
        }

        internal byte[] HandleStatisticsNotification(Statistic_View statistics)
        {
            string jsonAns = Seralize(new NotifyStatisticsData(statistics));
            return security.Encrypt(jsonAns);
        }

        public void HandleSessionClosed(WebSocketSession session) 
        {
            userService.Logout(GetUserNameBySocket(session));
        }

        internal byte[] HandleStatistics(string json)
        {
            GetStatisticsRequest res = JsonConvert.DeserializeObject<GetStatisticsRequest>(json);
            Statistic_View ans = userService.GetStatistics(res.username, res.startTime, res.endTime);
            string jsonAns = Seralize(new NotifyStatisticsData(ans));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleLogin(string json, WebSocketSession session)
        {
            bool isAdmin = false;
            Dictionary<int, int[]> permissions = new Dictionary<int, int[]>();
            LoginRequest res = JsonConvert.DeserializeObject<LoginRequest>(json);
            if (res.Username == null || res.Password == null)
            {
                string jsonAns2 = Seralize(new LoginResponse(false, "Blank details, please try again", new Dictionary<int, int[]>()));
                return security.Encrypt(jsonAns2);
            }
            if (!usersSessions.ContainsKey(res.Username))
            {
                usersSessions.Add(res.Username, session);
            }
            if (userService.isAdmin(res.Username))
                isAdmin = true;
            Tuple<bool, string> ans = userService.Login(res.Username, res.Password);
            if (!ans.Item1 && usersSessions.ContainsKey(res.Username))
            {
                usersSessions.Remove(res.Username);
            }
            if (ans.Item1)
            {
                permissions = userService.GetUserPermissions(res.Username);
            }
            string jsonAns = Seralize(new LoginResponse(ans.Item1, ans.Item2, permissions, isAdmin));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleLogout(string json)
        {
            LogoutRequest res = JsonConvert.DeserializeObject<LogoutRequest>(json);
            Tuple<bool, string> ans = userService.Logout(res.Username);
            string jsonAns = Seralize(new LogoutResponse(ans.Item1, ans.Item2));
            usersSessions.Remove(res.Username);
            return security.Encrypt(jsonAns);
        }

        public void HandleLogoutToInit(string json)
        {
            LogoutRequest res = JsonConvert.DeserializeObject<LogoutRequest>(json);
            userService.Logout(res.Username);
        }

        public byte[] HandleRegister(string json)
        {
            RegisterRequest res = JsonConvert.DeserializeObject<RegisterRequest>(json);
            Tuple<bool, string> ans = userService.Registration(res.Username, res.Password);
            string jsonAns = Seralize(new RegisterResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAllStores(string json)
        {
            GetAllStoresRequest res = JsonConvert.DeserializeObject<GetAllStoresRequest>(json);
            List<Store> ans = storeService.GetAllStores();
            string jsonAns = Seralize(new GetStoresResponse(converter.ToStoreDataList(ans)));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleNotification(NotifyData msg)
        {
            string json = Seralize(msg);
            return security.Encrypt(json);
        }

        public byte[] HandleGetProductsOfStore(string json)
        {
            StoresProductsRequest res = JsonConvert.DeserializeObject<StoresProductsRequest>(json);
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.StoreId, res.StoreId);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(filters);
            List<Product> prodList = ans[res.StoreId];
            string jsonAns = Seralize(new GetProductsResponse(converter.ToProductDataList(prodList)));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetProductDetails(string json)
        {
            Product prod = null;
            ProductInfoRequest res = JsonConvert.DeserializeObject<ProductInfoRequest>(json);
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.StoreId, res.StoreId);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(filters);
            List<Product> prodList = ans[res.StoreId];
            foreach (Product product in prodList) 
            {
                if (product.Id == res.ProductId)
                {
                    prod = product;
                    break;
                }
            }
            string jsonAns = Seralize(new ProductInfoResponse(converter.ToProductData(prod)));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleLoginAsGuest(string json)
        {
            Tuple<bool,string> ans = userService.LoginAsGuest();
            string jsonAns = Seralize(new LoginAsGuestResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleNoConnectionPurchase(string json)
        {
            NoConnectionPurchaseRequest res = JsonConvert.DeserializeObject<NoConnectionPurchaseRequest>(json);
            Tuple<bool, string> ans = purchService.PerformPurchase(res.Username, res.PaymentDetails, res.Address, true);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleGetStoresOwnedBy(string json)
        {
            GetStoresOwnedByRequest res = JsonConvert.DeserializeObject<GetStoresOwnedByRequest>(json);
            List<Store> ans = storeService.GetStoresOwnedBy(res.User);
            string jsonAns = Seralize(new GetStoresOwnedByResponse(converter.ToStoreDataList(ans),""));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleAddProductToCart(string json)
        {
            AddProductToCartRequest res = JsonConvert.DeserializeObject<AddProductToCartRequest>(json);
            Tuple<bool,string> ans = purchService.AddProductToShoppingCart(res.User, res.Store, res.Product, res.Amount);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAllRegisteredUsers(string json)
        {
            GetAllRegisteredUsersRequest res = JsonConvert.DeserializeObject<GetAllRegisteredUsersRequest>(json);
            List<User> ans = userService.GetAllRegisteredUsers();
            string jsonAns = Seralize(new GetAllRegisteredUsersResponse(converter.ToUserNameList(ans),""));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleGetAvailablePurchases(string json)
        {
            GetAvailableRawPurchaseRequest res = JsonConvert.DeserializeObject<GetAvailableRawPurchaseRequest>(json);
            Dictionary<int, string> ans = storeService.GetAvailableRawPurchasePolicy();
            string jsonAns = Seralize(new GetAvailableRawPurchaseResponse(ans));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAvailableDiscounts(string json)
        {
            GetAvailableRawDiscountsRequest res = JsonConvert.DeserializeObject<GetAvailableRawDiscountsRequest>(json);
            Dictionary<int, string> ans = storeService.GetAvailableRawDiscount();
            string jsonAns = Seralize(new GetAvailableRawDiscountsResponse(ans));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetStaffOfStore(string json)
        {
            GetStaffOfStoreRequest res = JsonConvert.DeserializeObject<GetStaffOfStoreRequest>(json);
            Dictionary<string,string> ans = storeService.GetStaffOfStore(res.StoreId);
            string jsonAns = Seralize(new GetStaffOfStoreResponse(ans));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleAddProductToStore(string json)
        {
            AddProductToStoreRequest res = JsonConvert.DeserializeObject<AddProductToStoreRequest>(json);
            Tuple<bool,string> ans = storeService.appendProduct(res.StoreId, res.UserName, res.ProductDetails, res.ProductPrice,
                res.ProductName, res.ProductCategory, res.Pamount, res.ImgUrl);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1,ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleDecreaseProductAmount(string json)
        {
            DecreaseProductAmountRequest res = JsonConvert.DeserializeObject<DecreaseProductAmountRequest>(json);
            Tuple<bool, string> ans = storeService.decraseProduct(res.StoreId, res.Username, res.ProductId, res.Amount);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetManagersPermission(string json)
        {
            GetManagersPermissionRequest res = JsonConvert.DeserializeObject<GetManagersPermissionRequest>(json);
            List<Tuple<string, Permission>> ans = userService.GetStoreManagersPermissions(res.username, res.StoreID);
            string jsonAns = Seralize(new GetManagerPermissionResponse(ans));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleApproveAppointment(string json)
        {
            ApproveAppointmentRequest res = JsonConvert.DeserializeObject<ApproveAppointmentRequest>(json);
            Tuple<bool, string> ans = appointService.ApproveAppointment(res.Owner, res.Appointed, res.StoreID, res.Approval);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleMakeAdmin(string json)
        {
            MakeAdminRequest res = JsonConvert.DeserializeObject<MakeAdminRequest>(json);
            Tuple<bool,string> ans = userService.MakeAdmin(res.Username);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleApprovalList(string json)
        {
            GetApprovalListRequest res = JsonConvert.DeserializeObject<GetApprovalListRequest>(json);
            List<string> ans = userService.GetApprovalListByStoreAndUser(res.Username, res.StoreID);
            string jsonAns = Seralize(new GetApprovalListResponse(ans));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleDiscountPolicy(string json)
        {
            GetDiscountPolicyRequest res = JsonConvert.DeserializeObject<GetDiscountPolicyRequest>(json);
            string ans = storeService.GetDiscountPolicy(res.StoreID);
            string jsonAns = Seralize(new GetDiscountPolicyResponse(ans));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandlePurchasePolicy(string json)
        {
            GetPurchasePolicyRequest res = JsonConvert.DeserializeObject<GetPurchasePolicyRequest>(json);
            string ans = storeService.GetPurchasePolicy(res.StoreID);
            string jsonAns = Seralize(new GetPurchasePolicyResponse(ans));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleUserPermissions(string json)
        {
            Dictionary<int, int[]> permissions;
            GetUserPermissionsRequest res = JsonConvert.DeserializeObject<GetUserPermissionsRequest>(json);
            permissions = userService.GetUserPermissions(res.Username);
            string jsonAns = Seralize(new GetUserPermissionsResponse(permissions));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleChangePermissions(string json)
        {
            ChangePermissionsRequest res = JsonConvert.DeserializeObject<ChangePermissionsRequest>(json);
            Tuple<bool, string> ans = appointService.ChangePermissions(res.Owner, res.Appoint, res.StoreId, res.Permissions.ToArray());
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleIncreaseProductAmount(string json)
        {
            IncreaseProductAmountRequest res = JsonConvert.DeserializeObject<IncreaseProductAmountRequest>(json);
            Tuple<bool, string> ans = storeService.IncreaseProductAmount(res.StoreId, res.Username, res.ProductId, res.Amount);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAllUsersHistory(string json)
        {
            GetAllUsersHistoryRequest res = JsonConvert.DeserializeObject<GetAllUsersHistoryRequest>(json);
            Tuple<Dictionary<string, List<Purchase>>, string> ans = purchService.GetAllUsersHistory(res.Admin);
            Tuple<List<string>, string> convRes = converter.ToUsersHistoryResponse(ans);
            string jsonAns = Seralize(new GetAllUsersHistoryResponse(convRes.Item1, convRes.Item2));
            return security.Encrypt(jsonAns);
        }

        internal byte[] HandleGetStoreById(string json)
        {
            GetStoreByIdRequest res = JsonConvert.DeserializeObject<GetStoreByIdRequest>(json);
            Store ans = storeService.GetStoreById(res.StoreId);
            string jsonAns = Seralize(new GetStoreByIdResponse(converter.ToStoreData(ans)));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetAllStoresHistory(string json)
        {
            GetAllStoresHistoryRequest res = JsonConvert.DeserializeObject<GetAllStoresHistoryRequest>(json);
            Tuple<Dictionary<Store, List<PurchaseBasket>>, string> ans = purchService.GetAllStoresHistory(res.Admin);
            Tuple<List<StoreData>, string> convRes = converter.ToStoresHistoryResponse(ans);
            string jsonAns = Seralize(new GetAllStoresHistoryResponse(convRes.Item1, convRes.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetStoreHistory(string json)
        {
            GetStoreHistoryRequest res = JsonConvert.DeserializeObject<GetStoreHistoryRequest>(json);
            Tuple<List<PurchaseBasket>, string> ans = purchService.GetStoreHistory(res.Username, res.StoreId);
            string jsonAns = Seralize(new GetStoreHistoryResponse(converter.ToPurchaseBasketDataList(ans.Item1), ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleRemoveProductFromStore(string json)
        {
            RemoveProductFromStoreRequest res = JsonConvert.DeserializeObject<RemoveProductFromStoreRequest>(json);
            Tuple<bool, string> ans = storeService.removeProduct(res.storeId, res.userName, res.productId);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleChangeProductAmountInCart(string json)
        {
            ChangeProductAmountInCartRequest res = JsonConvert.DeserializeObject<ChangeProductAmountInCartRequest>(json);
            Tuple<bool, string> ans = purchService.ChangeProductAmoountInShoppingCart(res.User, res.Store, res.Product, res.Amount);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleUpdateProductOfStore(string json)
        {
            UpdateProductOfStoreRequest res = JsonConvert.DeserializeObject<UpdateProductOfStoreRequest>(json);
            Tuple<bool, string> ans = storeService.UpdateProduct(res.UserName, res.StoreId, res.ProductId, res.PDetails,
                res.PPrice, res.PName, res.PCategory, res.ImgUrl);
            string jsonAns = Seralize(new SuccessFailResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandlePurchase(string json)
        {
            PurchaseRequest res = JsonConvert.DeserializeObject<PurchaseRequest>(json);
            Tuple<bool, string> ans = purchService.PerformPurchase(res.Username, res.PaymentDetails, res.Address);
            string jsonAns = Seralize(new PurchaseResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleGetCart(string json)
        {
            string jsonAns;
            CartRequest res = JsonConvert.DeserializeObject<CartRequest>(json);
            Tuple<Cart, string> ans = purchService.GetCartDetails(res.Username);
            if (ans.Item1 == null)
            {
                jsonAns = Seralize(new GetUsersCartResponse(null, ans.Item2));
            }
            else
            {
                jsonAns = Seralize(new GetUsersCartResponse(converter.ToCartData(ans.Item1), ans.Item2));
            }
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleRemovePoductFromCart(string json)
        {
            RemoveProductFromCartRequest res = JsonConvert.DeserializeObject<RemoveProductFromCartRequest>(json);
            Tuple<bool, string> ans = purchService.RemoveProductFromShoppingCart(res.Username, res.StoreId, res.ProductId);
            string jsonAns = Seralize(new RemoveProductFromCartResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleUpdateDiscountPolicy(string json)
        {
            UpdateDiscountPolicyRequest res = JsonConvert.DeserializeObject<UpdateDiscountPolicyRequest>(json);
            Tuple<bool, string> ans = storeService.updateDiscountPolicy(res.storeId, res.userName, res.DiscountPolicy);
            string jsonAns = Seralize(new UpdateDiscountPolicyResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleUpdatePurchasePolicy(string json)
        {
            UpdatePurchasePolicyRequest res = JsonConvert.DeserializeObject<UpdatePurchasePolicyRequest>(json);
            Tuple<bool, string> ans = storeService.updatePurchasePolicy(res.storeId, res.userName, res.purchasePolicy);
            string jsonAns = Seralize(new UpdatePurchasePolicyResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleSearchProduct(string json) //deal with doytsh
        {
            SearchProductRequest res = JsonConvert.DeserializeObject<SearchProductRequest>(json);
            Dictionary<int, List<Product>> ans = storeService.SearchProducts(res.Filters);
            string jsonAns = Seralize(new SearchProductResponse(converter.ToSearchProductResponse(ans)));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleOpenStore(string json)
        {
            OpenStoreRequest res = JsonConvert.DeserializeObject<OpenStoreRequest>(json);
            Tuple<int, string> ans = storeService.createStore(res.Username, res.StoreName);
            bool success = ans.Item1 == -1 ? false : true;
            string jsonAns = Seralize(new OpenStoreResponse(success,ans.Item2, ans.Item1));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleBuyerHistory(string json)
        {
            BuyerHistoryRequest res = JsonConvert.DeserializeObject<BuyerHistoryRequest>(json);
            Tuple<List<Purchase>, string> ans = purchService.GetBuyerHistory(res.Username);
            string jsonAns = Seralize(new HistoryResponse(converter.ToPurchaseDataList(ans.Item1), ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleAppointManager(string json)
        {
            AppointManagerRequest res = JsonConvert.DeserializeObject<AppointManagerRequest>(json);
            Tuple<bool, string> ans = appointService.AppointStoreManage(res.Appointer,res.Appointed,res.StoreId);
            string jsonAns = Seralize(new AppointManagerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleAppointOwner(string json)
        {
            AppointOwnerRequest res = JsonConvert.DeserializeObject<AppointOwnerRequest>(json);
            Tuple<bool, string> ans = appointService.AppointStoreOwner(res.Appointer, res.Appointed, res.StoreId);
            string jsonAns = Seralize(new AppointOwnerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleDemoteManager(string json)
        {
            DemoteManagerRequest res = JsonConvert.DeserializeObject<DemoteManagerRequest>(json);
            Tuple<bool, string> ans = appointService.RemoveStoreManager(res.Appointer, res.Appointed, res.StoreId);
            string jsonAns = Seralize(new DemoteManagerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public byte[] HandleDemoteOwner(string json)
        {
            DemoteOwnerRequest res = JsonConvert.DeserializeObject<DemoteOwnerRequest>(json);
            Tuple<bool, string> ans = appointService.RemoveStoreOwner(res.Appointer, res.Appointed, res.StoreId);
            string jsonAns = Seralize(new DemoteOwnerResponse(ans.Item1, ans.Item2));
            return security.Encrypt(jsonAns);
        }

        public int SpecialHandleOpenStore(string username)
        {
            Tuple<int, string> ans = storeService.createStore(username);
            return ans.Item1;
        }
    }
}
