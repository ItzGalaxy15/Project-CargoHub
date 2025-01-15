namespace apiV2.Interfaces
{
    public interface IInventoryService
    {
        public Inventory[] GetInventories();

        public Inventory? GetInventoryById(int id);

        public Task<Inventory?> GetInventoryByItemId(string itemId);

        public Task AddInventory(Inventory inventory);

        public Task DeleteInventory(Inventory inventory);

        public Task ReplaceInventory(Inventory inventory, int inventoryId);

        public Task<Inventory?> GetInventoryByUid(string uid);

        public Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string uid);

        public Task ModifyInventory(int id, Dictionary<string, dynamic> patch, Inventory inventory);
    }
}