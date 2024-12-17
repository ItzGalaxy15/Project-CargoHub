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
            new ItemType { Id = 1, Name = "ItemType A", Description = "", CreatedAt = "2014-06-21 17:46:19", UpdatedAt = "2014-06-24 17:46:19" },
            new ItemType { Id = 2, Name = "ItemType B", Description = "", CreatedAt = "2014-06-22 17:46:19", UpdatedAt = "2014-06-25 17:46:19" },
            new ItemType { Id = 3, Name = "ItemType C", Description = "", CreatedAt = "2014-06-23 17:46:19", UpdatedAt = "2014-06-26 17:46:19" }
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
        var newItemType = new ItemType { Id = 3, Name = "ItemType Abcdd", Description = "jrefefe", CreatedAt = "2014-06-23 18:46:19", UpdatedAt = "2014-06-23 19:46:19" };

        _provider?.Update(newItemType, 1);

        var ItemTypes = _provider?.Get();

        Assert.AreEqual(1, ItemTypes![0].Id);
        Assert.AreEqual("ItemType Abcdd", ItemTypes[0].Name);
        Assert.AreEqual("jrefefe", ItemTypes[0].Description);

        Assert.IsFalse(string.IsNullOrEmpty(ItemTypes[0].CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(ItemTypes[0].UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(ItemTypes[0].UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(ItemTypes[0].CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");
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
            Description = "jeff", 
            CreatedAt = "2014-06-23 17:46:19", 
            UpdatedAt = "2014-06-24 17:46:19"
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
            ""description"": ""jeff"",
            ""created_at"": ""2014-06-23 17:46:19"", 
            ""updated_at"": ""2014-06-24 17:46:19"" 
        }";

        // Act
        var ItemType = JsonSerializer.Deserialize<ItemType>(json);

        // Assert
        Assert.IsNotNull(ItemType);
        Assert.AreEqual(1, ItemType.Id);
        Assert.AreEqual("ItemType A", ItemType.Name);
        Assert.AreEqual("jeff", ItemType.Description);

        Assert.IsFalse(string.IsNullOrEmpty(ItemType.CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(ItemType.UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(ItemType.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(ItemType.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

    }
}