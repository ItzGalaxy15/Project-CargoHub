using System.Text.Json;

[TestClass]
public class ItemLineProviderTests
{
    private ItemLineProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<ItemLine>
        {
            new ItemLine
            {
                Id = 0,
                Name = "Laptop",
                Description = "High-end gaming laptop",
                CreatedAt = "2021-01-15 10:00:00",
                UpdatedAt = "2021-06-20 12:00:00"
            },
            new ItemLine
            {
                Id = 1,
                Name = "Chair",
                Description = "Ergonomic office chair",
                CreatedAt = "2020-03-22 09:30:00",
                UpdatedAt = "2021-05-18 11:45:00"
            }
        };
        _provider = new ItemLineProvider(mockData);
    }

    [TestMethod]
    public void CheckGetItemLine()
    {
        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddItemLine()
    {
        var newItemLine = new ItemLine
        {
            Id = 2,
            Name = "Notebook",
            Description = "Spiral-bound notebook",
            CreatedAt = "2022-01-01 00:00:00",
            UpdatedAt = "2022-01-01 00:00:00"
        };

        _provider?.Add(newItemLine);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteItemLine()
    {
        var newItemLine = new ItemLine
        {
            Id = 2,
            Name = "Notebook",
            Description = "Spiral-bound notebook",
            CreatedAt = "2022-01-01 00:00:00",
            UpdatedAt = "2022-01-01 00:00:00"
        };

        _provider?.Add(newItemLine);

        Assert.AreEqual(3, _provider?.Get().Length);

        _provider?.Delete(newItemLine);

        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateItemLine()
    {
        var updatedItemLine = new ItemLine
        {
            Id = 0,
            Name = "Updated Laptop",
            Description = "Updated high-end gaming laptop",
            CreatedAt = "2021-01-15 10:00:00",
            UpdatedAt = "2022-01-01 00:00:00"
        };

        _provider?.Update(0, updatedItemLine);

        var itemLines = _provider?.Get();

        Assert.AreEqual(0, itemLines![0].Id);
        Assert.AreEqual("Updated Laptop", itemLines[0].Name);
        Assert.AreEqual("Updated high-end gaming laptop", itemLines[0].Description);
    }
}