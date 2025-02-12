using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

[TestClass]
public class TransferProviderTests
{
    private TransferProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Transfer>
        {
            new Transfer { Id = 1, Reference = "REF001", TransferFrom = 1, TransferTo = 2, TransferStatus = "Pending", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" },
            new Transfer { Id = 2, Reference = "REF002", TransferFrom = 2, TransferTo = 3, TransferStatus = "Completed", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" },
            new Transfer { Id = 3, Reference = "REF003", TransferFrom = 3, TransferTo = 4, TransferStatus = "InProgress", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" }
        };
        _provider = new TransferProvider(mockData);
    }

    [TestMethod]
    public void CheckGetTransfer()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddTransfer()
    {
        var newTransfer = new Transfer { Id = 4, Reference = "REF004", TransferFrom = 4, TransferTo = 5, TransferStatus = "Pending", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newTransfer);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteTransfer()
    {
        var newTransfer = new Transfer { Id = 5, Reference = "REF005", TransferFrom = 5, TransferTo = 6, TransferStatus = "Pending", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newTransfer);

        Assert.AreEqual(4, _provider?.Get().Length);

        _provider?.Delete(newTransfer);

        var transfers = _provider?.Get();
        Assert.AreEqual(4, transfers?.Length);
        Assert.IsTrue(transfers?.First(t => t.Id == 5).IsDeleted);
    }

    [TestMethod]
    public void CheckUpdateTransfer()
    {
        var updatedTransfer = new Transfer { Id = 1, Reference = "REF001-UPDATED", TransferFrom = 1, TransferTo = 2, TransferStatus = "Completed", Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Update(updatedTransfer, 1);

        var transfers = _provider?.Get();

        Assert.AreEqual(1, transfers![0].Id);
        Assert.AreEqual("REF001-UPDATED", transfers[0].Reference);
        Assert.AreEqual("Completed", transfers[0].TransferStatus);
    }
}

[TestClass]
public class TransferUnitTest
{
    [TestMethod]
    public void SerializeTransferToJson()
    {
        // Arrange
        var transfer = new Transfer
        {
            Id = 1,
            Reference = "REF001",
            TransferFrom = 1,
            TransferTo = 2,
            TransferStatus = "Pending",
            Items = new List<ItemSmall>(),
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        // Act
        string json = JsonSerializer.Serialize(transfer);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""reference"":""REF001""");
        StringAssert.Contains(json, @"""transfer_from"":1");
        StringAssert.Contains(json, @"""transfer_to"":2");
        StringAssert.Contains(json, @"""transfer_status"":""Pending""");
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
    public void DeserializeJsonToTransfer()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""reference"": ""REF001"",
            ""transfer_from"": 1,
            ""transfer_to"": 2,
            ""transfer_status"": ""Pending"",
            ""items"": [],
            ""created_at"": ""2023-01-01 00:00:00"",
            ""updated_at"": ""2023-01-01 00:00:00""
        }";

        // Act
        var transfer = JsonSerializer.Deserialize<Transfer>(json);

        // Assert
        Assert.IsNotNull(transfer);
        Assert.AreEqual(1, transfer.Id);
        Assert.AreEqual("REF001", transfer.Reference);
        Assert.AreEqual(1, transfer.TransferFrom);
        Assert.AreEqual(2, transfer.TransferTo);
        Assert.AreEqual("Pending", transfer.TransferStatus);
        Assert.AreEqual("2023-01-01 00:00:00", transfer.CreatedAt);
        Assert.AreEqual("2023-01-01 00:00:00", transfer.UpdatedAt);
    }
}
