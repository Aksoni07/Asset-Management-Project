using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;// Provides the tools to create a user's identity, like a Claim for name, role EmpId

namespace AssetManagement.UI.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        // ClaimsPrincipal: complete identity Doc, Holds one or more identities -> check login status
        // ClaimsIdentity: single identity, contains claims and know how the user is authanticated

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

                
                //builds the entire Identity of User
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
                userSession.ExpiryTimestamp = DateTime.UtcNow.AddMinutes(60); // Updating Expiration Time from now
                await _sessionStorage.SetAsync("UserSession", userSession);
            }
        }

        // method to change the authantication state, Login <-->logout
        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null) // checking In
            {
                await _sessionStorage.SetAsync("UserSession", userSession); // saves the user's session data , in browser's secure storage.
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, userSession.UserName) })); // Overall Indentity generated
            }
            else // checking out
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task Logout() // clear user's session data from browser's protected storage
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