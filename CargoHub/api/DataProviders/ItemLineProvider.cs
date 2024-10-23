public class ItemLineProvider : BaseProvider<ItemLine>, IItemLineProvider
{
    public ItemLineProvider() : base("test_data/item_lines.json"){}

    public ItemLine[] Get()
    {
        return context.ToArray();
    }
}