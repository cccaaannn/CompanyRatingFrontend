using CompanyRatingFrontend.Data;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.ViewModels;

public class LoginViewModel(
    AuthService authService,
    NavigationManager nav,
    AccessTokenManager accessTokenManager
)
{
    public LoginRequest Request { get; } = new();
    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }

    public async Task SubmitAsync()
    {
        Error = null;
        IsBusy = true;
        try
        {
            var token = await authService.Login(Request);
            await accessTokenManager.SetAccessTokenAsync(token.AccessToken);

            nav.NavigateTo("/");
        }
        catch
        {
            Error = "Login failed. Please check your credentials.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}