public class LocationProvider : BaseProvider<Location>, ILocationProvider
{
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

    public bool Update(Location location, int locationId)
    {
        int index = context.FindIndex(l => l.Id == locationId);
        if (index == -1) return false;
        context[index] = location;
        return true;
    }
}