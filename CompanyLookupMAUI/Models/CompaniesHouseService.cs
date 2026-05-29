using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CompanyLookupMAUI.Models;

namespace CompanyLookupMAUI.Services;

public class CompaniesHouseService
{
    private readonly HttpClient _httpClient;

    private const string BaseUrl = "https://api.company-information.service.gov.uk";

    // Replace this with your real Companies House API key.
    // For a proper production app, do not hard-code this in the app.
    private const string ApiKey = "1a469325-a15d-49c6-a80d-b289ffda0e2e";

    public CompaniesHouseService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ApiKey}:"));
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", authValue);
    }

    public async Task<CompanyProfile?> GetCompanyByNumberAsync(string companyNumber)
    {
        if (string.IsNullOrWhiteSpace(companyNumber))
            return null;

        var cleanCompanyNumber = companyNumber.Trim();

        var response = await _httpClient.GetAsync($"/company/{Uri.EscapeDataString(cleanCompanyNumber)}");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Companies House returned: {(int)response.StatusCode} {response.ReasonPhrase}");

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<CompanyProfile>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<List<CompanySearchItem>> SearchCompaniesByNameAsync(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return new List<CompanySearchItem>();

        var cleanCompanyName = companyName.Trim();

        var response = await _httpClient.GetAsync(
            $"/search/companies?q={Uri.EscapeDataString(cleanCompanyName)}");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Companies House returned: {(int)response.StatusCode} {response.ReasonPhrase}");

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<CompanySearchResult>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Items ?? new List<CompanySearchItem>();
    }
}