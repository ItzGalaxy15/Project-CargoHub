public class ItemLineProvider : BaseProvider<ItemLine>, IItemLineProvider
{
    public ItemLineProvider(List<ItemLine> mockData)
        : base(mockData)
    {
    }

    public ItemLineProvider()
        : base("data/item_lines.json")
    {
    }

    public void Add(ItemLine itemLine)
    {
        this.context.Add(itemLine);
    }

    public ItemLine[] Get()
    {
        return this.context.ToArray();
    }

    public void Update(int id, ItemLine itemLine)
    {
        int index = this.context.FindIndex(il => il.Id == id);
        this.context[index] = itemLine;
    }

    public void Delete(ItemLine itemLine)
    {
        itemLine.IsDeleted = true;
        itemLine.UpdatedAt = itemLine.GetTimeStamp();
    }
}