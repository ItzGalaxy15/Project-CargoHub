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
}
