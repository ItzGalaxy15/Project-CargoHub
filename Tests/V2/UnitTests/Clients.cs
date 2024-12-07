using System.Text.Json;

//namespace ClientUnitTest;

[TestClass]
public class ClientProviderTests
{
    private ClientProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Client>
        {
            new Client 
            { 
                Id = 1, 
                Name = "Client A", 
                Address = "123 Main St", 
                City = "Anytown", 
                ZipCode = "12345", 
                Province = "IL", 
                Country = "USA",
                ContactName = "John Doe", 
                ContactPhone = "555-1234", 
                ContactEmail = "john.doe@example.com",
                CreatedAt = "", 
                UpdatedAt = "" 
            },
        };
        _provider = new ClientProvider(mockData);
    }

    [TestMethod]
    public void CheckGetClient()
    {
        Assert.AreEqual(1, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddClient()
    {
        var newClient = new Client 
        { 
            Id = 2, 
            Name = "Client B", 
            Address = "123 Main St", 
            City = "Anytown", 
            ZipCode = "12345", 
            Province = "IL", 
            Country = "USA",
            ContactName = "John Doe", 
            ContactPhone = "555-1234", 
            ContactEmail = "john.doe@example.com",
            CreatedAt = "", 
            UpdatedAt = "" 
        };
        _provider?.Add(newClient);

        Assert.AreEqual(2, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteClient()
    {
        var newClient = new Client 
        { 
            Id = 2, 
            Name = "Client B", 
            Address = "123 Main St", 
            City = "Anytown", 
            ZipCode = "12345", 
            Province = "IL", 
            Country = "USA",
            ContactName = "John Doe", 
            ContactPhone = "555-1234", 
            ContactEmail = "john.doe@example.com",
            CreatedAt = "", 
            UpdatedAt = "" 
        };

        _provider?.Add(newClient);

        Assert.AreEqual(2, _provider?.Get().Length);
        
        _provider?.Delete(newClient);

        Assert.AreEqual(1, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateClient()
    {
        var newClient = new Client 
        { 
            Id = 2, 
            Name = "Client B", 
            Address = "123 Main St", 
            City = "Anytown", 
            ZipCode = "12345", 
            Province = "IL", 
            Country = "USA",
            ContactName = "John Doe", 
            ContactPhone = "555-1234", 
            ContactEmail = "john.doe@example.com",
            CreatedAt = "", 
            UpdatedAt = "" 
        };

        _provider?.Update(newClient, 1);

        var clients = _provider?.Get();

        Assert.AreEqual(1, clients![0].Id);
        Assert.AreEqual("Client B", clients[0].Name);
    }
}

[TestClass]
public class ClientModelTest
{
    [TestMethod]
    public void SerializeClientToJson()
    {
        // Arrange
        var newClient = new Client 
        { 
            Id = 1, 
            Name = "Client A", 
            Address = "123 Main St", 
            City = "Anytown", 
            ZipCode = "12345", 
            Province = "IL", 
            Country = "USA",
            ContactName = "John Doe", 
            ContactPhone = "555-1234", 
            ContactEmail = "john.doe@example.com",
            CreatedAt = "", 
            UpdatedAt = "" 
        };

        // Act
        string json = JsonSerializer.Serialize(newClient);

        // Assert
        Assert.IsNotNull(json);
    }

    [TestMethod]
    public void DeserializeJsonToClient()
    {
        // Arrange
        string json = @"
        { 
            ""id"": 1, 
            ""name"": ""Client A"", 
            ""address"": ""123 Main St"", 
            ""city"": ""Anytown"", 
            ""zip_code"": ""12345"", 
            ""province"": ""IL"", 
            ""country"": ""USA"",
            ""contact_name"": ""John Doe"", 
            ""contact_phone"": ""555-1234"", 
            ""contact_email"": ""john.doe@example.com"",
            ""created_at"": ""1983-04-13 04:59:55"", 
            ""updated_at"": ""1983-04-13 04:59:55"" 
        }";

        // Act
        var Client = JsonSerializer.Deserialize<Client>(json);

        // Assert
        Assert.IsNotNull(Client);
        Assert.AreEqual(1, Client.Id);
        Assert.AreEqual("12345", Client.ZipCode);
    }
}
