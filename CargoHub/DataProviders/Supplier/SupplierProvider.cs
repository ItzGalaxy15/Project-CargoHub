public class SupplierProvider : BaseProvider<Supplier>, ISupplierProvider
{
    public SupplierProvider(List<Supplier> mockData)
        : base(mockData)
    {
    }

    public SupplierProvider()
        : base("data/suppliers.json")
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
        supplier.IsDeleted = true;
        supplier.UpdatedAt = supplier.GetTimeStamp();
    }

    public void Update(Supplier supplier, int supplierId)
    {
        supplier.Id = supplierId;
        int index = this.context.FindIndex(sup => sup.Id == supplierId);
        this.context[index] = supplier;
    }
}
