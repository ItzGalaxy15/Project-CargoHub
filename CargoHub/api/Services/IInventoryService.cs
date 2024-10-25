public interface IInventoryService
{
    public Inventory[] GetInventories();
    public Inventory? GetInventoryById(int id);
    public Task<bool> AddInventory(Inventory inventory);
    public Task DeleteInventory(Inventory inventory);
    public Task<bool> ReplaceInventory(Inventory inventory, int inventoryId);

    public Task<Inventory?> GetInventoryByUid(string uid);

    public Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string uid);


}