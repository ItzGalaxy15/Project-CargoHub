public interface IItemService
{
    Item[] GetItems();
    
    Item? GetItemById(string uid);

    Task<Dictionary<string, int>> GetItemTotalsByUid(string id);


    Task AddItem(Item item);

    Task DeleteItem(Item item);

    Task ReplaceItem(Item item);

    Item[] GetItemsFromItemLines(int itemLineId);

    Item[] GetItemsFromSupplierId(int id);

    Item[] GetItemsForItemGroups(int itemGroupId);


}