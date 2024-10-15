using System.Text.Json.Serialization;
using System.Globalization;

public abstract class Base 
{

    public string GetTimeStamp(){
        return DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z";
    }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required string UpdatedAt { get; set; }
    
}
