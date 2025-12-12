namespace CompanyRatingFrontend.Data;

public record TokenPayload
{
    public required string Email { get; init; }

    public required AppRole Role { get; init; }

    public required DateTime Expiration { get; init; }
}