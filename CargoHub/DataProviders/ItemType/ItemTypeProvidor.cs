public class ItemTypeProvider : BaseProvider<ItemType>, IItemTypeProvider
{
    public ItemTypeProvider(List<ItemType> mockData) : base(mockData) { }   
    public ItemTypeProvider() : base("test_data/item_types.json") {}

    public ItemType[] Get(){
        return context.ToArray();
    }

    public void Add(ItemType itemType){
        context.Add(itemType);
    }
    public void Delete(ItemType itemType){
        context.Remove(itemType);
    }

    public void Update(ItemType itemType, int itemTypeId)
    {
        itemType.Id = itemTypeId;
        int index = context.FindIndex(l => l.Id == itemTypeId);
        context[index] = itemType;
    }
}
