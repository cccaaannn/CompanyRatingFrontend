namespace CompanyRatingFrontend.Data;

public enum AppRole
{
    Admin,
    User
}

public static class AppRoleExtensions
{
    public static string ToDisplayString(this AppRole role) => role switch
    {
        AppRole.Admin => nameof(AppRole.Admin),
        AppRole.User => nameof(AppRole.User),
        _ => ""
    };

    public static AppRole FromString(string? role) => role switch
    {
        "Admin" => AppRole.Admin,
        "User" => AppRole.User,
        _ => throw new ArgumentException("Invalid role string", nameof(role))
    };
}