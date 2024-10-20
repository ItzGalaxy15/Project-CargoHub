public interface ILocationService
{
    public Task<Location[]> GetLocations();
    public Task<Location?> GetLocationById(int id);
}