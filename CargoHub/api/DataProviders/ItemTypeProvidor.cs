public class ItemTypeProvider : BaseProvider<ItemType>, IItemTypeProvider
{
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

    public bool Update(ItemType itemType, int itemTypeId)
    {
        int index = context.FindIndex(l => l.Id == itemTypeId);
        if (index == -1) return false;
        context[index] = itemType;
        return true;
    }
}
