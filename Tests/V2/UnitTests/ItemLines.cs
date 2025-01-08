using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ItemLineUnitTest
{
    [TestMethod]
    public void SerializeItemLineToJson()
    {
        // Arrange
        var itemLine = new ItemLine
        {
            Id = 1,
            Name = "Item 1",
            Description = "Description of Item 1",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        // Act
        string json = JsonSerializer.Serialize(itemLine);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""name"":""Item 1""");
        StringAssert.Contains(json, @"""description"":""Description of Item 1""");
        StringAssert.Contains(json, @"""created_at"":""2023-01-01 00:00:00""");
        StringAssert.Contains(json, @"""updated_at"":""2023-01-01 00:00:00""");

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
    public void DeserializeJsonToItemLine()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""name"": ""Item 1"",
            ""description"": ""Description of Item 1"",
            ""created_at"": ""2023-01-01 00:00:00"",
            ""updated_at"": ""2023-01-01 00:00:00""
        }";

        // Act
        var itemLine = JsonSerializer.Deserialize<ItemLine>(json);

        // Assert
        Assert.IsNotNull(itemLine);
        Assert.AreEqual(1, itemLine.Id);
        Assert.AreEqual("Item 1", itemLine.Name);
        Assert.AreEqual("Description of Item 1", itemLine.Description);
    }
}

[TestClass]
public class ItemLineProviderTests
{
    private ItemLineProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<ItemLine>
        {
            new ItemLine { Id = 1, Name = "Item 1", Description = "Description of Item 1", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" },
            new ItemLine { Id = 2, Name = "Item 2", Description = "Description of Item 2", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" },
            new ItemLine { Id = 3, Name = "Item 3", Description = "Description of Item 3", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" }
        };
        _provider = new ItemLineProvider(mockData);
    }

    [TestMethod]
    public void CheckGetItemLine()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddItemLine()
    {
        var newItemLine = new ItemLine { Id = 4, Name = "Item 4", Description = "Description of Item 4", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" };

        _provider?.Add(newItemLine);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteItemLine()
    {
        var newItemLine = new ItemLine { Id = 4, Name = "Item 4", Description = "Description of Item 4", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" };

        _provider?.Add(newItemLine);

        Assert.AreEqual(4, _provider?.Get().Length);

        _provider?.Delete(newItemLine);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateItemLine()
    {
        var updatedItemLine = new ItemLine { Id = 1, Name = "Item 1 Updated", Description = "Description of Item 1 Updated", CreatedAt = "2023-01-01 00:00:00", UpdatedAt = "2023-01-01 00:00:00" };

        _provider?.Update(1, updatedItemLine);

        var itemLines = _provider?.Get();

        Assert.AreEqual(1, itemLines![0].Id);
        Assert.AreEqual("Item 1 Updated", itemLines[0].Name);
        Assert.AreEqual("Description of Item 1 Updated", itemLines[0].Description);
        Assert.AreEqual("2023-01-01 00:00:00", itemLines[0].CreatedAt);
        Assert.AreEqual("2023-01-01 00:00:00", itemLines[0].UpdatedAt);
    }
}