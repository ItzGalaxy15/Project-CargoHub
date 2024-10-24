public class InventoryProvider : BaseProvider<Inventory>, IInventoryProvider
{
    public InventoryProvider() : base("test_data/inventories.json"){}

    public Inventory[] Get()
    {
        return context.ToArray();
    }
    public void Add(Inventory inventory)
    {
        throw new NotImplementedException();
    }

    public void Delete(Inventory inventory)
    {
        throw new NotImplementedException();
    }

    public bool Replace(Inventory inventory, int inventoryId)
    {
        int index = context.FindIndex(i => i.Id == inventoryId);
        if (index == -1) return false;
        context[index] = inventory;
        return true;
    }

    public Inventory? GetByUid(string uid)
    {
        return context.FirstOrDefault(i => i.ItemId == uid);
    }
}