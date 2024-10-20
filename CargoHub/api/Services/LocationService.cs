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
    
    public async Task<bool> LocationIsValid(Location newLocation){
        if (newLocation == null) return false;
        if (newLocation.Id <= 0) return false;
        Location[] locations = _locationProvider.Get();
        Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == newLocation.Id));
        if (location != null) return false;
        return true;
    }

    public async Task AddLocation(Location location){
        location.CreatedAt = location.GetTimeStamp();
        location.UpdatedAt = location.GetTimeStamp();
        _locationProvider.Add(location);
        await _locationProvider.Save();
    }
}