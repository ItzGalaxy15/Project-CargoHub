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

    public bool ReplaceItemLine(int id, ItemLine newItemLine)
    {
        int index = context.FindIndex(il => il.Id == id);
        if (index == -1) return false;
        context[index] = newItemLine;
        return true;
    }
}