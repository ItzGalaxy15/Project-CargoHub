public interface ILocationService
{
    public Task<Location[]> GetLocations();
    public Task<Location?> GetLocationById(int id);
    public Task<bool> LocationIsValid(Location location);
    public Task AddLocation(Location location);
}