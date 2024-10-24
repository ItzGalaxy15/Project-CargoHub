public interface IInventoryService
{
    public Inventory[] GetInventories();
    public Inventory? GetInventoryById(int id);
    public Task<bool> AddInventory(Inventory inventory);
    public Task DeleteInventory(Inventory inventory);
    public Task<bool> ReplaceInventory(Inventory inventory, int inventoryId);
}
