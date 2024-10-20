public interface IItemTypeService
{
    public Task<ItemType[]> GetItemTypes();
    public Task<ItemType?> GetItemTypeById(int id);
    public Task<bool> UpdateItemType(int id, ItemType updatedItemType);

}