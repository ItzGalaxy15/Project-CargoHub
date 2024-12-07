public class ItemTypeProvider : BaseProvider<ItemType>, IItemTypeProvider
{
    public ItemTypeProvider(List<ItemType> mockData)
        : base(mockData)
    {
    }

    public ItemTypeProvider()
        : base("test_data/item_types.json")
    {
    }

    public ItemType[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(ItemType itemType)
    {
        this.context.Add(itemType);
    }

    public void Delete(ItemType itemType)
    {
        this.context.Remove(itemType);
    }

    public void Update(ItemType itemType, int itemTypeId)
    {
        itemType.Id = itemTypeId;
        int index = this.context.FindIndex(l => l.Id == itemTypeId);
        this.context[index] = itemType;
    }
}
