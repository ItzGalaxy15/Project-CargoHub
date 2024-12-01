public class ItemGroupProvider : BaseProvider<ItemGroup>, IItemGroupProvider
{
    public ItemGroupProvider(List<ItemGroup> mockData) : base(mockData) { }
    public ItemGroupProvider() : base("test_data/item_groups.json") {}

    public ItemGroup[] Get()
    {
        return context.ToArray();
    }

    public void Add(ItemGroup itemGroup)
    {
        context.Add(itemGroup);
    }

    public void Delete(ItemGroup itemGroup)
    {
        context.Remove(itemGroup);
    }

    public void Update(ItemGroup itemGroup, int itemGroupId)
    {
        itemGroup.Id = itemGroupId;
        int index = context.FindIndex(i => i.Id == itemGroupId);
        context[index] = itemGroup;
    }
}
