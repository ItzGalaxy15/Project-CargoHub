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

    public async Task<bool> AddSupplier(Supplier supplier){
        // check if supplier is valid, like no duplicate id, else return false
        Supplier[] suppliers = _supplierProvider.Get();
        if (suppliers.Any(sup => sup.Id == supplier.Id)) return false;

        string now = supplier.GetTimeStamp();
        supplier.CreatedAt = now;
        supplier.UpdatedAt = now;
        _supplierProvider.Add(supplier);
        await _supplierProvider.Save();
        return true;
    }
}
