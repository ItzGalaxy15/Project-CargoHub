using System.Text.Json.Serialization;
using System.Globalization;

public abstract class Base
{
    public string GetTimeStamp()
    {
        var cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        string cetTimeString = cetTime.ToString("s", CultureInfo.InvariantCulture).Replace('T', ' ');
        return cetTimeString;
    }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required string UpdatedAt { get; set; }
}
