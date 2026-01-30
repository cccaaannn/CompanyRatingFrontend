using System.ComponentModel.DataAnnotations;
using CompanyRatingFrontend.Data.Company;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.Services;

namespace CompanyRatingFrontend.ViewModels;

public class UpdateCompanyViewModel(CompanyService companyService)
{
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();
    
    public ApiResult<CompanyDetailDto> CompanyResponse { get; private set; } = ApiResult<CompanyDetailDto>.Empty();
    public CompanyUpdateRequest UpdateCompany { get; set; } = CompanyUpdateRequest.Empty;
    public bool ErrorToastVisible { get; set; }
    public bool IsMutating { get; private set; }
    public bool UpdateButtonDisabled => !IsFormValid() || IsMutating;

    public async Task LoadCompanyAsync(Guid companyId)
    {
        CompanyResponse = await QueryManager.Query(async () => await companyService.GetAsync(companyId));
        if (CompanyResponse.Data != null)
        {
            var company = CompanyResponse.Data;
            UpdateCompany = new CompanyUpdateRequest
            {
                Name = company.Name,
                Industry = company.Industry,
                Description = company.Description,
                Address = company.Address,
                City = company.City,
                Country = company.Country,
                Email = company.Email,
                Website = company.Website,
                LogoUrl = company.LogoUrl
            };
        }
    }

    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(UpdateCompany.Name)) return false;
        
        if (!string.IsNullOrWhiteSpace(UpdateCompany.Website) && 
            !Uri.IsWellFormedUriString(UpdateCompany.Website, UriKind.Absolute)) 
            return false;
        
        if (!string.IsNullOrWhiteSpace(UpdateCompany.LogoUrl) && 
            !Uri.IsWellFormedUriString(UpdateCompany.LogoUrl, UriKind.Absolute)) 
            return false;
        
        if (!string.IsNullOrWhiteSpace(UpdateCompany.Email) && 
            !EmailAddressAttribute.IsValid(UpdateCompany.Email)) 
            return false;

        return true;
    }

    public async Task<Guid?> UpdateCompanyAsync(Guid companyId)
    {
        if (IsMutating || !IsFormValid()) return null;
        IsMutating = true;
        ErrorToastVisible = false;

        try
        {
            var updatedCompany = await companyService.UpdateAsync(companyId, UpdateCompany);
            UpdateCompany = CompanyUpdateRequest.Empty;
            return updatedCompany.Id;
        }
        catch (Exception)
        {
            ErrorToastVisible = true;
            return null;
        }
        finally
        {
            IsMutating = false;
        }
    }
}
