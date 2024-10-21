public interface IItemService
{
    Item[] GetItems();
    
    Item? GetItemById(string uid);

    Task<Dictionary<string, int>> GetItemTotalsByUid(string uid);

    Task<Dictionary<string, int>> GetItemStorageByUid(string uid);

    Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string uid);

    Task<bool> AddItem(Item item);

    Task DeleteItem(Item item);

    Task<bool> ReplaceItem(Item item);

    Item[] GetItemsFromSupplierId(int id);

}