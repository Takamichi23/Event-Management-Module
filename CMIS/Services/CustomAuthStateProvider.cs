using System.Security.Claims;
using CMIS.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CMIS.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await _sessionStorage.GetAsync<UserSession>("UserSession");
            if (userSession.Success && userSession.Value is not null)
            {
                var claims = BuildClaims(userSession.Value);
                var identity = new ClaimsIdentity(claims, "CustomAuth");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }
        catch
        {
            // Session storage is not available during prerender
        }

        return new AuthenticationState(_anonymous);
    }

    public async Task MarkUserAsAuthenticated(Account account)
    {
        var session = new UserSession
        {
            AccountId = account.AccountId,
            Username = account.Username,
            Email = account.Email,
            RoleName = account.Role.RoleName,
            FullName = $"{account.Profile.FirstName} {account.Profile.LastName}"
        };

        await _sessionStorage.SetAsync("UserSession", session);

        var claims = BuildClaims(session);
        var identity = new ClaimsIdentity(claims, "CustomAuth");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _sessionStorage.DeleteAsync("UserSession");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    private static List<Claim> BuildClaims(UserSession session)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.AccountId.ToString()),
            new(ClaimTypes.Name, session.Username),
            new(ClaimTypes.Email, session.Email),
            new(ClaimTypes.Role, session.RoleName),
            new("FullName", session.FullName)
        };
    }
}

/// <summary>
/// Lightweight session data stored in protected browser session storage.
/// </summary>
public class UserSession
{
    public int AccountId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
