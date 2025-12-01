namespace CompanyRatingFrontend.Data.Company;

public record CompanyGetQuery
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? SortBy { get; set; } = null;
    public SortDirection? SortDirection { get; set; } = null;
    public string? Name { get; set; } = null;
    public IList<CompanyIndustry>? Industries { get; set; } = null;

    public static CompanyGetQuery Default => new()
    {
        Page = 1,
        Size = 2,
        SortBy = SortByCompanyField.Name,
        SortDirection = Data.SortDirection.Ascending,
        Name = null,
        Industries = null
    };

    public string ToQueryString()
    {
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(SortBy))
        {
            queryParams.Add($"sortBy={SortBy}");
        }

        if (SortDirection.HasValue)
        {
            queryParams.Add($"sortDirection={SortDirection}");
        }

        if (!string.IsNullOrEmpty(Name))
        {
            queryParams.Add($"name={Name}");
        }

        if (Industries != null && Industries.Any())
        {
            foreach (var industry in Industries)
            {
                queryParams.Add($"industries={industry}");
            }
        }

        var otherQuery = string.Join("&", queryParams);
        var queryString = $"?page={Page}&size={Size}";
        if (!string.IsNullOrEmpty(otherQuery)) queryString += $"&{otherQuery}";

        return queryString;
    }
}