using apiV1.Services;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;


[TestClass]
public class LocationProviderTests
{
    private LocationProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Location>
        {
            new Location { Id = 1, WarehouseId = 1, Code = "A.2.e21e21e21e", Name = "Row: A, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "2014-06-21 17:46:19", UpdatedAt = "2014-06-22 17:46:19"},
            new Location { Id = 2, WarehouseId = 2, Code = "A.2.21fefefeff", Name = "Row: B, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "2014-06-22 17:46:19", UpdatedAt = "2014-06-23 17:46:19"},
            new Location { Id = 3, WarehouseId = 3, Code = "A.2.1231312321", Name = "Row: C, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "2014-06-23 17:46:19", UpdatedAt = "2014-06-24 17:46:19"}

        };
        _provider = new LocationProvider(mockData);
    }

    [TestMethod]
    public void CheckGetLocation()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddLocation()
    {
        var newLocation = new Location { Id = 4, WarehouseId = 4, Code = "A.2.1231312321", Name = "Row: C, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19"};

        _provider?.Add(newLocation);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteLocation()
    {
        var newLocation = new Location { Id = 5, WarehouseId = 5, Code = "A.2.e21e21e21e", Name = "Row: A, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19"};
        _provider?.Add(newLocation);
        
        _provider?.Delete(newLocation);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateLocation()
    {
        var newLocation = new Location { Id = 1, WarehouseId = 1, Code = "A.2",
                                            Name = "Row: A, R3123123123213231ack: 2, Shelf: 0",
                                            CreatedAt = "2014-06-24 17:46:19",
                                            UpdatedAt = "2014-06-25 17:46:19"};

        _provider?.Update(newLocation, 1);

        var Locations = _provider?.Get();

        Assert.AreEqual(1, Locations![0].Id);
        Assert.AreEqual("Row: A, R3123123123213231ack: 2, Shelf: 0", Locations[0].Name);
        Assert.AreEqual("A.2", Locations[0].Code);

        Assert.IsFalse(string.IsNullOrEmpty(Locations[0].CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(Locations[0].UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(Locations[0].UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(Locations[0].CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");
    }
}

[TestClass]
public class LocationModelTest
{
    [TestMethod]
    public void SerializeLocationToJson()
    {
        // Arrange
        var newLocation = new Location 
        { 
            Id = 1, 
            WarehouseId = 1, 
            Code = "A.2.e21e21e21e", 
            Name = "Row: A, R3123123123213231ack: 2, Shelf: 0", 
            CreatedAt = "2014-06-24 17:46:19", 
            UpdatedAt = "2014-06-25 17:46:19"
        };

        // Act
        string json = JsonSerializer.Serialize(newLocation);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""warehouse_id"":1");
        StringAssert.Contains(json, @"""code"":""A.2.e21e21e21e""");
        StringAssert.Contains(json, @"""name"":""Row: A, R3123123123213231ack: 2, Shelf: 0""");
        StringAssert.Contains(json, @"""created_at"":""2014-06-24 17:46:19""");
        StringAssert.Contains(json, @"""updated_at"":""2014-06-25 17:46:19""");

        // DateTime format checks
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        string createdAt = root.GetProperty("created_at").GetString()!;
        string updatedAt = root.GetProperty("updated_at").GetString()!;

        bool isValidCreatedAt = DateTime.TryParseExact(createdAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);
        bool isValidUpdatedAt = DateTime.TryParseExact(updatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);

        Assert.IsTrue(isValidCreatedAt, "CreatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
        Assert.IsTrue(isValidUpdatedAt, "UpdatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
    }

    [TestMethod]
    public void DeserializeJsonToLocation()
    {
        // Arrange
        string json = @"
        { 
            ""id"": 1, 
            ""warehouse_id"": 1, 
            ""code"": ""A.2.e21e21e21e"", 
            ""name"": ""Row: A, R3123123123213231ack: 2, Shelf: 0"", 
            ""created_at"": ""2014-06-24 17:46:19"", 
            ""updated_at"": ""2014-06-25 17:46:19"" 
        }";

        // Act
        var Location = JsonSerializer.Deserialize<Location>(json);

        // Assert
        Assert.IsNotNull(Location);
        Assert.AreEqual(1, Location.Id);
        Assert.AreEqual(1, Location.WarehouseId);
        Assert.AreEqual("A.2.e21e21e21e", Location.Code);
        Assert.AreEqual("Row: A, R3123123123213231ack: 2, Shelf: 0", Location.Name);

        Assert.IsFalse(string.IsNullOrEmpty(Location.CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(Location.UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(Location.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(Location.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");
    }
}
