using System.Text.Json;
using System.Text.Json.Serialization;

public class Inventory : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("item_id")]
    public required string ItemId { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("item_reference")]
    public required string ItemReference { get; set; }

    [JsonPropertyName("locations")]
    public required List<int> Locations { get; set; }

    [JsonPropertyName("total_on_hand")]
    public required int TotalOnHand { get; set; }

    [JsonPropertyName("total_expected")]
    public required int TotalExpected { get; set; }

    [JsonPropertyName("total_ordered")]
    public required int TotalOrdered { get; set; }

    [JsonPropertyName("total_allocated")]
    public required int TotalAllocated { get; set; }

    [JsonPropertyName("total_available")]
    public required int TotalAvailable { get; set; }
}
