using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
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

        public async Task ModifyItemGroup(int id, Dictionary<string, dynamic> patch, ItemGroup itemGroup)
        {
            foreach (var (key, value) in patch)
            {
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "name":
                            itemGroup.Name = jsonElement.GetString()!;
                            break;

                        case "description":
                            itemGroup.Description = jsonElement.GetString()!;
                            break;
                    }
                }
            }

            itemGroup.UpdatedAt = itemGroup.GetTimeStamp();
            this.itemGroupProvider.Update(itemGroup, id);
            await this.itemGroupProvider.Save();
        }
    }
}