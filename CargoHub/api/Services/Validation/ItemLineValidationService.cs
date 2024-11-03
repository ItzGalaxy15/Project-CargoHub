public class ItemLineValidationService : IItemLineValidationService
{

    private readonly IItemLineProvider _itemLineProvider;
    public ItemLineValidationService(IItemLineProvider itemLineProvider)
    {
        _itemLineProvider = itemLineProvider;
    }

    public bool IsItemLineValid(ItemLine? itemLine, bool update = false)
    {
        if (itemLine is null) return false;
        if (itemLine.Id < 1) return false;

        ItemLine[] itemLines = _itemLineProvider.Get();
        bool itemLineExists = itemLines.Any(i => i.Id == itemLine.Id);
        if (update)
        {
            // Put 
            if (!itemLineExists) return false; 
        }
        else
        {
            // Post
            if (itemLineExists) return false;
        }

        if (string.IsNullOrWhiteSpace(itemLine.Name)) return false;

        return true;
    }   

}