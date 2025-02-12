using apiV1.ValidationInterfaces;
using apiV1.Interfaces;

namespace apiV1.Validations
{
    public class InventoryValidationService : IInventoryValidationService
    {
        private readonly IInventoryProvider inventoryProvider;
        private readonly ILocationService locationService;
        private readonly IItemService itemService;

        public InventoryValidationService(IInventoryProvider inventoryProvider, ILocationService locationService, IItemService itemService)
        {
            this.inventoryProvider = inventoryProvider;
            this.locationService = locationService;
            this.itemService = itemService;
        }

        public bool IsInventoryValid(Inventory? inventory, bool update = false)
        {
            if (inventory is null)
            {
                return false;
            }

            if (inventory.Id < 0)
            {
                return false;
            }

            Inventory[] inventories = this.inventoryProvider.Get();
            bool inventoryExists = inventories.Any(i => i.Id == inventory.Id);
            if (update)
            {
                // Put
                if (!inventoryExists)
                {
                    return false;
                }
            }
            else
            {
                // Post
                if (inventoryExists)
                {
                    return false;
                }
            }

            // Validate item_id and item_reference and description of an inventory
            // if (string.IsNullOrWhiteSpace(inventory.ItemId)) return false;
            // if (string.IsNullOrWhiteSpace(inventory.ItemReference)) return false;
            // if (string.IsNullOrWhiteSpace(inventory.Description)) return false;
            Item? item = this.itemService.GetItemById(inventory.ItemId);
            if (item is null)
            {
                return false;
            }

            if (item.Uid != inventory.ItemId)
            {
                return false;
            }

            if (item.Code != inventory.ItemReference)
            {
                return false;
            }

            if (item.Description != inventory.Description)
            {
                return false;
            }

            // Validate locations list
            if (inventory.Locations == null || !inventory.Locations.Any())
            {
                return false;
            }

            List<int> locations = inventory.Locations;
            foreach (int locationId in locations)
            {
                // maybe this is a better option:
                // public async Task<bool> IsInventoryValid(Inventory? inventory, bool update = false);
                // Location? location = await _locationService.GetLocationById(locationId);
                Location? location = this.locationService.GetLocationById(locationId).GetAwaiter().GetResult();
                if (location is null)
                {
                    return false;
                }
            }

            // Validate quantity properties
            if (inventory.TotalOnHand < 0)
            {
                return false;
            }

            if (inventory.TotalExpected < 0)
            {
                return false;
            }

            if (inventory.TotalOrdered < 0)
            {
                return false;
            }

            if (inventory.TotalAllocated < 0)
            {
                return false;
            }

            if (inventory.TotalAvailable < 0)
            {
                return false;
            }

            // Optional: Ensure quantities are consistent
            // if (inventory.TotalOnHand != inventory.TotalExpected + inventory.TotalOrdered + inventory.TotalAllocated + inventory.TotalAvailable)
            //     return false;
            return true;
        }
    }
}