using System.Text.Json.Serialization;

public class ItemType : Base
{
    [JsonIgnore]
    public override string _path { get; set; } = "item_types.json";


    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description  { get; set; }

}