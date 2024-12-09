using System.Text.Json;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class LocationValidationService : ILocationValidationService
    {
        private readonly ILocationProvider locationProvider;

        public LocationValidationService(ILocationProvider locationProvider)
        {
            this.locationProvider = locationProvider;
        }

        public async Task<bool> IsLocationValidForPOST(Location newLocation)
        {
            if (newLocation == null)
            {
                return false;
            }

            if (newLocation.Id < 0)
            {
                return false;
            }

            Location[] locations = this.locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == newLocation.Id));
            if (location != null)
            {
                return false;
            }

            // if (location.Id < 0) return false;
            // if (string.IsNullOrWhiteSpace(newLocation.Code)) return false;
            // if (string.IsNullOrWhiteSpace(newLocation.Name)) return false;
            return true;
        }

        public async Task<bool> IsLocationValidForPUT(Location updatedLocation, int locationId)
        {
            if (updatedLocation == null)
            {
                return false;
            }

            if (updatedLocation.Id < 0)
            {
                return false;
            }

            Location[] locations = this.locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == updatedLocation.Id));
            int index = locations.ToList().FindIndex(l => l.Id == locationId);
            if (index == -1)
            {
                return false;
            }

            if (location == null)
            {
                return false;
            }

            // if (updatedLocation.Id < 0) return false;
            // if (string.IsNullOrWhiteSpace(updatedLocation.Code)) return false;
            // if (string.IsNullOrWhiteSpace(updatedLocation.Name)) return false;
            return true;
        }

        public async Task<bool> IsLocationValidForPATCH(Dictionary<string, dynamic> patch, int locationId)
        {
            if (patch == null)
            {
                return false;
            }

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "warehouse_id", JsonValueKind.Number },
                { "code", JsonValueKind.String },
                { "name", JsonValueKind.String },
            };
            Location[] locations = this.locationProvider.Get();
            Location? location = await Task.FromResult(locations.FirstOrDefault(l => l.Id == locationId));

            if (location == null)
            {
                return false;
            }

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.ContainsKey(key))
                {
                    var expectedType = validProperties[key];
                    JsonElement value = patch[key];
                    if (value.ValueKind != expectedType)
                    {
                        patch.Remove(key);

                        // remove key if not valid type
                    }
                    else
                    {
                        validKeysInPatch.Add(key);
                    }
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