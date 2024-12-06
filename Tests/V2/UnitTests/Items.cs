using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ItemProviderTests
{
    private ItemProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Item>
        {
            new Item { Uid = "1", Code = "Code1", Description = "Description1", ShortDescription = "ShortDesc1", UpcCode = "UPC1", ModelNumber = "Model1", CommodityCode = "Comm1", ItemLine = 1, ItemGroup = 1, ItemType = 1, UnitPurchaseQuantity = 10, UnitOrderQuantity = 20, PackOrderQuantity = 30, SupplierId = 1, SupplierCode = "SupCode1", SupplierPartNumber = "SupPart1", CreatedAt = "", UpdatedAt = "" },
            new Item { Uid = "2", Code = "Code2", Description = "Description2", ShortDescription = "ShortDesc2", UpcCode = "UPC2", ModelNumber = "Model2", CommodityCode = "Comm2", ItemLine = 2, ItemGroup = 2, ItemType = 2, UnitPurchaseQuantity = 15, UnitOrderQuantity = 25, PackOrderQuantity = 35, SupplierId = 2, SupplierCode = "SupCode2", SupplierPartNumber = "SupPart2", CreatedAt = "", UpdatedAt = "" },
            new Item { Uid = "3", Code = "Code3", Description = "Description3", ShortDescription = "ShortDesc3", UpcCode = "UPC3", ModelNumber = "Model3", CommodityCode = "Comm3", ItemLine = 3, ItemGroup = 3, ItemType = 3, UnitPurchaseQuantity = 20, UnitOrderQuantity = 30, PackOrderQuantity = 40, SupplierId = 3, SupplierCode = "SupCode3", SupplierPartNumber = "SupPart3", CreatedAt = "", UpdatedAt = "" }
        };
        _provider = new ItemProvider(mockData);
    }

    [TestMethod]
    public void CheckGetItems()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddItem()
    {
        var newItem = new Item { Uid = "4", Code = "Code4", Description = "Description4", ShortDescription = "ShortDesc4", UpcCode = "UPC4", ModelNumber = "Model4", CommodityCode = "Comm4", ItemLine = 4, ItemGroup = 4, ItemType = 4, UnitPurchaseQuantity = 25, UnitOrderQuantity = 35, PackOrderQuantity = 45, SupplierId = 4, SupplierCode = "SupCode4", SupplierPartNumber = "SupPart4", CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newItem);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteItem()
    {
        var newItem = new Item { Uid = "5", Code = "Code5", Description = "Description5", ShortDescription = "ShortDesc5", UpcCode = "UPC5", ModelNumber = "Model5", CommodityCode = "Comm5", ItemLine = 5, ItemGroup = 5, ItemType = 5, UnitPurchaseQuantity = 30, UnitOrderQuantity = 40, PackOrderQuantity = 50, SupplierId = 5, SupplierCode = "SupCode5", SupplierPartNumber = "SupPart5", CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newItem);

        Assert.AreEqual(4, _provider?.Get().Length);

        _provider?.Delete(newItem);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateItem()
    {
        var updatedItem = new Item { Uid = "1", Code = "UpdatedCode1", Description = "UpdatedDescription1", ShortDescription = "UpdatedShortDesc1", UpcCode = "UpdatedUPC1", ModelNumber = "UpdatedModel1", CommodityCode = "UpdatedComm1", ItemLine = 1, ItemGroup = 1, ItemType = 1, UnitPurchaseQuantity = 35, UnitOrderQuantity = 45, PackOrderQuantity = 55, SupplierId = 1, SupplierCode = "UpdatedSupCode1", SupplierPartNumber = "UpdatedSupPart1", CreatedAt = "", UpdatedAt = "" };

        _provider?.Update(updatedItem, "1");

        var items = _provider?.Get();

        Assert.AreEqual("1", items![0].Uid);
        Assert.AreEqual("UpdatedCode1", items[0].Code);
        Assert.AreEqual("UpdatedDescription1", items[0].Description);
        Assert.AreEqual("UpdatedShortDesc1", items[0].ShortDescription);
        Assert.AreEqual("UpdatedUPC1", items[0].UpcCode);
        Assert.AreEqual("UpdatedModel1", items[0].ModelNumber);
        Assert.AreEqual("UpdatedComm1", items[0].CommodityCode);
        Assert.AreEqual(1, items[0].ItemLine);
        Assert.AreEqual(1, items[0].ItemGroup);
        Assert.AreEqual(1, items[0].ItemType);
        Assert.AreEqual(35, items[0].UnitPurchaseQuantity);
        Assert.AreEqual(45, items[0].UnitOrderQuantity);
        Assert.AreEqual(55, items[0].PackOrderQuantity);
        Assert.AreEqual(1, items[0].SupplierId);
        Assert.AreEqual("UpdatedSupCode1", items[0].SupplierCode);
        Assert.AreEqual("UpdatedSupPart1", items[0].SupplierPartNumber);
    }
}
