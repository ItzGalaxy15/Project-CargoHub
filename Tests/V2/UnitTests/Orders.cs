using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

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

        var warehouses = _provider?.Get();
        Assert.AreEqual(4, warehouses?.Length);
        Assert.IsTrue(warehouses?.First(w => w.Id == 5).IsDeleted);
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

[TestClass]
public class OrderModelTest
{
    [TestMethod]
    public void SerializeOrderToJson()
    {
        // Arrange
        var newOrder = new Order 
        { 
            Id = 1, 
            SourceId = 1, 
            OrderDate = "2023-01-01 00:00:00", 
            RequestDate = "2023-01-02 00:00:00", 
            Reference = "REF001", 
            ReferenceExtra = "Extra001", 
            OrderStatus = "Pending", 
            Notes = "Order notes", 
            ShippingNotes = "Shipping notes", 
            PickingNotes = "Picking notes", 
            WarehouseId = 1, 
            ShipTo = 1, 
            BillTo = 1, 
            ShipmentId = 1, 
            TotalAmount = 100, 
            TotalDiscount = 10, 
            TotalTax = 5, 
            TotalSurcharge = 2, 
            Items = new List<ItemSmall>(), 
            CreatedAt = "2023-01-01 00:00:00", 
            UpdatedAt = "2023-01-01 00:00:00"
        };

        // Act
        string json = JsonSerializer.Serialize(newOrder);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""source_id"":1");
        StringAssert.Contains(json, @"""order_date"":""2023-01-01 00:00:00""");
        StringAssert.Contains(json, @"""request_date"":""2023-01-02 00:00:00""");
        StringAssert.Contains(json, @"""reference"":""REF001""");
        StringAssert.Contains(json, @"""reference_extra"":""Extra001""");
        StringAssert.Contains(json, @"""order_status"":""Pending""");
        StringAssert.Contains(json, @"""notes"":""Order notes""");
        StringAssert.Contains(json, @"""shipping_notes"":""Shipping notes""");
        StringAssert.Contains(json, @"""picking_notes"":""Picking notes""");
        StringAssert.Contains(json, @"""warehouse_id"":1");
        StringAssert.Contains(json, @"""ship_to"":1");
        StringAssert.Contains(json, @"""bill_to"":1");
        StringAssert.Contains(json, @"""shipment_id"":1");
        StringAssert.Contains(json, @"""total_amount"":100");
        StringAssert.Contains(json, @"""total_discount"":10");
        StringAssert.Contains(json, @"""total_tax"":5");
        StringAssert.Contains(json, @"""total_surcharge"":2");
        StringAssert.Contains(json, @"""created_at"":""2023-01-01 00:00:00""");
        StringAssert.Contains(json, @"""updated_at"":""2023-01-01 00:00:00""");
    }

    [TestMethod]
    public void DeserializeJsonToOrder()
    {
        // Arrange
        string json = @"
        { 
            ""id"": 1, 
            ""source_id"": 1, 
            ""order_date"": ""2023-01-01 00:00:00"", 
            ""request_date"": ""2023-01-02 00:00:00"", 
            ""reference"": ""REF001"", 
            ""reference_extra"": ""Extra001"", 
            ""order_status"": ""Pending"", 
            ""notes"": ""Order notes"", 
            ""shipping_notes"": ""Shipping notes"", 
            ""picking_notes"": ""Picking notes"", 
            ""warehouse_id"": 1, 
            ""ship_to"": 1, 
            ""bill_to"": 1, 
            ""shipment_id"": 1, 
            ""total_amount"": 100.0, 
            ""total_discount"": 10.0, 
            ""total_tax"": 5.0, 
            ""total_surcharge"": 2.0, 
            ""items"": [], 
            ""created_at"": ""2023-01-01 00:00:00"", 
            ""updated_at"": ""2023-01-01 00:00:00"" 
        }";

        // Act
        var order = JsonSerializer.Deserialize<Order>(json);

        // Assert
        Assert.IsNotNull(order);
        Assert.AreEqual(1, order.Id);
        Assert.AreEqual(1, order.SourceId);
        Assert.AreEqual("2023-01-01 00:00:00", order.OrderDate);
        Assert.AreEqual("2023-01-02 00:00:00", order.RequestDate);
        Assert.AreEqual("REF001", order.Reference);
        Assert.AreEqual("Extra001", order.ReferenceExtra);
        Assert.AreEqual("Pending", order.OrderStatus);
        Assert.AreEqual("Order notes", order.Notes);
        Assert.AreEqual("Shipping notes", order.ShippingNotes);
        Assert.AreEqual("Picking notes", order.PickingNotes);
        Assert.AreEqual(1, order.WarehouseId);
        Assert.AreEqual(1, order.ShipTo);
        Assert.AreEqual(1, order.BillTo);
        Assert.AreEqual(1, order.ShipmentId);
        Assert.AreEqual(100.0, order.TotalAmount);
        Assert.AreEqual(10.0, order.TotalDiscount);
        Assert.AreEqual(5.0, order.TotalTax);
        Assert.AreEqual(2.0, order.TotalSurcharge);
        Assert.AreEqual("2023-01-01 00:00:00", order.CreatedAt);
        Assert.AreEqual("2023-01-01 00:00:00", order.UpdatedAt);
    }
}