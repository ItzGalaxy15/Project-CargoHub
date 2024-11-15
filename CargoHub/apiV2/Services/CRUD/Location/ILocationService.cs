namespace apiV2.Interfaces
{
    public interface ILocationService
    {
        public Task<Location[]> GetLocations();
        public Task<Location?> GetLocationById(int id);
        public Task AddLocation(Location location);
        public Task UpdateLocation(int id, Location updatedLocation);
        public Task DeleteLocation(Location location);
        public Location[] GetLocationsInWarehouse(int warehouseId);
        public Task PatchLocation(int id, Dictionary<string, dynamic> patch, Location location);
    }
}