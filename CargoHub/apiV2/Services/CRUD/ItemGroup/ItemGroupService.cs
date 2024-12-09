using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
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
            _itemGroupProvider.Update(itemGroup, itemGroupId);
            await _itemGroupProvider.Save();

        }

        public async Task DeleteItemGroup(ItemGroup itemGroup)
        {
            _itemGroupProvider.Delete(itemGroup);
            await _itemGroupProvider.Save();
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
            _itemGroupProvider.Update(itemGroup, id);
            await _itemGroupProvider.Save();
        }
    }
}