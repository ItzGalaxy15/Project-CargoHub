using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json;


namespace SupplierUnitTest;

// public abstract class Base
// {
//     public string GetTimeStamp(){
//         var cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
//         string cetTimeString = cetTime.ToString("s", CultureInfo.InvariantCulture).Replace('T', ' ');
//         return cetTimeString;
//     }

//     [JsonPropertyName("created_at")]
//     public required string CreatedAt { get; set; }

//     [JsonPropertyName("updated_at")]
//     public required string UpdatedAt { get; set; }
    
// }
// public class Supplier : Base
// {

//     [JsonPropertyName("id")]
//     public required int Id { get; set; }

//     [JsonPropertyName("code")]
//     public required string Code { get; set; }

//     [JsonPropertyName("name")]
//     public required string Name { get; set; }

//     [JsonPropertyName("address")]
//     public required string Address { get; set; }

//     [JsonPropertyName("address_extra")]
//     public required string AddressExtra { get; set; }

//     [JsonPropertyName("city")]
//     public required string City { get; set; }

//     [JsonPropertyName("zip_code")]
//     public required string ZipCode { get; set; }

//     [JsonPropertyName("province")]
//     public required string Province { get; set; }

//     [JsonPropertyName("country")]
//     public required string Country { get; set; }

//     [JsonPropertyName("contact_name")]
//     public required string ContactName { get; set; }

//     [JsonPropertyName("phonenumber")]
//     public required string Phonenumber { get; set; }

//     [JsonPropertyName("reference")]
//     public required string Reference { get; set; }
    
// }


[TestClass]
public class SupplierUnitTest
{
    [TestMethod]
    public void SerializeSupplierToJson()
    {
        // Arrange
        var supplier = new Supplier
        {
            Id = 1,
            Code = "SUP0001",
            Name = "Lee, Parks and Johnson",
            Address = "5989 Sullivan Drives",
            AddressExtra = "Apt. 996",
            City = "Port Anitaburgh",
            ZipCode = "91688",
            Province = "Illinois",
            Country = "Czech Republic",
            ContactName = "Toni Barnett",
            Phonenumber = "363.541.7282x36825",
            Reference = "LPaJ-SUP0001",
            CreatedAt = "1971-10-20 18:06:17",
            UpdatedAt = "1985-06-08 00:13:46"
        };

        // Act
        string json = JsonSerializer.Serialize(supplier);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToSupplier()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""code"": ""SUP0001"",
            ""name"": ""Lee, Parks and Johnson"",
            ""address"": ""5989 Sullivan Drives"",
            ""address_extra"": ""Apt. 996"",
            ""city"": ""Port Anitaburgh"",
            ""zip_code"": ""91688"",
            ""province"": ""Illinois"",
            ""country"": ""Czech Republic"",
            ""contact_name"": ""Toni Barnett"",
            ""phonenumber"": ""363.541.7282x36825"",
            ""reference"": ""LPaJ-SUP0001"",
            ""created_at"": ""1971-10-20 18:06:17"",
            ""updated_at"": ""1985-06-08 00:13:46""
        }";

        // Act
        var supplier = JsonSerializer.Deserialize<Supplier>(json);

        // Assert
        Assert.IsNotNull(supplier);
        Assert.AreEqual(1, supplier.Id);
        Assert.AreEqual("SUP0001", supplier.Code);
        Assert.AreEqual("Lee, Parks and Johnson", supplier.Name);
        Assert.AreEqual("Port Anitaburgh", supplier.City);
    }
}