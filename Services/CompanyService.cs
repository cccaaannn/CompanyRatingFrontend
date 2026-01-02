using System.Net.Http.Json;
using System.Text.Json;
using CompanyRatingFrontend.Data;
using CompanyRatingFrontend.Data.Company;

namespace CompanyRatingFrontend.Services;

public class CompanyService(HttpClient http, JsonSerializerOptions jsonOptions) : IAuthorizedService
{
    public async Task<PagedList<CompanyDto>> GetAsync(CompanyGetQuery query)
    {
        var queryString = query.ToQueryString();

        var response = await http.GetAsync($"Api/Company{queryString}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PagedList<CompanyDto>>(json, jsonOptions) ?? PagedList<CompanyDto>.Empty;
    }

    public async Task<CompanyDetailDto> GetAsync(Guid id)
    {
        var response = await http.GetAsync($"Api/Company/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyDetailDto>(json, jsonOptions) ??
               throw new Exception("Failed to deserialize company data.");
    }

    public async Task<CompanyDto> AddAsync(CompanyAddRequest request)
    {
        var jsonContent = JsonContent.Create(request, options: jsonOptions);
        var response = await http.PostAsync("Api/Company", jsonContent);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyDto>(json, jsonOptions) ??
               throw new Exception("Failed to deserialize added company data.");
    }

    public async Task<CompanyDto> UpdateAsync(Guid id, CompanyUpdateRequest request)
    {
        var jsonContent = JsonContent.Create(request, options: jsonOptions);
        var response = await http.PutAsync($"Api/Company/{id}", jsonContent);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyDto>(json, jsonOptions) ??
               throw new Exception("Failed to deserialize updated company data.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await http.DeleteAsync($"Api/Company/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<CompanyDto> RateAsync(Guid id, CompanyRatingRequest request)
    {
        var jsonContent = JsonContent.Create(request, options: jsonOptions);
        var response = await http.PutAsync($"Api/Company/{id}/Rating", jsonContent);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyDto>(json, jsonOptions) ??
               throw new Exception("Failed to deserialize rated company data.");
    }

    public async Task<CompanyCommentDto> AddCommentAsync(Guid id, CommentAddRequest request)
    {
        var jsonContent = JsonContent.Create(request, options: jsonOptions);
        var response = await http.PutAsync($"Api/Company/{id}/Comment", jsonContent);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyCommentDto>(json, jsonOptions) ??
               throw new Exception("Failed to deserialize added comment data.");
    }
}