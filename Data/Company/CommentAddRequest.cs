namespace CompanyRatingFrontend.Data.Company;

public record CommentAddRequest
{
    public required string Content { get; init; } = string.Empty;
}