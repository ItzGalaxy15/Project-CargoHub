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
    public Task<bool> AddInventory(Inventory inventory)
    {
        throw new NotImplementedException();
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