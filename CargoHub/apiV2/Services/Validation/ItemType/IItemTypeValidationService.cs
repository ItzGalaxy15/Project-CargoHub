namespace apiV2.ValidationInterfaces
{
    public interface IItemTypeValidationService
    {
        Task<bool> IsItemTypeValidForPOST(ItemType newItemType);
        Task<bool> IsItemTypeValidForPUT(ItemType updatedItemType, int itemTypeId);
    }
}