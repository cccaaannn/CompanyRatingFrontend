using Blazored.LocalStorage;
using Blazorise;

namespace CompanyRatingFrontend.Managers;

public class ThemeManager(IServiceProvider serviceProvider)
{
    private const string ThemePreferenceKey = "CompanyAppTheme";
    private const string LightThemeValue = "light";
    private const string DarkThemeValue = "dark";

    #region Themes

    private readonly Theme _lightTheme = new()
    {
        ColorOptions = new ThemeColorOptions
        {
            Primary = "#3B82F6",
            Secondary = "#64748B",
            Success = "#22C55E",
            Danger = "#EF4444",
            Warning = "#F59E0B",
            Info = "#0EA5E9",
            Light = "#F8FAFC",
            Dark = "#0F172A",
        },
        BodyOptions = new ThemeBodyOptions
        {
            BackgroundColor = "#FFFFFF",
            TextColor = "#0F172A"
        },
    };

    private readonly Theme _darkTheme = new()
    {
        ColorOptions = new ThemeColorOptions
        {
            Primary = "#60A5FA",
            Secondary = "#94A3B8",
            Success = "#4ADE80",
            Danger = "#F87171",
            Warning = "#FBBF24",
            Info = "#38BDF8",
            Light = "#CBD5F5",
            Dark = "#020617",
        },
        BodyOptions = new ThemeBodyOptions
        {
            BackgroundColor = "#0B1120",
            TextColor = "#E2E8F0"
        }
    };

    #endregion

    public bool IsDark { get; private set; }

    public event Action? ThemeChanged;

    public Theme ActiveTheme => IsDark ? _darkTheme : _lightTheme;

    public async Task InitializeAsync()
    {
        var themePreference = await GetThemePreferenceAsync();
        IsDark = themePreference == DarkThemeValue;
    }

    public async Task OnThemeChanged()
    {
        IsDark = !IsDark;
        ThemeChanged?.Invoke();

        var storedThemeValue = IsDark ? DarkThemeValue : LightThemeValue;
        await SetThemePreferenceAsync(storedThemeValue);
    }

    private async Task SetThemePreferenceAsync(string themeValue)
    {
        var localStorage = GetLocalStorageService();
        await localStorage.SetItemAsStringAsync(ThemePreferenceKey, themeValue);
    }

    private async Task<string> GetThemePreferenceAsync()
    {
        var localStorage = GetLocalStorageService();
        return await localStorage.GetItemAsStringAsync(ThemePreferenceKey) ?? LightThemeValue;
    }

    private ILocalStorageService GetLocalStorageService()
    {
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
    }
}