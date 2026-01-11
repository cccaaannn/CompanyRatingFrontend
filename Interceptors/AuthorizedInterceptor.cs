using System.Net;
using CompanyRatingFrontend.Managers;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Interceptors;

public class AuthorizedInterceptor(
    NavigationManager navigationManager,
    IServiceProvider serviceProvider
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            // Resolve AuthContext here to break the circular dependency.
            var authContext = serviceProvider.GetRequiredService<AccessTokenManager>();
            await authContext.ClearTokenContextAsync();
            navigationManager.NavigateTo("/auth/login");

            throw new TaskCanceledException();
        }

        return response;
    }
}