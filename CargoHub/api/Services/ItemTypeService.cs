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
        ItemType[] itemTypes = _itemTypeProvider.Get();
        updatedItemType.Id = id;
        updatedItemType.UpdatedAt = updatedItemType.GetTimeStamp();
        bool check = false;
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i].Id == id)
            {
                updatedItemType.CreatedAt = itemTypes[i].CreatedAt;
                _itemTypeProvider.context[i] = updatedItemType;
                check = true;
            }
        }
        await _itemTypeProvider.Save();
        return check;
    }
    
    public async Task<bool> DeleteItemType(int id)
    {
        ItemType[] itemTypes = _itemTypeProvider.Get();
        bool check = false;
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i].Id == id)
            {
                _itemTypeProvider.Delete(i);
                check = true;
            }
        }
        await _itemTypeProvider.Save();
        return check;
    }
}
