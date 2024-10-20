public class LocationProvider : BaseProvider<Location>, ILocationProvider
{
    public LocationProvider() : base("data/locations.json") {}

    public Location[] Get(){
        return context.ToArray();
    }

    public void Add(Location client){
        context.Add(client);
    }

    public void Delete(int id){
        context.RemoveAt(id);
    }
}