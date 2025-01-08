using System.Text.Json;

// namespace InventoryUnitTest;

[TestClass]
public class InventoryProviderTests
{
    private InventoryProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Inventory>
        {
            new Inventory
            {
                Id = 1,
                ItemId = "P000001",
                Description = "Face-to-face clear-thinking complexity",
                ItemReference = "sjQ23408K",
                Locations = new List<int> { 3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817 },
                TotalOnHand = 262,
                TotalExpected = 0,
                TotalOrdered = 80,
                TotalAllocated = 41,
                TotalAvailable = 141,
                CreatedAt = "2015-02-19 16:08:24",
                UpdatedAt = "2015-09-26 06:37:56"
            },
            new Inventory
            {
                Id = 2,
                ItemId = "P000002",
                Description = "Focused transitional alliance",
                ItemReference = "nyg48736S",
                Locations = new List<int> { 19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717 },
                TotalOnHand = 194,
                TotalExpected = 0,
                TotalOrdered = 139,
                TotalAllocated = 0,
                TotalAvailable = 55,
                CreatedAt = "2020-05-31 16:00:08",
                UpdatedAt = "2020-11-08 12:49:21"
            }
        };
        _provider = new InventoryProvider(mockData);
    }

    [TestMethod]
    public void CheckGetInventory()
    {
        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddInventory()
    {
        var newInventory = new Inventory
        {
            Id = 3,
            ItemId = "P000003",
            Description = "New Inventory Item",
            ItemReference = "newRef123",
            Locations = new List<int> { 12345, 67890 },
            TotalOnHand = 100,
            TotalExpected = 50,
            TotalOrdered = 30,
            TotalAllocated = 20,
            TotalAvailable = 80,
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newInventory);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteInventory()
    {
        var newInventory = new Inventory
        {
            Id = 3,
            ItemId = "P000003",
            Description = "New Inventory Item",
            ItemReference = "newRef123",
            Locations = new List<int> { 12345, 67890 },
            TotalOnHand = 100,
            TotalExpected = 50,
            TotalOrdered = 30,
            TotalAllocated = 20,
            TotalAvailable = 80,
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newInventory);

        Assert.AreEqual(3, _provider?.Get().Length);

        _provider?.Delete(newInventory);

        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateInventory()
    {
        var updatedInventory = new Inventory
        {
            Id = 1,
            ItemId = "P000001",
            Description = "Updated Inventory Item",
            ItemReference = "updatedRef123",
            Locations = new List<int> { 3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817 },
            TotalOnHand = 300,
            TotalExpected = 100,
            TotalOrdered = 50,
            TotalAllocated = 25,
            TotalAvailable = 275,
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Update(updatedInventory, 1);

        var inventories = _provider?.Get();

        Assert.AreEqual(1, inventories![0].Id);
        Assert.AreEqual("P000001", inventories[0].ItemId);
        Assert.AreEqual("Updated Inventory Item", inventories[0].Description);
    }
}


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
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""item_id"":""P000001""");
        StringAssert.Contains(json, @"""description"":""Face-to-face clear-thinking complexity""");
        StringAssert.Contains(json, @"""item_reference"":""sjQ23408K""");
        StringAssert.Contains(json, @"""total_on_hand"":262");
        StringAssert.Contains(json, @"""total_expected"":0");
        StringAssert.Contains(json, @"""total_ordered"":80");
        StringAssert.Contains(json, @"""total_allocated"":41");
        StringAssert.Contains(json, @"""total_available"":141");
        StringAssert.Contains(json, @"""created_at"":""2015-02-19 16:08:24""");
        StringAssert.Contains(json, @"""updated_at"":""2015-09-26 06:37:56""");

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
        Assert.AreEqual("sjQ23408K", inventory.ItemReference);
        Assert.AreEqual(8, inventory.Locations.Count);
        Assert.AreEqual(262, inventory.TotalOnHand);
        Assert.AreEqual(0, inventory.TotalExpected);
        Assert.AreEqual(80, inventory.TotalOrdered);
        Assert.AreEqual(41, inventory.TotalAllocated);
        Assert.AreEqual(141, inventory.TotalAvailable);

        // DateTime format checks
        bool isValidCreatedAt = DateTime.TryParseExact(inventory.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);
        bool isValidUpdatedAt = DateTime.TryParseExact(inventory.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);

        Assert.IsTrue(isValidCreatedAt, "CreatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
        Assert.IsTrue(isValidUpdatedAt, "UpdatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
    }
}