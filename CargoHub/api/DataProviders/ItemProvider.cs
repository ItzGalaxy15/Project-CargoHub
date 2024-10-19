public class ItemProvider : BaseProvider<Item>, IItemProvider
{
    public ItemProvider() : base("test_data/items.json"){}

    public Item[] Get(){
        return context.ToArray();
    }

    public void Add(Item item){
        context.Add(item);
    }
}