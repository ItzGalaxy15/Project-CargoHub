using System.Text.Json.Serialization;

public class Supplier : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("address")]
    public required string Address { get; set; }

    [JsonPropertyName("address_extra")]
    public required string AddressExtra { get; set; }

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

    [JsonPropertyName("phonenumber")]
    public required string Phonenumber { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}