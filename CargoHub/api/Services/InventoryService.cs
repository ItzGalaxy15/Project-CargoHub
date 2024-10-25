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
    public async Task<bool> AddInventory(Inventory inventory)
    {
        // Check if inventory is valid
        
        // Check if inventory id is already in use
        Inventory[] inventories = GetInventories();
        if (inventories.Any(i => i.Id == inventory.Id)) return false;

        string now = inventory.GetTimeStamp();
        inventory.CreatedAt = now;
        inventory.UpdatedAt = now;
        _inventoryProvider.Add(inventory);
        await _inventoryProvider.Save();
        return true;
    }

    public async Task<bool> ReplaceInventory(Inventory inventory, int inventoryId)
    {
        // check if inventory is valid
        //


        string now = inventory.GetTimeStamp();
        inventory.UpdatedAt = now;

        // will return false if there is no inventory with the same id
        if (!_inventoryProvider.Replace(inventory, inventoryId)) return false;
        await _inventoryProvider.Save();
        return true;
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
            return null;
        }
        Inventory? inventory = _inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId);
        if (inventory == null)
        {
            return null;
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
        return _inventoryProvider.Get().FirstOrDefault(i => i.Id == inventoryId);
    }
}