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

    public Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        throw new NotImplementedException();
    }

    public Task DeleteItemGroup(ItemGroup itemGroup)
    {
        throw new NotImplementedException();
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
}