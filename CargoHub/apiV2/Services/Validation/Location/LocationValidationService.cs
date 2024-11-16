using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class LocationValidationService : ILocationValidationService
    {
        private readonly ILocationProvider _locationProvider;
        public LocationValidationService(ILocationProvider locationProvider){
            _locationProvider = locationProvider;
        }

        public async Task<bool> IsLocationValidForPOST(Location newLocation){
            if (newLocation == null) return false;
            if (newLocation.Id < 0) return false;
            Location[] locations = _locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == newLocation.Id));
            if (location != null) return false;
            // if (location.Id < 0) return false;
            // if (string.IsNullOrWhiteSpace(newLocation.Code)) return false;
            // if (string.IsNullOrWhiteSpace(newLocation.Name)) return false;
            return true;
        }

        public async Task<bool> IsLocationValidForPUT(Location updatedLocation, int locationId)
        {
            if (updatedLocation == null) return false;
            if (updatedLocation.Id < 0) return false;
            Location[] locations = _locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == updatedLocation.Id));
            int index = locations.ToList().FindIndex(l => l.Id == locationId);
            if (index == -1) return false;
            if (location == null) return false;
            //if (updatedLocation.Id < 0) return false;
            // if (string.IsNullOrWhiteSpace(updatedLocation.Code)) return false;
            // if (string.IsNullOrWhiteSpace(updatedLocation.Name)) return false;
            return true;
        }

        public async Task<bool> IsLocationValidForPATCH(Dictionary<string, dynamic> patch, int locationId){
            if (patch == null) return false;
            var validProperties = new HashSet<string> {
                "warehouse_id",
                "code",
                "name",
            };
            Location[] locations = _locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == locationId));

            if (location == null) return false;

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.Contains(key))
                {
                    validKeysInPatch.Add(key);
                    break;
                }
            }

            if (!validKeysInPatch.Any())
            {
                return false;
            }
            return true;
        }
    }
}