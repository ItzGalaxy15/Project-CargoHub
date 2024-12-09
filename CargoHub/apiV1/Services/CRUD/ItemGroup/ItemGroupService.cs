using apiV1.Interfaces;

namespace apiV1.Services
{
    public class ItemGroupService : IItemGroupService
    {
        private readonly IItemGroupProvider itemGroupProvider;

        public ItemGroupService(IItemGroupProvider itemGroupProvider)
        {
            this.itemGroupProvider = itemGroupProvider;
        }

        public ItemGroup[] GetItemGroups()
        {
            return this.itemGroupProvider.Get();
        }

        public ItemGroup? GetItemGroupById(int itemGroupId)
        {
            ItemGroup[] itemGroups = this.GetItemGroups();
            ItemGroup? itemGroup = itemGroups.FirstOrDefault(i => i.Id == itemGroupId);
            return itemGroup;
        }

        public async Task AddItemGroup(ItemGroup itemGroup)
        {
            string now = itemGroup.GetTimeStamp();
            itemGroup.CreatedAt = now;
            itemGroup.UpdatedAt = now;
            this.itemGroupProvider.Add(itemGroup);
            await this.itemGroupProvider.Save();
        }

        public async Task ReplaceItemGroup(ItemGroup itemGroup, int itemGroupId)
        {
            string now = itemGroup.GetTimeStamp();
            itemGroup.UpdatedAt = now;
            this.itemGroupProvider.Update(itemGroup, itemGroupId);
            await this.itemGroupProvider.Save();
        }

        public async Task DeleteItemGroup(ItemGroup itemGroup)
        {
            this.itemGroupProvider.Delete(itemGroup);
            await this.itemGroupProvider.Save();
        }
    }
}