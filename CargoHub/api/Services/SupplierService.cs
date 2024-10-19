public class SupplierService : ISupplierService
{
    private readonly ISupplierProvider _supplierProvider;
    public SupplierService(ISupplierProvider supplierProvider){
        _supplierProvider = supplierProvider;
    }

    public Supplier[] GetSuppliers(){
        return _supplierProvider.Get();
    }

}
