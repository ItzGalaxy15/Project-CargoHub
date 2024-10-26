public class ItemTypeService : IItemTypeService
{
    private readonly IItemTypeProvider _itemTypeProvider;

    public ItemTypeService(IItemTypeProvider itemTypeProvider)
    {
        _itemTypeProvider = itemTypeProvider;
    }

    public async Task<ItemType[]> GetItemTypes()
    {
        ItemType[] itemTypes = _itemTypeProvider.Get();
        return await Task.FromResult(itemTypes.ToArray());
    }

    public async Task<ItemType?> GetItemTypeById(int id)
    {
        ItemType[] itemTypes = _itemTypeProvider.Get();
        ItemType? itemType = await
            Task.FromResult(itemTypes.FirstOrDefault(i => i.Id == id));
        return itemType;
    }

    public async Task<bool> UpdateItemType(int id, ItemType updatedItemType)
    {
        updatedItemType.UpdatedAt = updatedItemType.GetTimeStamp();
        if (!_itemTypeProvider.Update(updatedItemType, id)) return false;
        await _itemTypeProvider.Save();
        return true;
    }
    
    public async Task DeleteItemType(ItemType itemType)
    {
        _itemTypeProvider.Delete(itemType);
        await _itemTypeProvider.Save();
    }

    public async Task AddItemType(ItemType itemType)
    {
        itemType.CreatedAt = itemType.GetTimeStamp();
        itemType.UpdatedAt = itemType.GetTimeStamp();
        _itemTypeProvider.Add(itemType);
        await _itemTypeProvider.Save();
    }
}
