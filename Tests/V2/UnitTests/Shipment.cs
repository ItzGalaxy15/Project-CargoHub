using System.Text.Json;

//namespace ShipmentUnitTest;

[TestClass]
public class ShipmentProviderTests
{
    private ShipmentProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Shipment>
        {
            new Shipment { Id = 1, OrderId = 1, SourceId = 1, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }},

            new Shipment { Id = 2, OrderId = 2, SourceId = 2, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }},

            new Shipment { Id = 3, OrderId = 3, SourceId = 3, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }},

        };
        _provider = new ShipmentProvider(mockData);
    }

    [TestMethod]
    public void CheckGetShipment()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddShipment()
    {
        var newShipment = new Shipment { Id = 4, OrderId = 4, SourceId = 4, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }};


        _provider?.Add(newShipment);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteShipment()
    {
        var newShipment = new Shipment { Id = 5, OrderId = 5, SourceId = 5, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }};

        _provider?.Add(newShipment);

        Assert.AreEqual(4, _provider?.Get().Length);
        
        _provider?.Delete(newShipment);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateShipment()
    {
        var newShipment = new Shipment { Id = 2, OrderId = 1, SourceId = 1, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "jeff is sick.",
            CarrierCode = "DPD", CarrierDescription = "dpdn", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }};
;

        _provider?.Update(newShipment, 1);

        var Shipments = _provider?.Get();

        Assert.AreEqual(1, Shipments![0].Id);
        Assert.AreEqual("jeff is sick.", Shipments[0].Notes);
        Assert.AreEqual("dpdn", Shipments[0].CarrierDescription);
    }
}
