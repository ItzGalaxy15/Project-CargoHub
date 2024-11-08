public interface IItemTypeService
{
    public Task<ItemType[]> GetItemTypes();
    public Task<ItemType?> GetItemTypeById(int id);
    public Task UpdateItemType(int id, ItemType updatedItemType);
    public Task DeleteItemType(ItemType itemType);
    public Task AddItemType(ItemType itemType);

}