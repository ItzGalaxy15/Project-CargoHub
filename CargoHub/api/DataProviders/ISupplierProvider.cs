public interface ISupplierProvider
{
    public List<Supplier> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Supplier[] Get();
}
