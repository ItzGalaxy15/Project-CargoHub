public class SupplierProvider : BaseProvider<Supplier>, ISupplierProvider
{
    public SupplierProvider(List<Supplier> mockData)
        : base(mockData)
    {
    }

    public SupplierProvider()
        : base("test_data/suppliers.json")
    {
    }

    public Supplier[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Supplier supplier)
    {
        this.context.Add(supplier);
    }

    public void Delete(Supplier supplier)
    {
        this.context.Remove(supplier);
    }

    public void Update(Supplier supplier, int supplierId)
    {
        supplier.Id = supplierId;
        int index = this.context.FindIndex(sup => sup.Id == supplierId);
        this.context[index] = supplier;
    }
}
