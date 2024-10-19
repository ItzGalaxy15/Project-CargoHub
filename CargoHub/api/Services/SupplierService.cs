public class SupplierService : ISupplierService
{
    private readonly ISupplierProvider _supplierProvider;
    public SupplierService(ISupplierProvider supplierProvider){
        _supplierProvider = supplierProvider;
    }

    public Supplier[] GetSuppliers(){
        return _supplierProvider.Get();
    }

    public Supplier? GetSupplierById(int id){
        Supplier[] suppliers = _supplierProvider.Get();
        Supplier? supplier = suppliers.FirstOrDefault(sup => sup.Id == id);
        return supplier;
    }
}
