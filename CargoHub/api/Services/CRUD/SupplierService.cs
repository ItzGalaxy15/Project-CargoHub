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
        string now = supplier.GetTimeStamp();
        supplier.CreatedAt = now;
        supplier.UpdatedAt = now;
        _supplierProvider.Add(supplier);
        await _supplierProvider.Save();
        return true;
    }

    public async Task DeleteSupplier(Supplier supplier){
        _supplierProvider.Delete(supplier);
        await _supplierProvider.Save();
    }

    public async Task ReplaceSupplier(Supplier supplier, int supplierId){
        string now = supplier.GetTimeStamp();
        supplier.CreatedAt = now;
        supplier.UpdatedAt = now;

        _supplierProvider.Replace(supplier, supplierId);
        await _supplierProvider.Save();
    }
}
