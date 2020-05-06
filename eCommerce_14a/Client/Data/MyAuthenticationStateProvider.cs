using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Service;
using Client.Data;
using Blazored.SessionStorage;

namespace Client.Data
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;

        public MyAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await _sessionStorageService.GetItemAsync<User>("user");
            ClaimsIdentity identity;

            if (user != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Roles[0])
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }
            
            var userAuth = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(userAuth));
        }

        public bool MarkUserAsAuthenticateUser(User user)
        {

            _sessionStorageService.SetItemAsync("user", user);

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Roles[0])
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

        public async void ChangeRole(string newRole)
        {

            User user = await _sessionStorageService.GetItemAsync<User>("user");
            string username = user.Username;

            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, newRole)
                }, "apiauth_type");

            var userClaim = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
        }
    }
}
