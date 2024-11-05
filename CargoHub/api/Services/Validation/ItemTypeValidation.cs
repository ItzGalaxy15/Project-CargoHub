public class ItemTypeValidation : IItemTypeValidation
{
    private readonly IItemTypeProvider _itemTypeProvider;
    public ItemTypeValidation(IItemTypeProvider itemTypeProvider){
        _itemTypeProvider = itemTypeProvider;
    }

    public async Task<bool> IsItemTypeValidForPOST(ItemType newItemType){
        if (newItemType == null) return false;
        if (newItemType.Id <= 0) return false;
        ItemType[] itemTypes = _itemTypeProvider.Get();
        ItemType? itemType = await Task.FromResult(itemTypes.FirstOrDefault(it => it.Id == newItemType.Id));
        if (itemType != null) return false;
        // if (string.IsNullOrWhiteSpace(newItemType.Name)) return false;
        return true;
    }

    public async Task<bool> IsItemTypeValidForPUT(ItemType updatedItemType, int itemTypeId)
    {
        if (updatedItemType == null) return false;
        if (updatedItemType.Id < 0) return false;
        ItemType[] itemTypes = _itemTypeProvider.Get();
        ItemType? itemType = await Task.FromResult(itemTypes.FirstOrDefault(it => it.Id == updatedItemType.Id));
        int index = itemTypes.ToList().FindIndex(it => it.Id == itemTypeId);
        if (index == -1) return false;
        if (itemType == null) return false;
        // if (string.IsNullOrWhiteSpace(updatedItemType.Name)) return false;
        return true;
    }
}