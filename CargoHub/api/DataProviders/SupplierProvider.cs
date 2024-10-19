public class SupplierProvider : BaseProvider<Supplier>, ISupplierProvider
{
    public SupplierProvider() : base("test_data/suppliers.json"){}

    public Supplier[] Get(){
        return context.ToArray();
    }
}
