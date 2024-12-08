public class ItemLineProvider : BaseProvider<ItemLine>, IItemLineProvider
{
    public ItemLineProvider(List<ItemLine> mockData) : base(mockData) { }
    public ItemLineProvider() : base("test_data/item_lines.json"){}

    public void Add(ItemLine itemLine)
    {
        context.Add(itemLine);
    }
    public ItemLine[] Get()
    {
        return context.ToArray();
    }

    public void Update(int id, ItemLine itemLine)
    {
        itemLine.Id = id;
        int index = context.FindIndex(il => il.Id == id);
        context[index] = itemLine;
    }

    public void Delete(ItemLine itemLine)
    {
        context.Remove(itemLine);
    }

    
}