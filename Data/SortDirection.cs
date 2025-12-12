namespace CompanyRatingFrontend.Data;

public enum SortDirection
{
    Ascending,
    Descending
}

public static class SortDirectionExtensions
{
    public static string ToDisplayString(this SortDirection direction) => direction switch
    {
        SortDirection.Ascending => nameof(SortDirection.Ascending),
        SortDirection.Descending => nameof(SortDirection.Descending),
        _ => ""
    };
}