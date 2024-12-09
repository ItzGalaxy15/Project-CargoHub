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