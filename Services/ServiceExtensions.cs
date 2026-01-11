using System.Net.Http.Headers;
using CompanyRatingFrontend.Data.Config;
using CompanyRatingFrontend.Interceptors;

namespace CompanyRatingFrontend.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<AuthorizedInterceptor>();
        services.AddScoped<ExceptionHandlingInterceptor>();
        services.AddTransient<AuthenticationHeaderInterceptor>();

        var apiSettings = configuration.GetRequiredSection(ApiSettings.SectionName).Get<ApiSettings>()!;

        services.AddHttpClient("Private", client =>
            {
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<ExceptionHandlingInterceptor>()
            .AddHttpMessageHandler<AuthenticationHeaderInterceptor>()
            .AddHttpMessageHandler<AuthorizedInterceptor>();

        services.AddHttpClient("Public", client =>
            {
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<ExceptionHandlingInterceptor>();

        services = AddApiService<IUnAuthorizedService>(services, "Public");
        services = AddApiService<IAuthorizedService>(services, "Private");

        return services;
    }

    // This method scans the assembly for implementations of TInterface and registers them with the specified HttpClient.
    // It is used to register services like IAuthorizedService and IUnAuthorizedService.
    private static IServiceCollection AddApiService<TInterface>(
        IServiceCollection services,
        string httpClientName
    ) where TInterface : class
    {
        var interfaceType = typeof(TInterface);
        var types = interfaceType.Assembly.GetTypes()
            .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface);

        foreach (var type in types)
        {
            services.AddScoped(type, sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient(httpClientName);

                // This assumes a constructor like: SomeService(HttpClient, ...)
                // It gets the other dependencies from the service provider.
                return ActivatorUtilities.CreateInstance(sp, type, client);
            });
        }

        return services;
    }
}