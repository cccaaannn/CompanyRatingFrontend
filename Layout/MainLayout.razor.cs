using CompanyRatingFrontend.Managers;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Layout;

public partial class MainLayout
{
    [Inject] private ThemeManager ThemeManager { get; set; } = null!;

    protected override void OnInitialized()
    {
        ThemeManager.ThemeChanged += OnThemeChanged;
        base.OnInitialized();
    }

    private void OnThemeChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ThemeManager.ThemeChanged -= OnThemeChanged;
    }
}