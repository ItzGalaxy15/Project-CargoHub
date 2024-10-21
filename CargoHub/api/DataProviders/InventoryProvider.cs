public class InventoryProvider : BaseProvider<Inventory>, IInventoryProvider
{
    public InventoryProvider() : base("test_data/inventories.json"){}

    public Inventory[] Get()
    {
        return context.ToArray();
    }

    public Inventory? GetByUid(string uid)
    {
        return context.FirstOrDefault(i => i.ItemId == uid);
    }
}