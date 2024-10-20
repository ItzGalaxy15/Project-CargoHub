public class ItemTypeProvider : BaseProvider<ItemType>, IItemTypeProvider
{
    public ItemTypeProvider() : base("data/item_types.json") {}

    public ItemType[] Get(){
        return context.ToArray();
    }

    public void Delete(int id){
        context.RemoveAt(id);
    }
}
