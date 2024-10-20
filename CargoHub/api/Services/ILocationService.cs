public interface ILocationService
{
    public Task<Location[]> GetLocations();
    public Task<Location?> GetLocationById(int id);
    public Task<bool> LocationIsValid(Location location);
    public Task AddLocation(Location location);
    public Task<bool> UpdateLocation(int id, Location updatedLocation);
    public Task<bool> DeleteLocation(int id);

}