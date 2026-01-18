using CompanyRatingFrontend.Data.Company;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Companies.Components;

public partial class CompanyCard
{
    [Parameter] public required CompanyDto Company { get; set; }
    [Parameter] public required EventCallback<int> OnRatingChanged { get; set; }
    [Parameter] public required EventCallback<Guid> OnEdit { get; set; }
    [Parameter] public required EventCallback OnDelete { get; set; }
    [Parameter] public required bool IsAdmin { get; set; }

    private string DetailPageUrl => $"/Company/{Company.Id}";
    private int AverageRating { get; set; }

    protected override void OnParametersSet()
    {
        AverageRating = (int)Math.Round(Company.AverageRating);
    }

    private async Task HandleRatingChanged(int vote)
    {
        await OnRatingChanged.InvokeAsync(vote);
    }

    private async Task HandleEdit()
    {
        if (!IsAdmin) return;
        await OnEdit.InvokeAsync(Company.Id);
    }

    private async Task HandleDelete()
    {
        if (!IsAdmin) return;
        await OnDelete.InvokeAsync();
    }
}