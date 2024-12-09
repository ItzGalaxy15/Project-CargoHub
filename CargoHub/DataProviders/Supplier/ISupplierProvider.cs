public interface ISupplierProvider
{
    public List<Supplier> context { get; set; }
    public string? path { get; set; }
    public Task Save();
    public Supplier[] Get();
    public void Add(Supplier supplier);
    public void Delete(Supplier supplier);
    public void Update(Supplier supplier, int supplierId);
}
