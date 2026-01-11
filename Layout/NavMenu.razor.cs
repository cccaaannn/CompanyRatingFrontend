using CompanyRatingFrontend.Managers;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Layout;

public partial class NavMenu
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private AccessTokenManager AccessTokenManager { get; set; } = null!;

    [Inject] private ThemeManager ThemeManager { get; set; } = null!;

    private bool IsAuthenticated => AccessTokenManager.IsAuthenticated;
    private string UserEmail => AccessTokenManager.TokenPayload?.Email ?? string.Empty;

    private void Logout()
    {
        _ = AccessTokenManager.ClearTokenContextAsync();
    }
}