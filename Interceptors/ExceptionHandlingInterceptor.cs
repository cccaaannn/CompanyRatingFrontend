namespace CompanyRatingFrontend.Interceptors;

public class ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred during HTTP request");
            throw;
        }
    }
}