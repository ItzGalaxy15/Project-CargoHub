public class ItemProvider : BaseProvider<Item>, IItemProvider
{
    public ItemProvider(List<Item> mockData)
        : base(mockData)
    {
    }

    public ItemProvider()
        : base("test_data/items.json")
    {
    }

    public Item[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Item item)
    {
        this.context.Add(item);
    }

    public void Delete(Item item)
    {
        this.context.Remove(item);
    }

    public void Update(Item item, string uid)
    {
        item.Uid = uid;
        int index = this.context.FindIndex(item => item.Uid == uid);
        this.context[index] = item;
    }

    public Dictionary<string, int> GetItemTotalsByUid(string uid)
    {
        Item? item = this.context.FirstOrDefault(i => i.Uid == uid);
        if (item == null)
        {
            return null!;
        }

        var totals = new Dictionary<string, int>
        {
            { "unit_purchase_quantity", item.UnitPurchaseQuantity },
            { "unit_order_quantity", item.UnitOrderQuantity },
            { "pack_order_quantity", item.PackOrderQuantity },
        };
        return totals;
    }
}