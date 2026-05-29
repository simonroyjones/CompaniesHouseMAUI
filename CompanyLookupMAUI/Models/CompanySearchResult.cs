using System.Text.Json.Serialization;

namespace CompanyLookupMAUI.Models;

public class CompanySearchResult
{
    [JsonPropertyName("items")]
    public List<CompanySearchItem> Items { get; set; } = new();
}

public class CompanySearchItem
{
    [JsonPropertyName("title")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("company_number")]
    public string? CompanyNumber { get; set; }

    [JsonPropertyName("company_status")]
    public string? CompanyStatus { get; set; }

    [JsonPropertyName("company_type")]
    public string? CompanyType { get; set; }

    [JsonPropertyName("date_of_creation")]
    public string? DateOfCreation { get; set; }

    [JsonPropertyName("address_snippet")]
    public string? AddressSnippet { get; set; }

    public string DisplayText =>
        $"{CompanyName} - {CompanyNumber} - {CompanyStatus}";
}