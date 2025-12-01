namespace CompanyRatingFrontend.Data.Company;

public record CompanyAddRequest
{
    public required string Name { get; init; }

    public CompanyIndustry Industry { get; init; } = CompanyIndustry.Other;

    public string Description { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Website { get; init; } = string.Empty;

    public string LogoUrl { get; init; } = string.Empty;
}