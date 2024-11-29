public class InventoryProvider : BaseProvider<Inventory>, IInventoryProvider
{
    public InventoryProvider() : base("test_data/inventories.json"){}

    public Inventory[] Get()
    {
        return context.ToArray();
    }
    public void Add(Inventory inventory)
    {
        context.Add(inventory);
    }

    public void Delete(Inventory inventory)
    {
        context.Remove(inventory);
    }

    public void Update(Inventory inventory, int inventoryId)
    {
        inventory.Id = inventoryId;
        int index = context.FindIndex(i => i.Id == inventoryId);
        context[index] = inventory;
    }
}