using apiV1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


[TestClass]
public class LocationProviderTests
{
    private LocationProvider? _provider;
    
    [TestInitialize]
    public void SetUp()
    {
        var mockData = new List<Location>
        {
            new Location { Id = 1, WarehouseId = 1, Code = "A.2.e21e21e21e", Name = "Row: A, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "", UpdatedAt = ""},
            new Location { Id = 2, WarehouseId = 2, Code = "A.2.21fefefeff", Name = "Row: B, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "", UpdatedAt = ""},
            new Location { Id = 3, WarehouseId = 3, Code = "A.2.1231312321", Name = "Row: C, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "", UpdatedAt = ""}

        };
        _provider = new LocationProvider(mockData);
    }

    [TestMethod]
    public void CheckGetLocation()
    {
        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckAddLocation()
    {
        var newLocation = new Location { Id = 4, WarehouseId = 4, Code = "A.2.1231312321", Name = "Row: C, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "", UpdatedAt = ""};

        _provider?.Add(newLocation);

        Assert.AreEqual(4, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckDeleteLocation()
    {
        var newLocation = new Location { Id = 5, WarehouseId = 3, Code = "A.2.1231312321", Name = "Row: C, R3123123123213231ack: 2, Shelf: 0", CreatedAt = "", UpdatedAt = ""};
        
        _provider?.Add(newLocation);

        Assert.AreEqual(4, _provider?.Get().Length);
        
        _provider?.Delete(newLocation);

        Assert.AreEqual(3, _provider?.Get().Length);
    }

    [TestMethod]
    public void CheckUpdateLocation()
    {
        var newLocation = new Location { Id = 1, WarehouseId = 1, Code = "A.2",
                                            Name = "Row: A, R3123123123213231ack: 2, Shelf: 0",
                                            CreatedAt = "",
                                            UpdatedAt = ""};

        _provider?.Update(newLocation, 1);

        var Locations = _provider?.Get();

        Assert.AreEqual(1, Locations![0].Id);
        Assert.AreEqual("Row: A, R3123123123213231ack: 2, Shelf: 0", Locations[0].Name);
        Assert.AreEqual("A.2", Locations[0].Code);
    }
}
