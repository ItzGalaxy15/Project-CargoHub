using System.Text.Json;

//namespace ShipmentUnitTest;

[TestClass]
public class ShipmentProviderTest
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

[TestClass]
public class ShipmentModelTest
{
    [TestMethod]
    public void SerializeShipmentToJson()
    {
        var newShipment = new Shipment { Id = 1, OrderId = 1, SourceId = 1, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "", UpdatedAt = "",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }};

        // Act
        string json = JsonSerializer.Serialize(newShipment);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToShipment()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""order_id"": 1,
            ""source_id"": 1,
            ""order_date"": ""2000-03-09"",
            ""request_date"": ""2000-03-11"",
            ""shipment_date"": ""2000-03-13"",
            ""shipment_type"": ""I"",
            ""shipment_status"": ""pending"",
            ""notes"": ""Zee vertrouwen klas rots heet lachen oneven begrijpen."",
            ""carrier_code"": ""DPD"",
            ""carrier_description"": ""Dynamic Parcel Distribution"",
            ""service_code"": ""Fastest"",
            ""payment_type"": ""Manual"",
            ""transfer_mode"": ""Ground"",
            ""total_package_count"": 31,
            ""total_package_weight"": 600.12,
            ""created_at"": """",
            ""updated_at"": """",
            ""items"": [
                {
                    ""item_id"": ""P007435"",
                    ""amount"": 1
                }
            ]
        }";

        // Act
        var Shipment = JsonSerializer.Deserialize<Shipment>(json);

        // Assert
        Assert.IsNotNull(Shipment);
        Assert.AreEqual(1, Shipment.Id);
        Assert.AreEqual("I", Shipment.ShipmentType);
        Assert.AreEqual(600.12, Shipment.TotalPackageWeight);
        Assert.AreEqual("P007435", Shipment.Items[0].ItemId);
    }
}
