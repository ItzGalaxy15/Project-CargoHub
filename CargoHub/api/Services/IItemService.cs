public interface IItemService
{
    public Item[] GetItems();

    public Item? GetItemById(string uid);
}