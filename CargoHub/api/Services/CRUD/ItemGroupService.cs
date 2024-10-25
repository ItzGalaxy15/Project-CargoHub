public class ItemGroupService : IItemGroupService
{
    private readonly IItemGroupProvider _itemGroupProvider;
    public ItemGroupService(IItemGroupProvider itemGroupProvider)
    {
        _itemGroupProvider = itemGroupProvider;
    }

    public ItemGroup[] GetItemGroups()
    {
        return _itemGroupProvider.Get();
    }

    public ItemGroup? GetItemGroupById(int itemGroupId)
    {
        ItemGroup[] itemGroups = GetItemGroups();
        ItemGroup? itemGroup = itemGroups.FirstOrDefault(i => i.Id == itemGroupId);
        return itemGroup;
    }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        // Check if itemGroup is valid

        // Check if itemGroup id is already in use
        ItemGroup[] itemGroups = GetItemGroups();
        if (itemGroups.Any(w => w.Id == itemGroup.Id)) return false;

        string now = itemGroup.GetTimeStamp();
        itemGroup.CreatedAt = now;
        itemGroup.UpdatedAt = now;
        _itemGroupProvider.Add(itemGroup);
        await _itemGroupProvider.Save();
        return true;
    }

    public async Task<bool> ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId)
    {
        // check if itemGroup is valid
        //


        string now = itemGroup.GetTimeStamp();
        itemGroup.UpdatedAt = now;

        // will return false if there is no itemGroup with the same id
        if (!_itemGroupProvider.Replace(itemGroup, itemGroupId)) return false;
        await _itemGroupProvider.Save();
        return true;
    }

    public async Task DeleteItemGroup(ItemGroup itemGroup)
    {
        _itemGroupProvider.Delete(itemGroup);
        await _itemGroupProvider.Save();
    }
}