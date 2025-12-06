namespace CompanyRatingFrontend.Data.Config;

public record ApiSettings
{
    public static readonly string SectionName = nameof(ApiSettings);

    public string BaseUrl { get; set; }
}