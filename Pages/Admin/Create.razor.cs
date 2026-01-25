using CompanyRatingFrontend.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Admin;

public partial class Create
{
    [Inject] private CreateCompanyViewModel Vm { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private async Task OnCreate()
    {
        var companyId = await Vm.CreateCompanyAsync();
        if (companyId.HasValue)
        {
            NavigationManager.NavigateTo($"/company/{companyId.Value}");
        }
        else
        {
            StateHasChanged();
        }
    }
}