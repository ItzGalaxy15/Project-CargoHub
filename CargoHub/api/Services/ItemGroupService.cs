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

    public Task<bool> ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId)
    {
        throw new NotImplementedException();
    }
}