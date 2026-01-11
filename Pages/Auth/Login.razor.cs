using Blazorise;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Auth;

public partial class Login
{
    [Inject] private LoginViewModel Vm { get; set; } = null!;
    [Inject] private AccessTokenManager AccessTokenManager { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private Validations? _validations;

    protected override void OnInitialized()
    {
        if (AccessTokenManager.IsAuthenticated)
        {
            Navigation.NavigateTo("/");
        }
    }

    private async Task HandleSubmit()
    {
        if (_validations is not null && await _validations.ValidateAll())
        {
            await Vm.SubmitAsync();
        }
    }
}