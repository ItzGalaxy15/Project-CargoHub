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

    public async Task AddItemGroup(ItemGroup itemGroup)
    {
        string now = itemGroup.GetTimeStamp();
        itemGroup.CreatedAt = now;
        itemGroup.UpdatedAt = now;
        _itemGroupProvider.Add(itemGroup);
        await _itemGroupProvider.Save();
    }

    public async Task ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId)
    {
        string now = itemGroup.GetTimeStamp();
        itemGroup.UpdatedAt = now;
        _itemGroupProvider.Replace(itemGroup, itemGroupId);
        await _itemGroupProvider.Save();

    }

    public async Task DeleteItemGroup(ItemGroup itemGroup)
    {
        _itemGroupProvider.Delete(itemGroup);
        await _itemGroupProvider.Save();
    }
}