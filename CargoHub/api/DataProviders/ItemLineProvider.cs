public class ItemLineProvider : BaseProvider<ItemLine>, IItemLineProvider
{
    public ItemLineProvider() : base("test_data/item_lines.json"){}

    public ItemLine[] Get()
    {
        return context.ToArray();
    }

    public ItemLine? GetById(int id)
    {
        return context.FirstOrDefault(il => il.Id == id);
    }
}