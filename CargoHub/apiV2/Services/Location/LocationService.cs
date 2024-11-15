using apiV2.Interfaces;

namespace apiV2.Services
{
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

        public async Task AddLocation(Location location){
            location.CreatedAt = location.GetTimeStamp();
            location.UpdatedAt = location.GetTimeStamp();
            _locationProvider.Add(location);
            await _locationProvider.Save();
        }

        public async Task UpdateLocation(int id, Location updatedLocation){
            updatedLocation.UpdatedAt = updatedLocation.GetTimeStamp();
            _locationProvider.Update(updatedLocation, id);
            await _locationProvider.Save();
    
        }
        
        public async Task DeleteLocation(Location location){
            _locationProvider.Delete(location);
            await _locationProvider.Save();
        }

        public Location[] GetLocationsInWarehouse(int warehouseId)
        {
            Location[] locations = _locationProvider.Get()
                                .Where(l => l.WarehouseId == warehouseId)
                                .ToArray();

            return locations;
        }
    }
}