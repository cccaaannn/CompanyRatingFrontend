using CompanyRatingFrontend.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Admin;

public partial class Update
{
    [Inject] private UpdateCompanyViewModel Vm { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public Guid CompanyId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await Vm.LoadCompanyAsync(CompanyId);
    }

    private async Task OnCreate()
    {
        var companyId = await Vm.UpdateCompanyAsync(CompanyId);
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