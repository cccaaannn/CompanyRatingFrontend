namespace CompanyRatingFrontend.Data.Company;

public record CompanyDetailDto : CompanyDto
{
    public IEnumerable<CompanyCommentDto> Comments { get; init; } = [];
}