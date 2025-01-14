public class InventoryProvider : BaseProvider<Inventory>, IInventoryProvider
{
    public InventoryProvider(List<Inventory> mockData)
        : base(mockData)
    {
    }

    public InventoryProvider()
        : base("test_data/inventories.json")
    {
    }

    public Inventory[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Inventory inventory)
    {
        this.context.Add(inventory);
    }

    public void Delete(Inventory inventory)
    {
        inventory.IsDeleted = true;
        inventory.UpdatedAt = inventory.GetTimeStamp();
    }

    public void Update(Inventory inventory, int inventoryId)
    {
        inventory.Id = inventoryId;
        int index = this.context.FindIndex(i => i.Id == inventoryId);
        this.context[index] = inventory;
    }
}