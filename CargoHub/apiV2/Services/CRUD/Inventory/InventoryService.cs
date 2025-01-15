using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryProvider inventoryProvider;

        public InventoryService(IInventoryProvider inventoryProvider)
        {
            this.inventoryProvider = inventoryProvider;
        }

        public Inventory[] GetInventories()
        {
            return this.inventoryProvider.Get();
        }

        public Inventory? GetInventoryById(int id)
        {
            Inventory[] inventories = this.GetInventories();
            Inventory? inventory = inventories.FirstOrDefault(i => i.Id == id);
            return inventory;
        }

        public async Task AddInventory(Inventory inventory)
        {
            string now = inventory.GetTimeStamp();
            inventory.CreatedAt = now;
            inventory.UpdatedAt = now;
            this.inventoryProvider.Add(inventory);
            await this.inventoryProvider.Save();
        }

        public async Task ReplaceInventory(Inventory inventory, int inventoryId)
        {
            string now = inventory.GetTimeStamp();
            inventory.UpdatedAt = now;
            this.inventoryProvider.Update(inventory, inventoryId);
            await this.inventoryProvider.Save();
        }

        public async Task DeleteInventory(Inventory inventory)
        {
            this.inventoryProvider.Delete(inventory);
            await this.inventoryProvider.Save();
        }

        public async Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string id)
        {
            if (!int.TryParse(id, out int inventoryId))
            {
                return null!;
            }

            Inventory? inventory = await Task.Run(() => this.inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId));
            if (inventory == null)
            {
                return null!;
            }

            var storageTotals = new Dictionary<string, int>
            {
                { "total_expected", inventory.TotalExpected },
                { "total_ordered", inventory.TotalOrdered },
                { "total_allocated", inventory.TotalAllocated },
                { "total_available", inventory.TotalAvailable },
            };
            return storageTotals;
        }

        public async Task<Inventory?> GetInventoryByItemId(string itemId)
        {
            Inventory[] inventories = await Task.Run(() => this.inventoryProvider.Get());
            Inventory? inventory = inventories.FirstOrDefault(inv => inv.ItemId == itemId);
            return inventory;
        }

        public async Task<Inventory?> GetInventoryByUid(string uid)
        {
            if (!int.TryParse(uid, out int inventoryId))
            {
                return null;
            }

            Inventory? inventory = await Task.Run(() => this.inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId));
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
            this.inventoryProvider.Update(inventory, id);
            await this.inventoryProvider.Save();
        }
    }
}