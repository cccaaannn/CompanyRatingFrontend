using CompanyRatingFrontend.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Company;

public partial class Company
{
    [Inject] private CompanyViewModel Vm { get; set; } = null!;
    [Parameter] public Guid CompanyId { get; set; }

    private string ReviewCountText => Vm.CompanyResponse.Data?.AverageRating.ToString("0.0") ?? "-" + " ⭐ "
        + $"({Vm.CompanyResponse.Data?.RatingCount ?? 0}) Reviews";

    protected override async Task OnInitializedAsync()
    {
        await Vm.LoadCompanyAsync(CompanyId);
    }

    private async Task OnComment()
    {
        await Vm.SubmitCommentAsync(CompanyId);
    }
}