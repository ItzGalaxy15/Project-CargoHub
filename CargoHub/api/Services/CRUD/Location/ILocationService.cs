namespace api.Interface
{
    public interface ILocationService
    {
        public Task<Location[]> GetLocations();
        public Task<Location?> GetLocationById(int id);
        public Task AddLocation(Location location);
        public Task UpdateLocation(int id, Location updatedLocation);
        public Task DeleteLocation(Location location);
        public Location[] GetLocationsInWarehouse(int warehouseId);
    }
}