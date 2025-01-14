using System.Text.Json.Serialization;

public class Transfer : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("transfer_from")]
    public required int? TransferFrom { get; set; }

    [JsonPropertyName("transfer_to")]
    public required int? TransferTo { get; set; }

    [JsonPropertyName("transfer_status")]
    public required string TransferStatus { get; set; }

    [JsonPropertyName("items")]
    public required List<ItemSmall> Items { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}