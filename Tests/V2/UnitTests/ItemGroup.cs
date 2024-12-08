using System.Text.Json;

// namespace ItemGroupUnitTest;

[TestClass]
public class ItemGroupProviderTests
{
    private ItemGroupProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<ItemGroup>
        {
            new ItemGroup
            {
                Id = 0,
                Name = "Electronics",
                Description = "",
                CreatedAt = "1998-05-15 19:52:53",
                UpdatedAt = "2000-11-20 08:37:56"
            },
            new ItemGroup
            {
                Id = 1,
                Name = "Furniture",
                Description = "",
                CreatedAt = "2019-09-22 15:51:07",
                UpdatedAt = "2022-05-18 13:49:28"
            }
        };
        _provider = new ItemGroupProvider(mockData);
    }

    [TestMethod]
    public void CheckGetItemGroup()
    {
        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddItemGroup()
    {
        var newItemGroup = new ItemGroup
        {
            Id = 2,
            Name = "Stationery",
            Description = "",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newItemGroup);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteItemGroup()
    {
        var newItemGroup = new ItemGroup
        {
            Id = 2,
            Name = "Stationery",
            Description = "",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newItemGroup);

        Assert.AreEqual(3, _provider?.Get().Length);

        _provider?.Delete(newItemGroup);

        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateItemGroup()
    {
        var updatedItemGroup = new ItemGroup
        {
            Id = 0,
            Name = "Updated Electronics",
            Description = "Updated Description",
            CreatedAt = "1998-05-15 19:52:53",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Update(updatedItemGroup, 0);

        var itemGroups = _provider?.Get();

        Assert.AreEqual(0, itemGroups![0].Id);
        Assert.AreEqual("Updated Electronics", itemGroups[0].Name);
        Assert.AreEqual("Updated Description", itemGroups[0].Description);
    }
}



[TestClass]
public class ItemGroupUnitTest
{
    [TestMethod]
    public void SerializeItemGroupToJson()
    {
        // Arrange
        var itemGroup = new ItemGroup
        {
            Id = 1,
            Name = "Fem Keijzer",
            Description = "",
            CreatedAt = "2019-09-22 15:51:07",
            UpdatedAt = "2022-05-18 13:49:28",
        };

        // Act
        string json = JsonSerializer.Serialize(itemGroup);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToItemGroup()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""name"": ""Fem Keijzer"",
            ""description"": """",
            ""created_at"": ""2019-09-22 15:51:07"",
            ""updated_at"": ""2022-05-18 13:49:28""
        }";

        // Act
        var itemGroup = JsonSerializer.Deserialize<ItemGroup>(json);

        // Assert
        Assert.IsNotNull(itemGroup);
        Assert.AreEqual(1, itemGroup.Id);
        Assert.AreEqual("Fem Keijzer", itemGroup.Name);
    }
}