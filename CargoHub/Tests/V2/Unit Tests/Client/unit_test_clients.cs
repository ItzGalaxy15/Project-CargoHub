using Microsoft.VisualStudio.TestTools.UnitTesting;




[TestClass]
public class ClientProviderTests
{
    private ClientProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Client>
        {
            new Client { Id = 1, Name = "Client A", Address = "123 Main St", City = "Anytown", ZipCode = "12345", Province = "IL", Country = "USA",
                    ContactName = "John Doe", ContactPhone = "555-1234", ContactEmail = "john.doe@example.com",
                    CreatedAt = "", UpdatedAt = "" },
        };
        _provider = new ClientProvider(mockData);
    }

    [TestMethod]
    public void CheckClientLength()
    {
        var newClient = new Client { Id = 2, Name = "Client A", Address = "123 Main St", City = "Anytown", ZipCode = "12345", Province = "IL", Country = "USA",
                    ContactName = "John Doe", ContactPhone = "555-1234", ContactEmail = "john.doe@example.com",
                    CreatedAt = "", UpdatedAt = "" };

        _provider?.Add(newClient);

        var clients = _provider?.Get();
        Assert.AreEqual(2, clients?.Length);
    }
}
