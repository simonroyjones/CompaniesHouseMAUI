using System.Text.Json.Serialization;

namespace CompanyLookupMAUI.Models;

public class CompanyProfile
{
    [JsonPropertyName("company_name")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("company_number")]
    public string? CompanyNumber { get; set; }

    [JsonPropertyName("company_status")]
    public string? CompanyStatus { get; set; }

    [JsonPropertyName("type")]
    public string? CompanyType { get; set; }

    [JsonPropertyName("date_of_creation")]
    public string? DateOfCreation { get; set; }

    [JsonPropertyName("registered_office_address")]
    public RegisteredOfficeAddress? RegisteredOfficeAddress { get; set; }
}

public class RegisteredOfficeAddress
{
    [JsonPropertyName("address_line_1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("address_line_2")]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("postal_code")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    public override string ToString()
    {
        var parts = new[]
        {
            AddressLine1,
            AddressLine2,
            Locality,
            Region,
            PostalCode,
            Country
        };

        return string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}