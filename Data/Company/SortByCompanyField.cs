namespace CompanyRatingFrontend.Data.Company;

public class SortByCompanyField
{
    public const string Name = nameof(CompanyDto.Name);
    public const string Industry = nameof(CompanyDto.Industry);
    public const string City = nameof(CompanyDto.City);
    public const string Country = nameof(CompanyDto.Country);

    public static readonly List<string> AllFields = [Name, Industry, City, Country];
}
