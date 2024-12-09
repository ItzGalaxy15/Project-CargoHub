namespace apiV1.Interfaces
{
    public interface IItemService
    {
        Item[] GetItems();

        Item? GetItemById(string uid);

        Task<Dictionary<string, int>> GetItemTotalsByUid(string id);

        Task AddItem(Item item);

        Task DeleteItem(Item item);

        Task UpdateItem(Item item, string uid);

        Item[] GetItemsFromItemLines(int itemLineId);

        Item[] GetItemsFromSupplierId(int id);

        Item[] GetItemsForItemGroups(int itemGroupId);
    }
}