using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationProvider locationProvider;

        public LocationService(ILocationProvider locationProvider)
        {
            this.locationProvider = locationProvider;
        }

        private readonly string locationFilePath = Path.Combine(AppContext.BaseDirectory, "data", "locations.json");

        public async Task<Location[]> GetLocations()
        {
            var json = await File.ReadAllTextAsync(this.locationFilePath);
            var locations = JsonSerializer.Deserialize<Location[]>(json);
            return locations ?? Array.Empty<Location>();
        }

        public async Task<Location?> GetLocationById(int id)
        {
            Location[] locations = this.locationProvider.Get();
            Location? location = await
                Task.FromResult(locations.FirstOrDefault(c => c.Id == id));
            return location;
        }

        public async Task AddLocation(Location location)
        {
            location.CreatedAt = location.GetTimeStamp();
            location.UpdatedAt = location.GetTimeStamp();
            this.locationProvider.Add(location);
            await this.locationProvider.Save();
        }

        public async Task UpdateLocation(int id, Location updatedLocation)
        {
            updatedLocation.UpdatedAt = updatedLocation.GetTimeStamp();
            this.locationProvider.Update(updatedLocation, id);
            await this.locationProvider.Save();
        }

        public async Task DeleteLocation(Location location)
        {
            this.locationProvider.Delete(location);
            await this.locationProvider.Save();
        }

        public Location[] GetLocationsInWarehouse(int warehouseId)
        {
            Location[] locations = this.locationProvider.Get()
                                .Where(l => l.WarehouseId == warehouseId)
                                .ToArray();

            return locations;
        }

        public async Task PatchLocation(int id, Dictionary<string, dynamic> patch, Location location)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "name":
                            location.Name = jsonElement.GetString()!;
                            break;
                        case "warehouse_id":
                            location.WarehouseId = jsonElement.GetInt32()!;
                            break;
                        case "code":
                            location.Code = jsonElement.GetString()!;
                            break;
                    }
                }
            }

            location.UpdatedAt = location.GetTimeStamp();
            this.locationProvider.Update(location, id);
            await this.locationProvider.Save();
        }
    }
}