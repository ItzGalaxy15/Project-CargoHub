using System.Text.Json;
using apiV2.Interfaces;
namespace apiV2.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryProvider _inventoryProvider;

        public InventoryService(IInventoryProvider inventoryProvider)
        {
            _inventoryProvider = inventoryProvider;
        }
        public Inventory[] GetInventories()
        {
            return _inventoryProvider.Get();
        }

        public Inventory? GetInventoryById(int id)
        {
            Inventory[] inventories = GetInventories();
            Inventory? inventory = inventories.FirstOrDefault(i => i.Id == id);
            return inventory;
        }
        public async Task AddInventory(Inventory inventory)
        {
            string now = inventory.GetTimeStamp();
            inventory.CreatedAt = now;
            inventory.UpdatedAt = now;
            _inventoryProvider.Add(inventory);
            await _inventoryProvider.Save();
        }

        public async Task ReplaceInventory(Inventory inventory, int inventoryId)
        {
            string now = inventory.GetTimeStamp();
            inventory.UpdatedAt = now;
            _inventoryProvider.Update(inventory, inventoryId);
            await _inventoryProvider.Save();
        }

        public async Task DeleteInventory(Inventory inventory)
        {
            _inventoryProvider.Delete(inventory);
            await _inventoryProvider.Save();
        }


        public async Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string id)
        {
            if (!int.TryParse(id, out int inventoryId))
            {
                return null!;
            }
            Inventory? inventory = await Task.Run(() => _inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId));
            if (inventory == null)
            {
                return null!;
            }

            var storageTotals = new Dictionary<string, int>
            {
                { "total_expected", inventory.TotalExpected },
                { "total_ordered", inventory.TotalOrdered },
                { "total_allocated", inventory.TotalAllocated },
                { "total_available", inventory.TotalAvailable }
            };
            return storageTotals;
        }

        public async Task<Inventory?> GetInventoryByUid(string uid)
        {
            if (!int.TryParse(uid, out int inventoryId))
            {
                return null;
            }
            Inventory? inventory = await Task.Run(() => _inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId));
            return inventory;
        }
        public async Task ModifyInventory(int id, Dictionary<string, dynamic> patch, Inventory inventory)
        {
            foreach (var (key, value) in patch)
            {
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "item_id":
                            inventory.ItemId = jsonElement.GetString()!;
                            break;

                        case "description":
                            inventory.Description = jsonElement.GetString()!;
                            break;

                        case "item_reference":
                            inventory.ItemReference = jsonElement.GetString()!;
                            break;

                        case "locations":
                            if (jsonElement.ValueKind == JsonValueKind.Array)
                            {
                                inventory.Locations = jsonElement.EnumerateArray().Select(e => e.GetInt32()).ToList();
                            }
                            break;

                        case "total_on_hand":
                            inventory.TotalOnHand = jsonElement.GetInt32();
                            break;

                        case "total_expected":
                            inventory.TotalExpected = jsonElement.GetInt32();
                            break;

                        case "total_ordered":
                            inventory.TotalOrdered = jsonElement.GetInt32();
                            break;

                        case "total_allocated":
                            inventory.TotalAllocated = jsonElement.GetInt32();
                            break;

                        case "total_available":
                            inventory.TotalAvailable = jsonElement.GetInt32();
                            break;
                    }
                }
            }

            inventory.UpdatedAt = inventory.GetTimeStamp();
            _inventoryProvider.Update(inventory, id);
            await _inventoryProvider.Save();
        }
    }
}