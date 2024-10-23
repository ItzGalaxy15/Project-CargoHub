public class ItemLineService : IItemLineService
{
    private readonly IItemLineProvider _itemLineProvider;
    private readonly IItemProvider _itemProvider;

    public ItemLineService(IItemLineProvider itemLineProvider, IItemProvider itemProvider)
    {
        _itemLineProvider = itemLineProvider;
        _itemProvider = itemProvider;
    }

    public ItemLine[] GetItemLines()
    {
        return _itemLineProvider.Get();
    }

    public ItemLine? GetItemLineById(int id)
    {
        return _itemLineProvider.GetById(id);
    }

    public Item[] GetItemsByItemLineId(int itemLineId)
    {
        return _itemProvider.Get().Where(item => item.ItemLine == itemLineId).ToArray();
    }

    public bool ReplaceItemLine(int id, ItemLine newItemLine)
    {
        return _itemLineProvider.ReplaceItemLine(id, newItemLine);
    }
}