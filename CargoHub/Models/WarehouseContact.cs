using System.Text.Json.Serialization;

public class WarehouseContact
{

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
