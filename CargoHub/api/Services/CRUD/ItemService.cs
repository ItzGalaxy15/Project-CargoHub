public class ItemService : IItemService
{
    private readonly IItemProvider _itemProvider;
    private readonly IInventoryProvider _inventoryProvider;

    public ItemService(IItemProvider itemProvider, IInventoryProvider inventoryProvider)
    {
        _itemProvider = itemProvider;
        _inventoryProvider = inventoryProvider;
    }

    public async Task<bool> AddItem(Item item)
    {
        Item[] items = _itemProvider.Get();
        if (items.Any(i => i.Uid == item.Uid))
        {
            return false;
        }
        // date is valid
        string now = item.GetTimeStamp();
        item.UpdatedAt = now;
        item.CreatedAt = now;

        _itemProvider.Add(item);
        await _itemProvider.Save();

        return true;
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

    public async Task<Dictionary<string, int>> GetItemTotalsByUid(string uid)
    {
        return _itemProvider.GetItemTotalsByUid(uid);
    }

    public async Task DeleteItem(Item item)
    {
        _itemProvider.Delete(item);
        await _itemProvider.Save();
    }

    public async Task<bool> ReplaceItem(Item item)
    {
        Item[] items = _itemProvider.Get();
        if (!items.Any(i => i.Uid == item.Uid) || !_itemProvider.Replace(item))
        {
            return false;
        }

        string now = item.GetTimeStamp();
        item.UpdatedAt = now;

        _itemProvider.Replace(item);
        await _itemProvider.Save();

        return true;
    }


    public Item[] GetItemsFromItemLines(int itemLineId)
    {
        Item[] items = _itemProvider.Get()
                        .Where(i => i.ItemLine == itemLineId)
                        .ToArray();
        Console.WriteLine($"Items for ItemLine {itemLineId}: {items.Length}");
        return items;
    }
    

    public Item[] GetItemsFromSupplierId(int supplierId)
    {
        Item[] items = _itemProvider.Get();
        Item[] itemsFromSupplier = items
                                    .Where(item => item.SupplierId == supplierId)
                                    .ToArray();
        return itemsFromSupplier;
    }

    public Item[] GetItemsForItemGroups(int itemGroupId)
    {
        Item[] items = _itemProvider.Get()
                        .Where(i => i.ItemGroup == itemGroupId)
                        .ToArray();
        return items;
    }

}