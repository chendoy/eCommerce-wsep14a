using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Service;

namespace Client.Data
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            /*    var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "guest"),
                }, "guest");*/

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public bool TryAuthenticateUser(string username, string password)
        {
            ECommerce14AService service = new ECommerce14AService();
            User user = service.ValidateUser(username, password);

            if (user != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                }, "apiauth_type");

                var userClaim = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userClaim)));
                return true;
            }

            else
                return false;
        }
    }
}
