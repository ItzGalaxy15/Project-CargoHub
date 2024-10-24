public interface IInventoryProvider
{
    public Inventory? GetByUid(string uid);
    public List<Inventory> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Inventory[] Get();
    public void Add(Inventory inventory);
    public void Delete(Inventory inventory);
    public bool Replace(Inventory inventory, int inventoryId);
}
