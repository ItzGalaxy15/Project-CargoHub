using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.TagHelpers;

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
}