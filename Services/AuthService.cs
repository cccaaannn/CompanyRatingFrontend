using System.Net.Http.Json;
using System.Text.Json;
using CompanyRatingFrontend.Data;

namespace CompanyRatingFrontend.Services;

public class AuthService(HttpClient http, JsonSerializerOptions jsonOptions) : IUnAuthorizedService
{
    public async Task<AccessTokenDto> Register(RegisterRequest request)
    {
        var response = await http.PostAsJsonAsync("Api/Auth/Register", request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AccessTokenDto>(jsonResponse, jsonOptions) ??
               throw new Exception("Failed to deserialize access token.");
    }

    public async Task<AccessTokenDto> Login(LoginRequest request)
    {
        var response = await http.PostAsJsonAsync("Api/Auth/Login", request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AccessTokenDto>(jsonResponse, jsonOptions) ??
               throw new Exception("Failed to deserialize access token.");
    }
}