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
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }},

            new Shipment { Id = 2, OrderId = 2, SourceId = 2, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-25 17:46:19", UpdatedAt = "2014-06-26 17:46:19",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }},

            new Shipment { Id = 3, OrderId = 3, SourceId = 3, OrderDate = "2000-03-09", RequestDate = "2000-03-11", ShipmentDate = "2000-03-13",
            ShipmentType = "I", ShipmentStatus = "pending", Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
            CarrierCode = "DPD", CarrierDescription = "Dynamic Parcel Distribution", ServiceCode = "Fastest", PaymentType = "Manual",
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-26 17:46:19", UpdatedAt = "2014-06-27 17:46:19",
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
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19",
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
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19",
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
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19",
            Items = new List<ItemSmall> { new ItemSmall { ItemId = "P007435", Amount = 1 } }};
;

        _provider?.Update(newShipment, 1);

        var Shipments = _provider?.Get();

        Assert.AreEqual(1, Shipments![0].Id);
        Assert.AreEqual(1, Shipments[0].OrderId);
        Assert.AreEqual(1, Shipments[0].SourceId);
        Assert.AreEqual("2000-03-09", Shipments[0].OrderDate);
        Assert.AreEqual("2000-03-11", Shipments[0].RequestDate);
        Assert.AreEqual("2000-03-13", Shipments[0].ShipmentDate);
        Assert.AreEqual("I", Shipments[0].ShipmentType);
        Assert.AreEqual("pending", Shipments[0].ShipmentStatus);
        Assert.AreEqual("jeff is sick.", Shipments[0].Notes);
        Assert.AreEqual("DPD", Shipments[0].CarrierCode);
        Assert.AreEqual("dpdn", Shipments[0].CarrierDescription);
        Assert.AreEqual("Fastest", Shipments[0].ServiceCode);
        Assert.AreEqual("Manual", Shipments[0].PaymentType);
        Assert.AreEqual("Ground", Shipments[0].TransferMode);
        Assert.AreEqual(31, Shipments[0].TotalPackageCount);
        Assert.AreEqual(600.12, Shipments[0].TotalPackageWeight);
        
        Assert.IsFalse(string.IsNullOrEmpty(Shipments[0].CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(Shipments[0].UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(Shipments[0].UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(Shipments[0].CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        Assert.AreEqual("P007435", Shipments[0].Items[0].ItemId);
        Assert.AreEqual(1, Shipments[0].Items[0].Amount);

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
            TransferMode = "Ground", TotalPackageCount = 31, TotalPackageWeight = 600.12, CreatedAt = "2014-06-24 17:46:19", UpdatedAt = "2014-06-25 17:46:19",
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
            ""created_at"": ""2014-06-24 17:46:19"",
            ""updated_at"": ""2014-06-25 17:46:19"",
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
        Assert.AreEqual(1, Shipment.OrderId);
        Assert.AreEqual(1, Shipment.SourceId);
        Assert.AreEqual("2000-03-09", Shipment.OrderDate);
        Assert.AreEqual("2000-03-11", Shipment.RequestDate);
        Assert.AreEqual("2000-03-13", Shipment.ShipmentDate);
        Assert.AreEqual("I", Shipment.ShipmentType);
        Assert.AreEqual("pending", Shipment.ShipmentStatus);
        Assert.AreEqual("Zee vertrouwen klas rots heet lachen oneven begrijpen.", Shipment.Notes);
        Assert.AreEqual("DPD", Shipment.CarrierCode);
        Assert.AreEqual("Dynamic Parcel Distribution", Shipment.CarrierDescription);
        Assert.AreEqual("Fastest", Shipment.ServiceCode);
        Assert.AreEqual("Manual", Shipment.PaymentType);
        Assert.AreEqual("Ground", Shipment.TransferMode);
        Assert.AreEqual(31, Shipment.TotalPackageCount);
        Assert.AreEqual(600.12, Shipment.TotalPackageWeight);

        Assert.IsFalse(string.IsNullOrEmpty(Shipment.CreatedAt), "CreatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(Shipment.UpdatedAt), "UpdatedAt should not be empty");

        DateTime updatedAt;
        bool isValidFormatUpdate = DateTime.TryParseExact(Shipment.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormatUpdate, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        DateTime createdAt;
        bool isValidFormatCreated = DateTime.TryParseExact(Shipment.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out createdAt);
        Assert.IsTrue(isValidFormatCreated, "CreatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");

        Assert.AreEqual("P007435", Shipment.Items[0].ItemId);
        Assert.AreEqual(1, Shipment.Items[0].Amount);
    }
}
