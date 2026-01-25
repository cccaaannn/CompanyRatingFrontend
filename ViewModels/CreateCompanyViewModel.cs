using System.ComponentModel.DataAnnotations;
using CompanyRatingFrontend.Data.Company;
using CompanyRatingFrontend.Services;

namespace CompanyRatingFrontend.ViewModels;

public class CreateCompanyViewModel(CompanyService companyService)
{
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();

    public CompanyAddRequest NewCompany { get; set; } = CompanyAddRequest.Empty;
    public bool ErrorToastVisible { get; set; }
    public bool IsMutating { get; private set; }
    public bool CreateButtonDisabled => !IsFormValid() || IsMutating;

    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(NewCompany.Name)) return false;

        if (!string.IsNullOrWhiteSpace(NewCompany.Website) &&
            !Uri.IsWellFormedUriString(NewCompany.Website, UriKind.Absolute))
            return false;

        if (!string.IsNullOrWhiteSpace(NewCompany.LogoUrl) &&
            !Uri.IsWellFormedUriString(NewCompany.LogoUrl, UriKind.Absolute))
            return false;

        if (!string.IsNullOrWhiteSpace(NewCompany.Email) &&
            !EmailAddressAttribute.IsValid(NewCompany.Email))
            return false;

        return true;
    }

    public async Task<Guid?> CreateCompanyAsync()
    {
        if (IsMutating || !IsFormValid()) return null;
        IsMutating = true;
        ErrorToastVisible = false;

        try
        {
            var createdCompany = await companyService.AddAsync(NewCompany);
            NewCompany = CompanyAddRequest.Empty;
            return createdCompany.Id;
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