public class LocationProvider : BaseProvider<Location>, ILocationProvider
{
    public LocationProvider(List<Location> mockData) : base(mockData) { }
    public LocationProvider() : base("test_data/locations.json") {}

    public Location[] Get(){
        return context.ToArray();
    }

    public void Add(Location location){
        context.Add(location);
    }

    public void Delete(Location location){
        context.Remove(location);
    }

    public void Update(Location location, int locationId)
    {
        location.Id = locationId;
        int index = context.FindIndex(l => l.Id == locationId);
        context[index] = location;
    }
}