public class ItemTypeProvider : BaseProvider<ItemType>, IItemTypeProvider
{
    public ItemTypeProvider() : base("test_data/item_types.json") {}

    public ItemType[] Get(){
        return context.ToArray();
    }

    public void Add(ItemType itemType){
        context.Add(itemType);
    }
    public void Delete(int id){
        context.RemoveAt(id);
    }
}
