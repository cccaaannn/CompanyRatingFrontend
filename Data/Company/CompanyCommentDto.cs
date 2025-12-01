namespace CompanyRatingFrontend.Data.Company;

public record CompanyCommentDto
{
    public Guid Id { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string UserSurname { get; init; } = string.Empty;
}