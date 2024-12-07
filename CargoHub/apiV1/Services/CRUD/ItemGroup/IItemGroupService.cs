namespace apiV1.Interfaces
{
    public interface IItemGroupService
    {
        public ItemGroup[] GetItemGroups();

        public ItemGroup? GetItemGroupById(int itemGroupId);

        public Task AddItemGroup(ItemGroup itemGroup);

        public Task DeleteItemGroup(ItemGroup itemGroup);

        public Task ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId);
    }
}