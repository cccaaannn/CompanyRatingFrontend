using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Blazorise.Utilities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CompanyRatingFrontend;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.Services;
using CompanyRatingFrontend.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Blazorise
builder.Services
    .AddBlazorise()
    .AddTailwindProviders()
    .AddFontAwesomeIcons()
    .AddBlazoredLocalStorage();

// Validation
builder.Services.AddScoped<IValidationMessageLocalizerAttributeFinder, ValidationMessageLocalizerAttributeFinder>();

// JSON Options
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonOptions.Converters.Add(new JsonStringEnumConverter());
builder.Services.AddSingleton(jsonOptions);

// Public and Private HttpClients
builder.Services.AddApiServices(builder.Configuration);

// Managers
builder.Services.AddSingleton<AccessTokenManager>();
builder.Services.AddSingleton<ThemeManager>();

// ViewModels
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<CompanyViewModel>();
builder.Services.AddScoped<CompaniesViewModel>();
builder.Services.AddScoped<CreateCompanyViewModel>();

var host = builder.Build();

var authContext = host.Services.GetRequiredService<AccessTokenManager>();
await authContext.InitializeAsync();

var themeManager = host.Services.GetRequiredService<ThemeManager>();
await themeManager.InitializeAsync();

await host.RunAsync();