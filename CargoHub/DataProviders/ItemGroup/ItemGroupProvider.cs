public class ItemGroupProvider : BaseProvider<ItemGroup>, IItemGroupProvider
{
    public ItemGroupProvider(List<ItemGroup> mockData)
        : base(mockData)
    {
    }

    public ItemGroupProvider()
        : base("test_data/item_groups.json")
    {
    }

    public ItemGroup[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(ItemGroup itemGroup)
    {
        this.context.Add(itemGroup);
    }

    public void Delete(ItemGroup itemGroup)
    {
        itemGroup.IsDeleted = true;
        itemGroup.UpdatedAt = itemGroup.GetTimeStamp();
    }

    public void Update(ItemGroup itemGroup, int itemGroupId)
    {
        itemGroup.Id = itemGroupId;
        int index = this.context.FindIndex(i => i.Id == itemGroupId);
        this.context[index] = itemGroup;
    }
}
