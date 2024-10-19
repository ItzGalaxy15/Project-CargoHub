public interface IItemService
{
    public Item[] GetItems();

    public Item? GetItemById(string uid);

    public Task<bool> AddItem(Item item);

    public Task DeleteItem(Item item);
}