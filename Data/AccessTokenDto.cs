namespace CompanyRatingFrontend.Data;

public record AccessTokenDto
{
    public required string AccessToken { get; init; }

    public required DateTime Expires { get; init; }
}