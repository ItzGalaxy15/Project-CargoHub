using System.Text.Json;
using System.Text.Json.Serialization;

public class Client : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("address")]
    public required string Address { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("zip_code")]
    public required string ZipCode { get; set; }

    [JsonPropertyName("province")]
    public required string Province { get; set; }

    [JsonPropertyName("country")]
    public required string Country { get; set; }

    [JsonPropertyName("contact_name")]
    public required string ContactName { get; set; }

    [JsonPropertyName("contact_phone")]
    public required string ContactPhone { get; set; }

    [JsonPropertyName("contact_email")]
    public required string ContactEmail { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
