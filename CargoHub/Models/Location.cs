using System.Text.Json.Serialization;

public class Location : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("warehouse_id")]
    public required int WarehouseId { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
