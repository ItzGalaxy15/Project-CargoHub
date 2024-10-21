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


    public async Task<Dictionary<string, int>> GetItemStorageTotalsByUid(string uid)
    {
        Inventory? inventory = _inventoryProvider.GetByUid(uid);
        if (inventory == null)
        {
            return null;
        }

        var storageTotals = new Dictionary<string, int>
        {
            { "total_expected", inventory.TotalExpected },
            { "total_ordered", inventory.TotalOrdered },
            { "total_allocated", inventory.TotalAllocated },
            { "total_available", inventory.TotalAvailable }
        };
        return storageTotals;
    }

    public async Task<Inventory?> GetInventoryByUid(string uid)
    {
        return _inventoryProvider.GetByUid(uid);
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

    public Item[] GetItemsFromSupplierId(int supplierId)
    {
        Item[] items = _itemProvider.Get();
        Item[] itemsFromSupplier = items
                                    .Where(item => item.SupplierId == supplierId)
                                    .ToArray();
        return itemsFromSupplier;
    }
}