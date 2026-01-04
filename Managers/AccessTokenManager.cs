using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using CompanyRatingFrontend.Data;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Managers;

public class AccessTokenManager(IServiceProvider serviceProvider)
{
    private const string AccessTokenKey = "AccessToken";

    private Timer? _tokenExpirationTimer;

    private DateTime? TokenExpiration { get; set; }

    public event Action? OnAuthStateChanged;

    public string? AccessToken { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

    public bool IsAdmin => TokenPayload?.Role == AppRole.Admin;

    public ClaimsPrincipal? User { get; private set; }

    public TokenPayload? TokenPayload { get; private set; }

    public async Task InitializeAsync()
    {
        OnAuthStateChanged += HandleUnAuthorizedRedirect;

        var localStorage = GetLocalStorageService();

        var token = await localStorage.GetItemAsStringAsync(AccessTokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            SetTokenContext(token);
        }
    }

    public async Task SetAccessTokenAsync(string token)
    {
        var localStorage = GetLocalStorageService();

        await localStorage.SetItemAsStringAsync(AccessTokenKey, token);
        SetTokenContext(token);
    }

    public async Task ClearTokenContextAsync()
    {
        AccessToken = null;
        User = null;

        if (_tokenExpirationTimer is not null)
        {
            await _tokenExpirationTimer.DisposeAsync();
        }

        _tokenExpirationTimer = null;

        var localStorage = GetLocalStorageService();
        await localStorage.RemoveItemAsync(AccessTokenKey);

        OnAuthStateChanged?.Invoke();
    }

    private void SetTokenContext(string token)
    {
        AccessToken = token;
        var (user, tokenPayload) = GetTokenClaims(token);
        User = user;
        TokenPayload = tokenPayload;
        TokenExpiration = tokenPayload?.Expiration;
        StartTokenExpirationTimer();

        OnAuthStateChanged?.Invoke();
    }

    private void StartTokenExpirationTimer()
    {
        _tokenExpirationTimer?.Dispose();
        if (TokenExpiration is null) return;

        var timeUntilExpiration = TokenExpiration - DateTime.UtcNow;
        if (timeUntilExpiration < TimeSpan.Zero)
        {
            Task.Run(ClearTokenContextAsync);
            return;
        }

        _tokenExpirationTimer = new Timer(
            _ => Task.Run(ClearTokenContextAsync),
            null,
            (TimeSpan)timeUntilExpiration,
            Timeout.InfiniteTimeSpan
        );
    }

    private (ClaimsPrincipal?, TokenPayload?) GetTokenClaims(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            var expirationTime = jwtToken.ValidTo;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var tokenPayload = new TokenPayload
            {
                Email = email ?? string.Empty,
                Role = AppRoleExtensions.FromString(role),
                Expiration = expirationTime
            };
            return (user, tokenPayload);
        }
        catch
        {
            return (null, null);
        }
    }

    private ILocalStorageService GetLocalStorageService()
    {
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
    }

    private NavigationManager GetNavigationManager()
    {
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<NavigationManager>();
    }

    private void HandleUnAuthorizedRedirect()
    {
        var nav = GetNavigationManager();
        if (!IsAuthenticated)
        {
            nav.NavigateTo("/auth/login");
        }
    }
}