namespace apiV2.Interfaces
{
    public interface IItemTypeService
    {
        public Task<ItemType[]> GetItemTypes();
        public Task<ItemType?> GetItemTypeById(int id);
        public Task UpdateItemType(int id, ItemType updatedItemType);
        public Task DeleteItemType(ItemType itemType);
        public Task AddItemType(ItemType itemType);
        public Task PatchItemType(int id, Dictionary<string, object> patch, ItemType itemType);

    }
}