using CompanyRatingFrontend.Data;
using CompanyRatingFrontend.Data.Company;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace CompanyRatingFrontend.Pages.Companies;

public partial class Companies
{
    [Inject] private CompaniesViewModel Vm { get; set; } = null!;
    [Inject] private AccessTokenManager AccessTokenManager { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private bool _nameFilterDebounceInProgress = false;
    private const int SkeletonCount = 5;

    protected override Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
        UpdateUrlFromQuery();
        return Task.CompletedTask;
    }

    private async Task OnNameFilterChanged(string value)
    {
        Vm.DebouncedNameFilter = value;
        if (_nameFilterDebounceInProgress) return;
        _nameFilterDebounceInProgress = true;
        await Task.Delay(500);
        _nameFilterDebounceInProgress = false;

        var query = Vm.SearchQuery;
        query.Name = value;
        query.Page = 1;
        Vm.Companies.Clear();
        Vm.UpdateSearchQuery(query);
        UpdateUrlFromQuery();
    }

    private void OnIndustriesFilterChanged(IReadOnlyList<CompanyIndustry> values)
    {
        var query = Vm.SearchQuery;
        query.Industries = values.ToList();
        query.Page = 1;
        Vm.Companies.Clear();
        Vm.UpdateSearchQuery(query);
        UpdateUrlFromQuery();
    }

    private void ClearFilters()
    {
        Vm.ResetState();
        UpdateUrlFromQuery();
    }

    private void OnSortDirectionFilterChanged(SortDirection direction)
    {
        var query = Vm.SearchQuery;
        query.SortDirection = direction;
        query.Page = 1;
        Vm.Companies.Clear();
        Vm.UpdateSearchQuery(query);
        UpdateUrlFromQuery();
    }

    private void OnSortByFilterChanged(string? sortBy)
    {
        var query = Vm.SearchQuery;
        query.SortBy = sortBy;
        query.Page = 1;
        Vm.Companies.Clear();
        Vm.UpdateSearchQuery(query);
        UpdateUrlFromQuery();
    }

    private void OnLoadMore()
    {
        Vm.LoadMore();
        UpdateUrlFromQuery();
    }

    private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (NavigationManager.ToBaseRelativePath(e.Location) == "companies")
        {
            Vm.ResetState();
            UpdateUrlFromQuery();
            return;
        }

        SyncFromUri();
        await Vm.LoadCompaniesAsync();
        StateHasChanged();
    }

    private void SyncFromUri()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        var searchQuery = Vm.SearchQuery;

        if (query.TryGetValue("page", out var pageVal) && int.TryParse(pageVal, out var page))
            searchQuery.Page = page;

        if (query.TryGetValue("size", out var sizeVal) && int.TryParse(sizeVal, out var size))
            searchQuery.Size = size;

        if (query.TryGetValue("sortBy", out var sortByVal) && !string.IsNullOrWhiteSpace(sortByVal))
            searchQuery.SortBy = sortByVal!;

        if (query.TryGetValue("sortDirection", out var sortDirVal) &&
            Enum.TryParse<SortDirection>(sortDirVal, true, out var sortDir))
            searchQuery.SortDirection = sortDir;

        if (query.TryGetValue("name", out var nameVal) && !string.IsNullOrWhiteSpace(nameVal))
            searchQuery.Name = nameVal!;
        else
            searchQuery.Name = "";

        if (query.TryGetValue("industries", out var industriesVal))
        {
            searchQuery.Industries = industriesVal
                .SelectMany(v => v?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [])
                .Select(s => Enum.TryParse<CompanyIndustry>(s, true, out var industry) ? industry : (CompanyIndustry?)null)
                .Where(i => i.HasValue)
                .Select(i => i!.Value)
                .ToList();
        }
        else
        {
            searchQuery.Industries = [];
        }

        Vm.UpdateSearchQuery(searchQuery);
    }

    private void UpdateUrlFromQuery()
    {
        var baseUri = NavigationManager.Uri.Split('?')[0];
        var queryParams = new Dictionary<string, string?>
        {
            ["page"] = Vm.SearchQuery.Page.ToString(),
            ["size"] = Vm.SearchQuery.Size.ToString(),
            ["sortBy"] = Vm.SearchQuery.SortBy,
            ["sortDirection"] = Vm.SearchQuery.SortDirection.ToString(),
            ["name"] = string.IsNullOrWhiteSpace(Vm.SearchQuery.Name) ? null : Vm.SearchQuery.Name,
        };

        var newUri = QueryHelpers.AddQueryString(baseUri, queryParams!);
        if (Vm.SearchQuery.Industries != null && Vm.SearchQuery.Industries.Any())
        {
            var industriesStr = string.Join("&industries=", Vm.SearchQuery.Industries);
            newUri += $"&industries={industriesStr}";
        }

        NavigationManager.NavigateTo(newUri, replace: true);
    }

    private async Task OnRateChanged(Guid id, int rate)
    {
        await Vm.RateCompanyAsync(id, rate);
        StateHasChanged();
    }

    private void OnEdit(Guid id)
    {
        NavigationManager.NavigateTo($"/company/{id}/update");
    }

    private async Task OnDelete(Guid id)
    {
        await Vm.DeleteCompanyAsync(id);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
