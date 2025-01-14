using System.Text.Json;

// namespace SupplierUnitTest;

[TestClass]
public class SupplierProviderTests
{
    private SupplierProvider? _provider;

    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Supplier>
        {
            new Supplier
            {
                Id = 1,
                Code = "SUP0001",
                Name = "Lee, Parks and Johnson",
                Address = "5989 Sullivan Drives",
                AddressExtra = "Apt. 996",
                City = "Port Anitaburgh",
                ZipCode = "91688",
                Province = "Illinois",
                Country = "Czech Republic",
                ContactName = "Toni Barnett",
                Phonenumber = "363.541.7282x36825",
                Reference = "LPaJ-SUP0001",
                CreatedAt = "1971-10-20 18:06:17",
                UpdatedAt = "1985-06-08 00:13:46"
            },
            new Supplier
            {
                Id = 2,
                Code = "SUP0002",
                Name = "Holden-Quinn",
                Address = "576 Christopher Roads",
                AddressExtra = "Suite 072",
                City = "Amberbury",
                ZipCode = "16105",
                Province = "Illinois",
                Country = "Saint Martin",
                ContactName = "Kathleen Vincent",
                Phonenumber = "001-733-291-8848x3542",
                Reference = "H-SUP0002",
                CreatedAt = "1995-12-18 03:05:46",
                UpdatedAt = "2019-11-10 22:11:12"
            }
        };
        _provider = new SupplierProvider(mockData);
    }

    [TestMethod]
    public void CheckGetSupplier()
    {
        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddSupplier()
    {
        var newSupplier = new Supplier
        {
            Id = 3,
            Code = "SUP0003",
            Name = "New Supplier",
            Address = "New Address",
            AddressExtra = "Suite 123",
            City = "New City",
            ZipCode = "12345",
            Province = "New Province",
            Country = "New Country",
            ContactName = "New Contact",
            Phonenumber = "123456789",
            Reference = "NS-SUP0003",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newSupplier);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteSupplier()
    {
        var newSupplier = new Supplier
        {
            Id = 3,
            Code = "SUP0003",
            Name = "New Supplier",
            Address = "New Address",
            AddressExtra = "Suite 123",
            City = "New City",
            ZipCode = "12345",
            Province = "New Province",
            Country = "New Country",
            ContactName = "New Contact",
            Phonenumber = "123456789",
            Reference = "NS-SUP0003",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Add(newSupplier);

        Assert.AreEqual(3, _provider?.Get().Length);

        _provider?.Delete(newSupplier);

        var suppliers = _provider?.Get();
        Assert.AreEqual(3, suppliers?.Length);
        Assert.IsTrue(suppliers?.First(s => s.Id == 3).IsDeleted);

    }

    [TestMethod]
    public void CheckUpdateSupplier()
    {
        var updatedSupplier = new Supplier
        {
            Id = 1,
            Code = "SUP0001",
            Name = "Updated Supplier",
            Address = "Updated Address",
            AddressExtra = "Suite 456",
            City = "Updated City",
            ZipCode = "54321",
            Province = "Updated Province",
            Country = "Updated Country",
            ContactName = "Updated Contact",
            Phonenumber = "987654321",
            Reference = "US-SUP0001",
            CreatedAt = "2023-01-01 00:00:00",
            UpdatedAt = "2023-01-01 00:00:00"
        };

        _provider?.Update(updatedSupplier, 1);

        var suppliers = _provider?.Get();

        Assert.AreEqual(1, suppliers![0].Id);
        Assert.AreEqual("SUP0001", suppliers[0].Code);
        Assert.AreEqual("Updated Supplier", suppliers[0].Name);
        Assert.AreEqual("Updated City", suppliers[0].City);
    }
}


[TestClass]
public class SupplierUnitTest
{
    [TestMethod]
    public void SerializeSupplierToJson()
    {
        // Arrange
        var supplier = new Supplier
        {
            Id = 1,
            Code = "SUP0001",
            Name = "Lee, Parks and Johnson",
            Address = "5989 Sullivan Drives",
            AddressExtra = "Apt. 996",
            City = "Port Anitaburgh",
            ZipCode = "91688",
            Province = "Illinois",
            Country = "Czech Republic",
            ContactName = "Toni Barnett",
            Phonenumber = "363.541.7282x36825",
            Reference = "LPaJ-SUP0001",
            CreatedAt = "1971-10-20 18:06:17",
            UpdatedAt = "1985-06-08 00:13:46"
        };

        // Act
        string json = JsonSerializer.Serialize(supplier);

        // Assert
        Assert.IsNotNull(json);
        StringAssert.Contains(json, @"""id"":1");
        StringAssert.Contains(json, @"""code"":""SUP0001""");
        StringAssert.Contains(json, @"""name"":""Lee, Parks and Johnson""");
        StringAssert.Contains(json, @"""address"":""5989 Sullivan Drives""");
        StringAssert.Contains(json, @"""address_extra"":""Apt. 996""");
        StringAssert.Contains(json, @"""city"":""Port Anitaburgh""");
        StringAssert.Contains(json, @"""zip_code"":""91688""");
        StringAssert.Contains(json, @"""province"":""Illinois""");
        StringAssert.Contains(json, @"""country"":""Czech Republic""");
        StringAssert.Contains(json, @"""contact_name"":""Toni Barnett""");
        StringAssert.Contains(json, @"""phonenumber"":""363.541.7282x36825""");
        StringAssert.Contains(json, @"""reference"":""LPaJ-SUP0001""");
        StringAssert.Contains(json, @"""created_at"":""1971-10-20 18:06:17""");
        StringAssert.Contains(json, @"""updated_at"":""1985-06-08 00:13:46""");

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
    public void DeserializeJsonToSupplier()
    {
        // Arrange
        string json = @"
        {
            ""id"": 1,
            ""code"": ""SUP0001"",
            ""name"": ""Lee, Parks and Johnson"",
            ""address"": ""5989 Sullivan Drives"",
            ""address_extra"": ""Apt. 996"",
            ""city"": ""Port Anitaburgh"",
            ""zip_code"": ""91688"",
            ""province"": ""Illinois"",
            ""country"": ""Czech Republic"",
            ""contact_name"": ""Toni Barnett"",
            ""phonenumber"": ""363.541.7282x36825"",
            ""reference"": ""LPaJ-SUP0001"",
            ""created_at"": ""1971-10-20 18:06:17"",
            ""updated_at"": ""1985-06-08 00:13:46""
        }";

        // Act
        var supplier = JsonSerializer.Deserialize<Supplier>(json);

        // Assert
        Assert.IsNotNull(supplier);
        Assert.AreEqual(1, supplier.Id);
        Assert.AreEqual("SUP0001", supplier.Code);
        Assert.AreEqual("Lee, Parks and Johnson", supplier.Name);
        Assert.AreEqual("5989 Sullivan Drives", supplier.Address);
        Assert.AreEqual("Apt. 996", supplier.AddressExtra);
        Assert.AreEqual("Port Anitaburgh", supplier.City);
        Assert.AreEqual("91688", supplier.ZipCode);
        Assert.AreEqual("Illinois", supplier.Province);
        Assert.AreEqual("Czech Republic", supplier.Country);
        Assert.AreEqual("Toni Barnett", supplier.ContactName);
        Assert.AreEqual("363.541.7282x36825", supplier.Phonenumber);
        Assert.AreEqual("LPaJ-SUP0001", supplier.Reference);
        Assert.AreEqual("1971-10-20 18:06:17", supplier.CreatedAt);
        Assert.AreEqual("1985-06-08 00:13:46", supplier.UpdatedAt);

        // DateTime format checks
        bool isValidCreatedAt = DateTime.TryParseExact(supplier.CreatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);
        bool isValidUpdatedAt = DateTime.TryParseExact(supplier.UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _);

        Assert.IsTrue(isValidCreatedAt, "CreatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
        Assert.IsTrue(isValidUpdatedAt, "UpdatedAt does not match the expected format 'yyyy-MM-dd HH:mm:ss'");
    }
}
