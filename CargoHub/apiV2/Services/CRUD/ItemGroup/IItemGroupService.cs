namespace apiV2.Interfaces
{
    public interface IItemGroupService
    {
        public ItemGroup[] GetItemGroups();

        public ItemGroup? GetItemGroupById(int itemGroupId);

        public Task AddItemGroup(ItemGroup itemGroup);

        public Task DeleteItemGroup(ItemGroup itemGroup);

        public Task ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId);

        public Task ModifyItemGroup(int id, Dictionary<string, dynamic> patch, ItemGroup itemGroup);
    }
}