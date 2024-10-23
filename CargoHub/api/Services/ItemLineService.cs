public class ItemLineService : IItemLineService
{
    private readonly IItemLineProvider _itemLineProvider;

    public ItemLineService(IItemLineProvider itemLineProvider)
    {
        _itemLineProvider = itemLineProvider;
    }

    public ItemLine[] GetItemLines()
    {
        return _itemLineProvider.Get();
    }

    public ItemLine? GetItemLineById(int id)
    {
        return _itemLineProvider.GetById(id);
    }
}