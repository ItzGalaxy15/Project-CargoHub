using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json;

namespace InventoryUnitTest;


[TestClass]
public class InventoryUnitTest
{
    [TestMethod]
    public void SerializeInventoryToJson()
    {
        // Arrange
        var inventory = new Inventory
        {
            Id = 1,
            ItemId = "P000001",
            Description = "Face-to-face clear-thinking complexity",
            ItemReference = "sjQ23408K",
            Locations = [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817],
            TotalOnHand = 262,
            TotalExpected = 0,
            TotalOrdered = 80,
            TotalAllocated = 41,
            TotalAvailable = 141,
            CreatedAt = "2015-02-19 16:08:24",
            UpdatedAt = "2015-09-26 06:37:56",
        };

        // Act
        string json = JsonSerializer.Serialize(inventory);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToInventory()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""item_id"": ""P000001"",
            ""description"": ""Face-to-face clear-thinking complexity"",
            ""item_reference"": ""sjQ23408K"",
            ""locations"": [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817],
            ""total_on_hand"": 262,
            ""total_expected"": 0,
            ""total_ordered"": 80,
            ""total_allocated"": 41,
            ""total_available"": 141,
            ""created_at"": ""2015-02-19 16:08:24"",
            ""updated_at"": ""2015-09-26 06:37:56""
        }";

        // Act
        var inventory = JsonSerializer.Deserialize<Inventory>(json);

        // Assert
        Assert.IsNotNull(inventory);
        Assert.AreEqual(1, inventory.Id);
        Assert.AreEqual("P000001", inventory.ItemId);
        Assert.AreEqual("Face-to-face clear-thinking complexity", inventory.Description);
        Assert.AreEqual(8, inventory.Locations.Count);
        Assert.AreEqual(262, inventory.TotalOnHand);
    }
}
