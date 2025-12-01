namespace CompanyRatingFrontend.Data.Company;

public enum CompanyIndustry
{
    Technology,
    Finance,
    Health,
    Education,
    Food,
    Retail,
    Other
}

public static class CompanyIndustryExtensions
{
    public static string ToDisplayString(this CompanyIndustry industry) => industry switch
    {
        CompanyIndustry.Technology => nameof(CompanyIndustry.Technology),
        CompanyIndustry.Finance => nameof(CompanyIndustry.Finance),
        CompanyIndustry.Health => nameof(CompanyIndustry.Health),
        CompanyIndustry.Education => nameof(CompanyIndustry.Education),
        CompanyIndustry.Food => nameof(CompanyIndustry.Food),
        CompanyIndustry.Retail => nameof(CompanyIndustry.Retail),
        CompanyIndustry.Other => nameof(CompanyIndustry.Other),
        _ => "Unknown"
    };
}
