using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Components.Toast;

public partial class GenericToast
{
    [Parameter] public string? Header { get; set; }
    [Parameter] public RenderFragment? Body { get; set; }
    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    private async Task OnVisibleChanged(bool newVisibility)
    {
        if (Visible != newVisibility)
        {
            Visible = newVisibility;
            await VisibleChanged.InvokeAsync(Visible);
        }
    }
}