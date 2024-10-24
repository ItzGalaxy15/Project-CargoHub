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

    public Task<bool> ReplaceInventory(Inventory inventory, int inventoryId)
    {
        throw new NotImplementedException();
    }
    public Task DeleteInventory(Inventory inventory)
    {
        throw new NotImplementedException();
    }

}