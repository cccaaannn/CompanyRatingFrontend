using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Components.Error;

public partial class GenericErrorBoundary
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}