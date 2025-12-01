namespace CompanyRatingFrontend.Data.Company;

public record CompanyDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;

    public CompanyIndustry Industry { get; init; } = CompanyIndustry.Other;

    public string Description { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Website { get; init; } = string.Empty;

    public string LogoUrl { get; init; } = string.Empty;

    public double AverageRating { get; init; } = 0.0;

    public int RatingCount { get; init; } = 0;
}