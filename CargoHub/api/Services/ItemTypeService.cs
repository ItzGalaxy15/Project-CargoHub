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
}
