public class ItemService : IItemService
{
    private readonly IItemProvider _itemProvider;

    public ItemService(IItemProvider itemProvider)
    {
        _itemProvider = itemProvider;
    }

    public Item[] GetItems()
    {
        return _itemProvider.Get();
    }

    public Item? GetItemById(string uid)
    {
        Item[] items = _itemProvider.Get();
        return _itemProvider.Get().FirstOrDefault(i => i.Uid == uid);
    }
}