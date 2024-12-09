using apiV1.Interfaces;

namespace apiV1.Services
{
    public class ItemTypeService : IItemTypeService
    {
        private readonly IItemTypeProvider itemTypeProvider;

        public ItemTypeService(IItemTypeProvider itemTypeProvider)
        {
            this.itemTypeProvider = itemTypeProvider;
        }

        public async Task<ItemType[]> GetItemTypes()
        {
            ItemType[] itemTypes = this.itemTypeProvider.Get();
            return await Task.FromResult(itemTypes.ToArray());
        }

        public async Task<ItemType?> GetItemTypeById(int id)
        {
            ItemType[] itemTypes = this.itemTypeProvider.Get();
            ItemType? itemType = await
                Task.FromResult(itemTypes.FirstOrDefault(i => i.Id == id));
            return itemType;
        }

        public async Task UpdateItemType(int id, ItemType updatedItemType)
        {
            updatedItemType.UpdatedAt = updatedItemType.GetTimeStamp();
            this.itemTypeProvider.Update(updatedItemType, id);
            await this.itemTypeProvider.Save();
        }

        public async Task DeleteItemType(ItemType itemType)
        {
            this.itemTypeProvider.Delete(itemType);
            await this.itemTypeProvider.Save();
        }

        public async Task AddItemType(ItemType itemType)
        {
            itemType.CreatedAt = itemType.GetTimeStamp();
            itemType.UpdatedAt = itemType.GetTimeStamp();
            this.itemTypeProvider.Add(itemType);
            await this.itemTypeProvider.Save();
        }
    }
}
