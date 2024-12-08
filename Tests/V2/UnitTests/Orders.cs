using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class OrderProviderTests
{
    private OrderProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Order>
        {
            new Order { Id = 1, SourceId = 1, OrderDate = "", RequestDate = "", Reference = "REF001", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 1, ShipTo = 1, BillTo = 1, ShipmentId = 1, TotalAmount = 100.0, TotalDiscount = 10.0, TotalTax = 5.0, TotalSurcharge = 2.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" },
            new Order { Id = 2, SourceId = 2, OrderDate = "", RequestDate = "", Reference = "REF002", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 2, ShipTo = 2, BillTo = 2, ShipmentId = 2, TotalAmount = 200.0, TotalDiscount = 20.0, TotalTax = 10.0, TotalSurcharge = 4.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" },
            new Order { Id = 3, SourceId = 3, OrderDate = "", RequestDate = "", Reference = "REF003", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 3, ShipTo = 3, BillTo = 3, ShipmentId = 3, TotalAmount = 300.0, TotalDiscount = 30.0, TotalTax = 15.0, TotalSurcharge = 6.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" }
        };
        _provider = new OrderProvider(mockData);
    }

    [TestMethod]
    public void CheckGetOrders()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddOrder()
    {
        var newOrder = new Order { Id = 4, SourceId = 4, OrderDate = "", RequestDate = "", Reference = "REF004", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 4, ShipTo = 4, BillTo = 4, ShipmentId = 4, TotalAmount = 400.0, TotalDiscount = 40.0, TotalTax = 20.0, TotalSurcharge = 8.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newOrder);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteOrder()
    {
        var newOrder = new Order { Id = 5, SourceId = 5, OrderDate = "", RequestDate = "", Reference = "REF005", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 5, ShipTo = 5, BillTo = 5, ShipmentId = 5, TotalAmount = 500.0, TotalDiscount = 50.0, TotalTax = 25.0, TotalSurcharge = 10.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newOrder);

        Assert.AreEqual(4, _provider?.Get().Length);

        _provider?.Delete(newOrder);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateOrder()
    {
        var updatedOrder = new Order { Id = 1, SourceId = 1, OrderDate = "", RequestDate = "", Reference = "REF001-UPDATED", ReferenceExtra = "", OrderStatus = "", Notes = "", ShippingNotes = "", PickingNotes = "", WarehouseId = 1, ShipTo = 1, BillTo = 1, ShipmentId = 1, TotalAmount = 100.0, TotalDiscount = 10.0, TotalTax = 5.0, TotalSurcharge = 2.0, Items = new List<ItemSmall>(), CreatedAt = "", UpdatedAt = "" };

        _provider?.Update(updatedOrder, 1);

        var orders = _provider?.Get();

        Assert.AreEqual(1, orders![0].Id);
        Assert.AreEqual("REF001-UPDATED", orders[0].Reference);
    }
}