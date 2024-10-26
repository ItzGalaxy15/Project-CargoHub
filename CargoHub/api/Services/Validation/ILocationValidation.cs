public interface ILocationValidation
{
    public Task<bool> IsLocationValidForPOST(Location newLocation);
    public Task<bool> IsLocationValidForPUT(Location updatedLocation, int locationId);
}