using System.Text.Json;

namespace WarehouseUnitTest;

[TestClass]
public class WarehouseProviderTests
{
    private WarehouseProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Warehouse>
        {
            new Warehouse
            {
                Id = 1,
                Code = "YQZZNL56",
                Name = "Heemskerk cargo hub",
                Address = "Karlijndreef 281",
                Zip = "4002 AS",
                City = "Heemskerk",
                Province = "Friesland",
                Country = "NL",
                Contact = new WarehouseContact
                {
                    Name = "Fem Keijzer",
                    Phone = "(078) 0013363",
                    Email = "blamore@example.net"
                },
                CreatedAt = "1983-04-13 04:59:55",
                UpdatedAt = "2007-02-08 20:11:00"
            },
            new Warehouse
            {
                Id = 2,
                Code = "GIOMNL90",
                Name = "Petten longterm hub",
                Address = "Owenweg 731",
                Zip = "4615 RB",
                City = "Petten",
                Province = "Noord-Holland",
                Country = "NL",
                Contact = new WarehouseContact
                {
                    Name = "Maud Adryaens",
                    Phone = "+31836 752702",
                    Email = "nickteunissen@example.com"
                },
                CreatedAt = "2008-02-22 19:55:39",
                UpdatedAt = "2009-08-28 23:15:50"
            }
        };
        _provider = new WarehouseProvider(mockData);
    }

    [TestMethod]
    public void CheckGetWarehouse()
    {
        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddWarehouse()
    {
        var newWarehouse = new Warehouse
        {
            Id = 3,
            Code = "NEWCODE",
            Name = "New Warehouse",
            Address = "New Address",
            Zip = "12345",
            City = "New City",
            Province = "New Province",
            Country = "NL",
            Contact = new WarehouseContact
            {
                Name = "New Contact",
                Phone = "123456789",
                Email = "newcontact@example.com"
            },
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newWarehouse);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteWarehouse()
    {
        var newWarehouse = new Warehouse
        {
            Id = 3,
            Code = "NEWCODE",
            Name = "New Warehouse",
            Address = "New Address",
            Zip = "12345",
            City = "New City",
            Province = "New Province",
            Country = "NL",
            Contact = new WarehouseContact
            {
                Name = "New Contact",
                Phone = "123456789",
                Email = "newcontact@example.com"
            },
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newWarehouse);

        Assert.AreEqual(3, _provider?.Get().Length);

        _provider?.Delete(newWarehouse);

        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateWarehouse()
    {
        var updatedWarehouse = new Warehouse
        {
            Id = 1,
            Code = "UPDATEDCODE",
            Name = "Updated Warehouse",
            Address = "Updated Address",
            Zip = "54321",
            City = "Updated City",
            Province = "Updated Province",
            Country = "NL",
            Contact = new WarehouseContact
            {
                Name = "Updated Contact",
                Phone = "987654321",
                Email = "updatedcontact@example.com"
            },
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Update(updatedWarehouse, 1);

        var warehouses = _provider?.Get();

        Assert.AreEqual(1, warehouses![0].Id);
        Assert.AreEqual("UPDATEDCODE", warehouses[0].Code);
        Assert.AreEqual("Updated Warehouse", warehouses[0].Name);
    }
}

[TestClass]
public class WarehouseUnitTest
{
    [TestMethod]
    public void SerializeWarehouseToJson()
    {
        // Arrange
        var contact = new WarehouseContact
        {
            Name = "Fem Keijzer",
            Phone = "(078) 0013363",
            Email = "blamore@example.net"
        };

        var warehouse = new Warehouse
        {
            Id = 1,
            Code = "YQZZNL56",
            Name = "Heemskerk cargo hub",
            Address = "Karlijndreef 281",
            Zip = "4002 AS",
            City = "City",
            Province = "Friesland",
            Country = "NL",
            Contact = contact,
            CreatedAt = "1983-04-13 04:59:55",
            UpdatedAt = "2007-02-08 20:11:00"
        };

        // Act
        string json = JsonSerializer.Serialize(warehouse);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""code"":""YQZZNL56""");
        StringAssert.Contains(json, @"""name"":""Heemskerk cargo hub""");
        StringAssert.Contains(json, @"""address"":""Karlijndreef 281""");
        StringAssert.Contains(json, @"""zip"":""4002 AS""");
        StringAssert.Contains(json, @"""city"":""City""");
        StringAssert.Contains(json, @"""province"":""Friesland""");
        StringAssert.Contains(json, @"""country"":""NL""");
        StringAssert.Contains(json, @"""created_at"":""1983-04-13 04:59:55""");
        StringAssert.Contains(json, @"""updated_at"":""2007-02-08 20:11:00""");
        StringAssert.Contains(json, @"""name"":""Fem Keijzer""");
        StringAssert.Contains(json, @"""phone"":""(078) 0013363""");
        StringAssert.Contains(json, @"""email"":""blamore@example.net""");

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
    public void DeserializeJsonToWarehouse()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""code"": ""YQZZNL56"",
            ""name"": ""Heemskerk cargo hub"",
            ""address"": ""Karlijndreef 281"",
            ""zip"": ""4002 AS"",
            ""city"": ""City"",
            ""province"": ""Friesland"",
            ""country"": ""NL"",
            ""contact"": {
                ""name"": ""Fem Keijzer"",
                ""phone"": ""(078) 0013363"",
                ""email"": ""blamore@example.net""
            },
            ""created_at"": ""1983-04-13 04:59:55"",
            ""updated_at"": ""2007-02-08 20:11:00""
        }";

        // Act
        var warehouse = JsonSerializer.Deserialize<Warehouse>(json);

        // Assert
        Assert.IsNotNull(warehouse);
        Assert.AreEqual(1, warehouse.Id);
        Assert.AreEqual("YQZZNL56", warehouse.Code);
        Assert.AreEqual("Heemskerk cargo hub", warehouse.Name);
        Assert.AreEqual("Karlijndreef 281", warehouse.Address);
        Assert.AreEqual("4002 AS", warehouse.Zip);
        Assert.AreEqual("City", warehouse.City);
        Assert.AreEqual("Friesland", warehouse.Province);
        Assert.AreEqual("NL", warehouse.Country);
        Assert.AreEqual("Fem Keijzer", warehouse.Contact.Name);
        Assert.AreEqual("(078) 0013363", warehouse.Contact.Phone);
        Assert.AreEqual("blamore@example.net", warehouse.Contact.Email);

        // DateTime format checks
        bool isValidCreatedAt = DateTime.TryParseExact(warehouse.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);
        bool isValidUpdatedAt = DateTime.TryParseExact(warehouse.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);

        Assert.IsTrue(isValidCreatedAt, "CreatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
        Assert.IsTrue(isValidUpdatedAt, "UpdatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
    }
}