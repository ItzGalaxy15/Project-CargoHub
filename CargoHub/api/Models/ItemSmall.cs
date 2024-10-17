using System.Text.Json.Serialization;

public class ItemSmall
{
    [JsonPropertyName("item_id")]
    public required string ItemId { get; set; }

    [JsonPropertyName("amount")]
    public required int Amount { get; set; }
}
