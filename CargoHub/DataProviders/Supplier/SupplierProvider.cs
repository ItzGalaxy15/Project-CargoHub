public class SupplierProvider : BaseProvider<Supplier>, ISupplierProvider
{
    public SupplierProvider(List<Supplier> mockData) : base(mockData) { }
    public SupplierProvider() : base("test_data/suppliers.json"){}

    public Supplier[] Get(){
        return context.ToArray();
    }
    
    public void Add(Supplier supplier){
        context.Add(supplier);
    }

    public void Delete(Supplier supplier){
        context.Remove(supplier);
    }

    public void Update(Supplier supplier, int supplierId)
    {
        supplier.Id = supplierId;
        int index = context.FindIndex(sup => sup.Id == supplierId);
        context[index] = supplier;
    }
}
