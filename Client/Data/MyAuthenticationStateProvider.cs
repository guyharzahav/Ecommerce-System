using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Service;
using Client.Data;
using Blazored.SessionStorage;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;

namespace Client.Data
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;
        private ClaimsIdentity Identity;
        private bool wasSeller;

        public MyAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
            wasSeller = false;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Tuple<UserData, Dictionary<int, int[]>> userWithPerms = await _sessionStorageService.GetItemAsync<Tuple<UserData, Dictionary<int, int[]>>>("user");
            ClaimsIdentity identity;

            if (userWithPerms != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userWithPerms.Item1.Username),
                    new Claim(ClaimTypes.Role, "TestRole")
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var userAuth = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(userAuth));
        }

        public async Task<bool> MarkUserAsAuthenticateUser(UserData user, Dictionary<int, int[]> permissions, bool isAdmin)
        {

            await _sessionStorageService.SetItemAsync("user", user);
            ////await _sessionStorageService.SetItemAsync("permissions", permissions);
            ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "User")
                }, "apiauth_type"); ;


            if (permissions.Count > 0)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Seller"));
                wasSeller = true;
            }

            if (isAdmin)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            Identity = identity;


            var userClaim = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
            return true;

        }

        public bool MarkUserAsAGuest(UserData user)
        {
            _sessionStorageService.SetItemAsync("user", user);
            var identity = new ClaimsIdentity(new[]
{
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Guest")
                }, "apiauth_type");

            var userClaim = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
            return true;
        }

        public void  MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("user");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsSeller()
        {
            if (!wasSeller)
            {
                Identity.AddClaim(new Claim(ClaimTypes.Role, "Seller"));
            }

            var userClaim = new ClaimsPrincipal(Identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
        }

        public async void ChangeRole(string newRole)
        {

            Tuple<UserData, Dictionary<int, int[]>> userWithPerms = await _sessionStorageService.GetItemAsync<Tuple<UserData, Dictionary<int, int[]>>>("user");
            string username = userWithPerms.Item1.Username;

            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, newRole)
                }, "apiauth_type");

            var userClaim = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
        }

        public async Task<string> GetLoggedInUsername()
        {
            UserData user = await _sessionStorageService.GetItemAsync<UserData>("user");
            return user.Username;
        }
    }
}
