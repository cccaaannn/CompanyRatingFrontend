using CompanyRatingFrontend.Data;
using CompanyRatingFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.ViewModels;

public class RegisterViewModel(
    AuthService authService,
    NavigationManager nav
)
{
    public RegisterRequest Request { get; } = new();
    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }

    public async Task SubmitAsync()
    {
        Error = null;
        IsBusy = true;
        try
        {
            await authService.Register(Request);
            nav.NavigateTo("/login");
        }
        catch
        {
            Error = "Register failed. Please check your credentials.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}