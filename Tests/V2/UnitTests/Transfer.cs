using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        Assert.AreEqual(3, _provider?.Get().Length);
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
