using System.Text.Json;

//namespace ItemTypeUnitTest;

[TestClass]
public class ItemTypeProviderTests
{
    private ItemTypeProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<ItemType>
        {
            new ItemType { Id = 1, Name = "ItemType A", Description = "", CreatedAt = "", UpdatedAt = "" },
            new ItemType { Id = 2, Name = "ItemType B", Description = "", CreatedAt = "", UpdatedAt = "" },
            new ItemType { Id = 3, Name = "ItemType C", Description = "", CreatedAt = "", UpdatedAt = "" }
        };
        _provider = new ItemTypeProvider(mockData);
    }

    [TestMethod]
    public void CheckGetItemType()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddItemType()
    {
        var newItemType = new ItemType { Id = 4, Name = "ItemType D", Description = "", CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newItemType);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteItemType()
    {
        var newItemType = new ItemType { Id = 5, Name = "ItemType H", Description = "", CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newItemType);

        Assert.AreEqual(4, _provider?.Get().Length);
        
        _provider?.Delete(newItemType);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateItemType()
    {
        var newItemType = new ItemType { Id = 3, Name = "ItemType Abcdd", Description = "jrefefe", CreatedAt = "", UpdatedAt = "" };

        _provider?.Update(newItemType, 1);

        var ItemTypes = _provider?.Get();

        Assert.AreEqual(1, ItemTypes![0].Id);
        Assert.AreEqual("ItemType Abcdd", ItemTypes[0].Name);
    }
}

[TestClass]
public class ItemTypeModelTest
{
    [TestMethod]
    public void SerializeItemTypeToJson()
    {
        // Arrange
        var newItemType = new ItemType 
        { 
            Id = 1, 
            Name = "ItemType A", 
            Description = "", 
            CreatedAt = "", 
            UpdatedAt = ""
        };

        // Act
        string json = JsonSerializer.Serialize(newItemType);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToItemType()
    {
        // Arrange
        string json = @"
        { 
            ""id"": 1, 
            ""name"": ""ItemType A"",
            ""description"": """",
            ""created_at"": """", 
            ""updated_at"": """" 
        }";

        // Act
        var ItemType = JsonSerializer.Deserialize<ItemType>(json);

        // Assert
        Assert.IsNotNull(ItemType);
        Assert.AreEqual(1, ItemType.Id);
        Assert.AreEqual("ItemType A", ItemType.Name);
    }
}