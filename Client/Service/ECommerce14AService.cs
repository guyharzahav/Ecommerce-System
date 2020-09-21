using Client.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Server.Communication.DataObject;
using Server.Communication.DataObject.ThinObjects;
using Server.Communication.DataObject.Requests;
using Server.Communication.DataObject.Responses;
using Server.UserComponent.DomainLayer;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Client.Service
{
    public class ECommerce14AService
    {
        private Communication comm;
        public ECommerce14AService()
        {
            comm = new Communication();
            Time = DateTime.Now;
            Permissions = new Dictionary<int, Permission>();
        }

        public DateTime Time { get; private set; }
        public Dictionary<int, Permission> Permissions { get; set; }

        public NotifierService NotifierService
        {
            get { return comm.NotifierService; }
        }

        public void SetPermissions(Dictionary<int, int[]> permissions)
        {
            Permissions = new Dictionary<int, Permission>();
            foreach (var item in permissions)
            {
                Permissions.Add(item.Key, new Permission(item.Value));
            }
        }

        async public Task SendStatisticsRequest(string username, DateTime? startDate, DateTime? endDate)
        {
            GetStatisticsRequest request = new GetStatisticsRequest(username, startDate, endDate);
            comm.SendRequest(request);
        }

        async public Task<List<StoreData>> GetAllActiveStores()
        {
            GetAllStoresRequest getAllStoresRequest = new GetAllStoresRequest();
            comm.SendRequest(getAllStoresRequest);
            GetStoresResponse getStoresResponse = await comm.Get<GetStoresResponse>(); 
            return getStoresResponse.Stores;
        }

        async public Task<InventoryData> GetStoresInventory(int storeId)
        {
            GetStoreByIdRequest request = new GetStoreByIdRequest(storeId);
            comm.SendRequest(request);
            GetStoreByIdResponse response = await comm.Get<GetStoreByIdResponse>();
            return response.Store.Products;
        }

        async public Task<GetUsersCartResponse> GetCart(string username)
        {
            CartRequest cartRequest = new CartRequest(username);
            comm.SendRequest(cartRequest);
            GetUsersCartResponse getCartResponse = await comm.Get<GetUsersCartResponse>();
            return getCartResponse;
        }

        async public Task<TResponse> GetResponse<TRequest, TResponse>(TRequest request)
        {
            comm.SendRequest(request);
            TResponse response = await comm.Get<TResponse>();
            return response;
        }



        async public Task<LoginResponse> Login(UserData _user)
        {
            LoginRequest loginRequest = new LoginRequest(_user.Username, _user.Password);
            comm.SendRequest(loginRequest);
            LoginResponse response = await comm.Get<LoginResponse>();
            return response;


        }

        async public Task<RegisterResponse> Register(UserData _user)
        {
            RegisterRequest registerRequest = new RegisterRequest(_user.Username, _user.Password);
            comm.SendRequest(registerRequest);
            RegisterResponse response = await comm.Get<RegisterResponse>();
            return response;
        }

        async public Task<LogoutResponse> Logout(string username)
        {
            LogoutRequest request = new LogoutRequest(username);
            comm.SendRequest(request);
            LogoutResponse response = await comm.Get<LogoutResponse>();
            if (response.Success)
            {
                Permissions = new Dictionary<int, Permission>();
            }
            return response;
        }

        async public Task<LoginAsGuestResponse> LoginAsaGuest()
        {
            LoginAsGuestRequest request = new LoginAsGuestRequest();
            comm.SendRequest(request);
            LoginAsGuestResponse loginAsGuestResponse = await comm.Get<LoginAsGuestResponse>();
            return loginAsGuestResponse;
        }

        async public Task<GetStoresOwnedByResponse> GetStoresOwnedBy(string username)
        {
            GetStoresOwnedByRequest request = new GetStoresOwnedByRequest(username);
            comm.SendRequest(request);
            GetStoresOwnedByResponse response = await comm.Get<GetStoresOwnedByResponse>();
            return response;
        }

        async public Task<GetAllRegisteredUsersResponse> GetAllActiveUsers()
        {
            GetAllRegisteredUsersRequest request = new GetAllRegisteredUsersRequest();
            comm.SendRequest(request);
            GetAllRegisteredUsersResponse response = await comm.Get<GetAllRegisteredUsersResponse>();
            return response;
        }

        async public Task<Tuple<bool, string>> AppointOwner(string appointer, string appointed, int storeId)
        {
            AppointOwnerRequest request = new AppointOwnerRequest(appointer, appointed, storeId);
            comm.SendRequest(request);
            AppointOwnerResponse response = await comm.Get<AppointOwnerResponse>();
            return new Tuple<bool, string>(response.Success, response.Error);
        }

        async public Task<Tuple<bool, string>> AppointManager(string appointer, string appointed, int storeId)
        {
            AppointManagerRequest request = new AppointManagerRequest(appointer, appointed, storeId);
            comm.SendRequest(request);
            AppointManagerResponse response = await comm.Get<AppointManagerResponse>();
            return new Tuple<bool, string>(response.Success, response.Error);
        }

        async public Task<Tuple<bool, string>> DemoteOwner(string appointer, string appointed, int storeId)
        {
            DemoteOwnerRequest request = new DemoteOwnerRequest(appointer, appointed, storeId);
            comm.SendRequest(request);
            DemoteOwnerResponse response = await comm.Get<DemoteOwnerResponse>();
            return new Tuple<bool, string>(response.Success, response.Error);
        }

        async public Task<Tuple<bool, string>> DemoteManager(string appointer, string appointed, int storeId)
        {
            DemoteManagerRequest request = new DemoteManagerRequest(appointer, appointed, storeId);
            comm.SendRequest(request);
            DemoteManagerResponse response = await comm.Get<DemoteManagerResponse>();
            return new Tuple<bool, string> (response.Success, response.Error);
        }

        async public Task<Dictionary<string, string>> GetStaff(int storeId)
        {
            GetStaffOfStoreRequest request = new GetStaffOfStoreRequest(storeId);
            comm.SendRequest(request);
            GetStaffOfStoreResponse response = await comm.Get<GetStaffOfStoreResponse>();
            return response.Staff;
        }

        async public Task<OpenStoreResponse> OpenNewStore(string username, string storename)
        {
            OpenStoreRequest request = new OpenStoreRequest(username, storename);
            comm.SendRequest(request);
            OpenStoreResponse response = await comm.Get<OpenStoreResponse>();
            return response;
        }

        async public Task<SuccessFailResponse> RemoveProductFromStore(int storeId, string username, int productId)
        {
            RemoveProductFromStoreRequest request = new RemoveProductFromStoreRequest(storeId, username, productId);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<SuccessFailResponse> IncreaseProductAmount(int storeId, string username, int productId, int delta)
        {
            IncreaseProductAmountRequest request = new IncreaseProductAmountRequest(storeId, username, productId, delta);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<SuccessFailResponse> DecreaseProductAmount(int storeId, string username, int productId, int delta)
        {
            DecreaseProductAmountRequest request = new DecreaseProductAmountRequest(storeId, username, productId, delta);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<SuccessFailResponse> AddProductToStore(int StoreId, string UserName, string ProductDetails, double ProductPrice, string ProductName, string ProductCategory, int Pamount, string ImgUrl)
        {
            AddProductToStoreRequest request = new AddProductToStoreRequest(StoreId, UserName, ProductDetails, ProductPrice, ProductName, ProductCategory, Pamount, ImgUrl);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<Dictionary<int, string>> GetRawDiscounts()
        {
            GetAvailableRawDiscountsRequest request = new GetAvailableRawDiscountsRequest();
            comm.SendRequest(request);
            GetAvailableRawDiscountsResponse response = await comm.Get<GetAvailableRawDiscountsResponse>();
            return response.DiscountPolicies;
        }
        async public Task<Dictionary<int, string>> GetRawPurchasePolcies()
        {
            GetAvailableRawPurchaseRequest request = new GetAvailableRawPurchaseRequest();
            comm.SendRequest(request);
            GetAvailableRawPurchaseResponse response = await comm.Get<GetAvailableRawPurchaseResponse>();
            return response.RawPurchases;
        }
        async public Task<SuccessFailResponse> UpdateDiscountPolicy(int StoreId, string loggedInUser, string discountText)
        {
            UpdateDiscountPolicyRequest request = new UpdateDiscountPolicyRequest(StoreId, loggedInUser, discountText);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<GetAvailableRawDiscountsResponse> GetAllCurrentDiscounts()
        {
            GetAvailableRawDiscountsRequest request = new GetAvailableRawDiscountsRequest();
            comm.SendRequest(request);
            GetAvailableRawDiscountsResponse response = await comm.Get<GetAvailableRawDiscountsResponse>();
            return response;
        }
        async public Task<SuccessFailResponse> UpdatePurchasePolicy(int StoreId, string loggedInUser, string policyText)
        {
            UpdatePurchasePolicyRequest request = new UpdatePurchasePolicyRequest(StoreId, loggedInUser, policyText);
            comm.SendRequest(request);
            SuccessFailResponse response = await comm.Get<SuccessFailResponse>();
            return response;
        }

        async public Task<string> GetPurchasePolicy(int StoreId)
        {
            GetPurchasePolicyRequest request = new GetPurchasePolicyRequest(StoreId);
            comm.SendRequest(request);
            GetPurchasePolicyResponse response = await comm.Get<GetPurchasePolicyResponse>();
            return response.PurchasePolicy;
        }

        async public Task<string> GetDiscountPolicy(int StoreId)
        {
            GetDiscountPolicyRequest request = new GetDiscountPolicyRequest(StoreId);
            comm.SendRequest(request);
            GetDiscountPolicyResponse response = await comm.Get<GetDiscountPolicyResponse>();
            return response.DiscountPolicy;
        }
    }
}
