using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace AssetManagement.UI.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionResult.Success ? userSessionResult.Value : null;

                // If there is no session OR the session has expired...
                if (userSession == null || DateTime.UtcNow > userSession.ExpiryTimestamp)
                {
                    //...clear the storage and return an anonymous (logged out) state.
                    await _sessionStorage.DeleteAsync("UserSession");
                    return new AuthenticationState(_anonymous);
                }

                // IMPORTANT: We NO LONGER update the timestamp here. This method now only reads the state.

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName)
                }, "CustomAuth"));

                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        // This is a new method to explicitly refresh the session timer
        public async Task RefreshSessionExpiration()
        {
            var sessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
            if (sessionResult.Success && sessionResult.Value != null)
            {
                var userSession = sessionResult.Value;
                userSession.ExpiryTimestamp = DateTime.UtcNow.AddMinutes(60); // Change to 20 for production
                await _sessionStorage.SetAsync("UserSession", userSession);
            }
        }

        // The rest of the file remains the same
        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, userSession.UserName) }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task Logout()
        {
            await UpdateAuthenticationState(null);
        }
    }

    public class UserSession
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime ExpiryTimestamp { get; set; }
    }
}