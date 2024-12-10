public class LocationProvider : BaseProvider<Location>, ILocationProvider
{
    public LocationProvider(List<Location> mockData)
        : base(mockData)
    {
    }

    public LocationProvider()
        : base("test_data/locations.json")
    {
    }

    public Location[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Location location)
    {
        this.context.Add(location);
    }

    public void Delete(Location location)
    {
        this.context.Remove(location);
    }

    public void Update(Location location, int locationId)
    {
        location.Id = locationId;
        int index = this.context.FindIndex(l => l.Id == locationId);
        this.context[index] = location;
    }
}