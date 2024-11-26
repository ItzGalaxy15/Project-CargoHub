public interface IInventoryProvider
{
    public List<Inventory> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Inventory[] Get();
    public void Add(Inventory inventory);
    public void Delete(Inventory inventory);
    public void Update(Inventory inventory, int inventoryId);
}
