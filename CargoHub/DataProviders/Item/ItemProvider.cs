public class ItemProvider : BaseProvider<Item>, IItemProvider
{
    public ItemProvider(List<Item> mockData) : base(mockData) { }
    public ItemProvider() : base("test_data/items.json"){}

    public Item[] Get(){
        return context.ToArray();
    }

    public void Add(Item item){
        context.Add(item);
    }

    public void Delete(Item item){
        context.Remove(item);
    }

    public void Update(Item item, string uid)
    {
        item.Uid = uid;
        int index = context.FindIndex(item => item.Uid == uid);
        context[index] = item;
    }


    public Dictionary<string, int> GetItemTotalsByUid(string uid)
    {
        Item? item = context.FirstOrDefault(i => i.Uid == uid);
        if (item == null)
        {
            return null!;
        }
        var totals = new Dictionary<string, int>
        {
            { "unit_purchase_quantity", item.UnitPurchaseQuantity },
            { "unit_order_quantity", item.UnitOrderQuantity },
            { "pack_order_quantity", item.PackOrderQuantity }
        };
        return totals;
    }
}