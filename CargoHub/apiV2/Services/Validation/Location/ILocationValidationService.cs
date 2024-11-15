namespace apiV2.ValidationInterfaces
{
    public interface ILocationValidationService
    {
        public Task<bool> IsLocationValidForPOST(Location newLocation);
        public Task<bool> IsLocationValidForPUT(Location updatedLocation, int locationId);
        public Task<bool> IsLocationValidForPATCH(Dictionary<string, dynamic> patch, int locationId);
    }
}