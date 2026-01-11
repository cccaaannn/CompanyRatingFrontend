using System.Net.Http.Headers;
using CompanyRatingFrontend.Managers;

namespace CompanyRatingFrontend.Interceptors;

public class AuthenticationHeaderInterceptor(AccessTokenManager accessTokenManager) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (accessTokenManager.IsAuthenticated && !string.IsNullOrEmpty(accessTokenManager.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenManager.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}