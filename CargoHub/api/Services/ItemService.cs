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
}