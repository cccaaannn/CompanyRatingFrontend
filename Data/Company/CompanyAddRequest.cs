namespace CompanyRatingFrontend.Data.Company;

public record CompanyAddRequest
{
    public required string Name { get; set; }

    public CompanyIndustry Industry { get; set; } = CompanyIndustry.Other;

    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public static CompanyAddRequest Empty => new()
    {
        Name = string.Empty,
        Industry = CompanyIndustry.Other,
        Description = string.Empty,
        Address = string.Empty,
        City = string.Empty,
        Country = string.Empty,
        Email = string.Empty,
        Website = string.Empty,
        LogoUrl = string.Empty
    };
}