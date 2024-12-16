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
                CreatedAt = "2004-06-20 17:46:19", 
                UpdatedAt = "2014-06-20 18:46:19" 
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
            CreatedAt = "2014-06-20 17:46:19", 
            UpdatedAt = "2014-06-20 18:46:19" 
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
            CreatedAt = "2014-06-20 17:46:19", 
            UpdatedAt = "2014-06-20 18:46:19" 
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
            CreatedAt = "2014-06-20 17:46:19", 
            UpdatedAt = "2014-06-20 18:46:19" 
        };

        _provider?.Update(newClient, 1);

        var clients = _provider?.Get();

        Assert.AreEqual(1, clients![0].Id);
        Assert.AreEqual("Client B", clients[0].Name);
        Assert.AreEqual("123 Main St", clients[0].Address);
        Assert.AreEqual("Anytown", clients[0].City);
        Assert.AreEqual("12345", clients[0].ZipCode);
        Assert.AreEqual("IL", clients[0].Province);
        Assert.AreEqual("USA", clients[0].Country);
        Assert.AreEqual("John Doe", clients[0].ContactName);
        Assert.AreEqual("555-1234", clients[0].ContactPhone);
        Assert.AreEqual("john.doe@example.com", clients[0].ContactEmail);

        Assert.IsFalse(string.IsNullOrEmpty(clients[0].CreatedAt), "UpdatedAt should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(clients[0].UpdatedAt), "UpdatedAt should not be empty");
        DateTime updatedAt;
        bool isValidFormat = DateTime.TryParseExact(clients[0].UpdatedAt, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updatedAt);
        Assert.IsTrue(isValidFormat, "UpdatedAt should have the format 'yyyy-MM-dd HH:mm:ss'");
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
