public class LocationService : ILocationService
{
    private readonly ILocationProvider _locationProvider;

    public LocationService(ILocationProvider locationProvider){
        _locationProvider = locationProvider;
    }

    public async Task<Location[]> GetLocations(){
        Location[] locations = _locationProvider.Get();
        return await Task.FromResult(locations.ToArray());
    }

    public async Task<Location?> GetLocationById(int id){
        Location[] Locations = _locationProvider.Get();
        Location? Location = await 
            Task.FromResult(Locations.FirstOrDefault(c => c.Id == id));
        return Location;
    }

}