public interface IItemGroupService
{
    public ItemGroup[] GetItemGroups();
    public ItemGroup? GetItemGroupById(int itemGroupId);
    // public Task<bool> AddItemGroup(ItemGroup itemGroup);
    public Task DeleteItemGroup(ItemGroup itemGroup);
    public Task<bool> ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId);
}