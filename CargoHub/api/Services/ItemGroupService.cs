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

    public Warehouse? GetItemGroupById(int itemGroupId)
    {
        throw new NotImplementedException();
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

    ItemGroup? IItemGroupService.GetItemGroupById(int itemGroupId)
    {
        throw new NotImplementedException();
    }
}