using CompanyRatingFrontend.Data;
using CompanyRatingFrontend.Data.Company;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.Services;

namespace CompanyRatingFrontend.ViewModels;

public class CompaniesViewModel(CompanyService companyService)
{
    public List<CompanyDto> Companies { get; } = [];
    public ApiResult<PagedList<CompanyDto>> CompaniesResponse { get; private set; } = ApiResult<PagedList<CompanyDto>>.Empty();
    public bool HasNext { get; private set; }
    public bool IsMutating { get; private set; }
    public bool RatedToastVisible { get; set; }
    public bool ErrorToastVisible { get; set; }
    public string DebouncedNameFilter { get; set; } = string.Empty;
    public CompanyGetQuery SearchQuery { get; private set; } = CompanyGetQuery.Default;

    public async Task LoadCompaniesAsync()
    {
        CompaniesResponse = await QueryManager.Query(async () => await companyService.GetAsync(SearchQuery));
        if (CompaniesResponse.Data == null) return;
        Companies.AddRange(CompaniesResponse.Data.Content);
        HasNext = CompaniesResponse.Data.HasNext;
    }

    public void UpdateSearchQuery(CompanyGetQuery query)
    {
        SearchQuery = query;
    }

    public void LoadMore()
    {
        if (!HasNext) return;
        SearchQuery.Page += 1;
    }

    public async Task RateCompanyAsync(Guid id, int rate)
    {
        if (IsMutating) return;
        IsMutating = true;

        try
        {
            var request = new CompanyRatingRequest { Rating = rate };
            await companyService.RateAsync(id, request);
            RatedToastVisible = true;
        }
        catch (Exception)
        {
            ErrorToastVisible = true;
        }
        finally
        {
            IsMutating = false;
        }
    }

    public async Task DeleteCompanyAsync(Guid id)
    {
        if (IsMutating) return;
        IsMutating = true;

        try
        {
            await companyService.DeleteAsync(id);
            ResetState();
        }
        catch (Exception)
        {
            ErrorToastVisible = true;
        }
        finally
        {
            IsMutating = false;
        }
    }

    public void ResetState()
    {
        Companies.Clear();
        HasNext = false;
        IsMutating = false;
        RatedToastVisible = false;
        ErrorToastVisible = false;
        CompaniesResponse = ApiResult<PagedList<CompanyDto>>.Empty();
        SearchQuery = CompanyGetQuery.Default;
        DebouncedNameFilter = string.Empty;
    }
}
