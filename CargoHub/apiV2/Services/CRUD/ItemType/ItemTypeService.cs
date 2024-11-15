using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class ItemTypeService : IItemTypeService
    {
        private readonly IItemTypeProvider _itemTypeProvider;

        public ItemTypeService(IItemTypeProvider itemTypeProvider)
        {
            _itemTypeProvider = itemTypeProvider;
        }

        public async Task<ItemType[]> GetItemTypes()
        {
            ItemType[] itemTypes = _itemTypeProvider.Get();
            return await Task.FromResult(itemTypes.ToArray());
        }

        public async Task<ItemType?> GetItemTypeById(int id)
        {
            ItemType[] itemTypes = _itemTypeProvider.Get();
            ItemType? itemType = await
                Task.FromResult(itemTypes.FirstOrDefault(i => i.Id == id));
            return itemType;
        }

        public async Task UpdateItemType(int id, ItemType updatedItemType)
        {
            updatedItemType.UpdatedAt = updatedItemType.GetTimeStamp();
            _itemTypeProvider.Update(updatedItemType, id);
            await _itemTypeProvider.Save();
        }
        
        public async Task DeleteItemType(ItemType itemType)
        {
            _itemTypeProvider.Delete(itemType);
            await _itemTypeProvider.Save();
        }

        public async Task AddItemType(ItemType itemType)
        {
            itemType.CreatedAt = itemType.GetTimeStamp();
            itemType.UpdatedAt = itemType.GetTimeStamp();
            _itemTypeProvider.Add(itemType);
            await _itemTypeProvider.Save();
        }

        public async Task PatchItemType(int id, Dictionary<string, object> patch, ItemType itemType)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "name":
                            itemType.Name = jsonElement.GetString()!;
                            break;
                        case "description":
                            itemType.Description = jsonElement.GetString()!;
                            break;
                    }
                }
            }
            itemType.UpdatedAt = itemType.GetTimeStamp();
            _itemTypeProvider.Update(itemType, id);
            await _itemTypeProvider.Save();
        }
    }
}
